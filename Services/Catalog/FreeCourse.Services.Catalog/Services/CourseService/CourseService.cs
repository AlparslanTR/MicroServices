using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services.CategoryService;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _courseCollection = InitializeDatabase(databaseSettings);
            _mapper = mapper;
            _categoryCollection = InitializeDatabaseForCategory(databaseSettings);
        }
        private IMongoCollection<Course> InitializeDatabase(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            if (!database.ListCollectionNames().ToList().Contains(databaseSettings.CourseTableName))
            {
                database.CreateCollection(databaseSettings.CourseTableName);
            }
            return database.GetCollection<Course>(databaseSettings.CourseTableName);
        }

        private IMongoCollection<Category> InitializeDatabaseForCategory(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            return database.GetCollection<Category>(databaseSettings.CategoryTableName);
        }

        public async Task<ResponseDto<List<ListCourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection
                           .Find(course => true)
                           .SortByDescending(x => x.Name)
                           .ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection
                                       .Find<Category>(x => x.Id == course.CategoryId)
                                       .FirstOrDefaultAsync();
                }
            }

            if (!courses.Any() || courses is null)
            {
                return ResponseDto<List<ListCourseDto>>
                    .CreateFail("Kurs Bulunamadı Veya Datanız Yok.!",
                        HttpStatusCode.NotFound);
            }
            return ResponseDto<List<ListCourseDto>>
                .CreateSuccess(
                    _mapper.Map<List<ListCourseDto>>(courses),
                    "Kurs Listesi Getirildi.",
                    HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ListCourseDto>> GetByIdAsync(string id)
        {
            var getByCourse = await _courseCollection
                              .Find(x => x.Id == id)
                              .FirstOrDefaultAsync();
            if (getByCourse is null || id != getByCourse.Id)
            {
                return ResponseDto<ListCourseDto>
                    .CreateFail(
                        "Kurs Bulunamadı Veya Yanlış Değer Girildi.!",
                        HttpStatusCode.NotFound);
            }

            getByCourse.Category = await _categoryCollection
                                    .Find<Category>(x => x.Id == getByCourse.CategoryId)
                                    .FirstOrDefaultAsync();

            return ResponseDto<ListCourseDto>
                .CreateSuccess(
                    _mapper.Map<ListCourseDto>(getByCourse),
                    $"{getByCourse.Title} Adlı Kurs Getirildi",
                    HttpStatusCode.OK);
        }

        public async Task<ResponseDto<AddCourseDto>> CreateAsync(Course course)
        {
            await _courseCollection.InsertOneAsync(course);
            if (course is null)
            {
                return ResponseDto<AddCourseDto>
                 .CreateFail(
                    "Kategori Eklenemedi",
                    HttpStatusCode.BadRequest);
            }
            return ResponseDto<AddCourseDto>
                .CreateSuccess(
                    _mapper.Map<AddCourseDto>(course),
                    "Kategori Oluşturuldu",
                    HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ListCourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = _courseCollection.Find(x => x.UserId == userId).ToList();
            if (courses is null || !courses.Any())
            {
                return ResponseDto<List<ListCourseDto>>
                    .CreateFail(
                        "Kurs Bulunamadı Veya Datanız Yok.!",
                        HttpStatusCode.NotFound);
            }

            foreach (var course in courses)
            {
                course.Category = await _categoryCollection
                                   .Find<Category>(x => x.Id == course.CategoryId)
                                   .FirstOrDefaultAsync();
            }

            return ResponseDto<List<ListCourseDto>>
                .CreateSuccess(
                    _mapper.Map<List<ListCourseDto>>(courses),
                    "Kurs Listesi Getirildi.",
                    HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ListCourseDto>> CreateAsync(AddCourseDto addCourseDto)
        {
            var newCourse = _mapper.Map<Course>(addCourseDto);
            newCourse.Created = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            if (newCourse is null)
            {
                return ResponseDto<ListCourseDto>
                 .CreateFail(
                    "Kurs Eklenemedi",
                    HttpStatusCode.BadRequest);
            }
            return ResponseDto<ListCourseDto>
                .CreateSuccess(
                    _mapper.Map<ListCourseDto>(newCourse),
                    $"{newCourse.Title} Adlı Kurs Oluşturuldu",
                    HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ListCourseDto>> UpdateAsync(UpdateCourseDto updateCourseDto)
        {
            var updateCourse = _mapper.Map<Course>(updateCourseDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourse.Id, updateCourse);
            if (result is null)
            {
                return ResponseDto<ListCourseDto>
                 .CreateFail(
                    "Kurs Güncellenemedi",
                    HttpStatusCode.BadRequest);
            }
            return ResponseDto<ListCourseDto>.CreateSuccess(null,$"{result.Name} Adlı Kurs Güncellendi",HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ListCourseDto>> DeleteAsync(string id)
        {
            var course = await _courseCollection.FindOneAndDeleteAsync(x => x.Id == id);
            if (course is null)
            {
                return ResponseDto<ListCourseDto>
                       .CreateFail(
                       "Kurs Bulunamadı",
                        HttpStatusCode.BadRequest);
            }
            return ResponseDto<ListCourseDto>
                .CreateSuccess(
                    null,
                    $"{course.Name} Adlı Kurs Silindi",
                    HttpStatusCode.OK);
        }
    }
}
