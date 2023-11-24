using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services.CourseService
{
    public interface ICourseService
    {
        Task<ResponseDto<List<ListCourseDto>>> GetAllAsync();
        Task<ResponseDto<ListCourseDto>> GetByIdAsync(string id);
        Task<ResponseDto<AddCourseDto>> CreateAsync(Course course);
        Task<ResponseDto<List<ListCourseDto>>> GetAllByUserIdAsync(string userId);
        Task<ResponseDto<ListCourseDto>> CreateAsync(AddCourseDto addCourseDto);
        Task<ResponseDto<ListCourseDto>> UpdateAsync(UpdateCourseDto updateCourseDto);
        Task<ResponseDto<ListCourseDto>> DeleteAsync(string id);
    }
}
