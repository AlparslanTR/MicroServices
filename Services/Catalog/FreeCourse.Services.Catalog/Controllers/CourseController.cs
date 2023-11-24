using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Services.CourseService;
using FreeCourse.Shared.BaseController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _courseService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResultInstance(await _courseService.GetByIdAsync(id));
        }

        [HttpGet("User{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            return CreateActionResultInstance(await _courseService.GetAllByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCourseDto courseCreateDto)
        {
            return CreateActionResultInstance(await _courseService.CreateAsync(courseCreateDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseDto courseUpdateDto)
        {
            return CreateActionResultInstance(await _courseService.UpdateAsync(courseUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return CreateActionResultInstance(await _courseService.DeleteAsync(id));
        }

    }
}
