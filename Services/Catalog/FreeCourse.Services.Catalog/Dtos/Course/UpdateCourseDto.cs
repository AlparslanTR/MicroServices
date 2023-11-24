using FreeCourse.Services.Catalog.Dtos.Feature;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public record UpdateCourseDto(
        string Id,
        string Name,
        decimal Price,
        string Title,
        string Description,
        string PictureUrl,
        string UserId,
        ListFeatureDto Feature,
        string CategoryId
        )
    {}
}
