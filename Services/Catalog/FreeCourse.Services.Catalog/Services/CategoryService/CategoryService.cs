using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;
            _categoryCollection = InitializeDatabase(databaseSettings);
        }

        private IMongoCollection<Category> InitializeDatabase(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            if(!database.ListCollectionNames().ToList().Contains(databaseSettings.CategoryTableName))
            {
                database.CreateCollection(databaseSettings.CategoryTableName);
            }
            return database.GetCollection<Category>(databaseSettings.CategoryTableName);
        }
        public async Task<ResponseDto<List<ListCategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection
                            .Find(category => true)
                            .SortByDescending(x=>x.Name)
                            .ToListAsync();
           
            if(!categories.Any() || categories is null)
            {
                return ResponseDto<List<ListCategoryDto>>
                    .CreateFail("Kategori Bulunamadı Veya Datanız Yok.!",
                        HttpStatusCode.NotFound);
            }
            return ResponseDto<List<ListCategoryDto>>
                .CreateSuccess(
                    _mapper.Map<List<ListCategoryDto>>(categories),
                    "Kategori Listesi Getirildi.",
                    HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ListCategoryDto>> GetByIdAsync(string id)
        {
            var getByCategory = await _categoryCollection
                                .Find(x => x.Id == id)
                                .FirstOrDefaultAsync();
            if (getByCategory is null || id != getByCategory.Id)
            {
                return ResponseDto<ListCategoryDto>
                    .CreateFail(
                        "Kategori Bulunamadı Veya Yanlış Değer Girildi.!",
                        HttpStatusCode.NotFound);
            }
            return ResponseDto<ListCategoryDto>
                .CreateSuccess(
                    _mapper.Map<ListCategoryDto>(getByCategory),
                    $"{getByCategory.Name} Adlı Kategori Getirildi",
                    HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ListCategoryDto>> CreateAsync(ListCategoryDto category)
        {
            var categoryMap = _mapper.Map<Category>(category);
            await _categoryCollection.InsertOneAsync(categoryMap);
            if(categoryMap is not null)
            {
                return ResponseDto<ListCategoryDto>
                        .CreateSuccess(
                            _mapper.Map<ListCategoryDto>(categoryMap),
                            $"{categoryMap.Name} Adlı Kategori Oluşturuldu",
                            HttpStatusCode.Created);
            }    
            return ResponseDto<ListCategoryDto>
                    .CreateFail(
                       "Kategori Eklenemedi",
                       HttpStatusCode.BadRequest);
        }

        
    }
}
