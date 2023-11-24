using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FreeCourse.Services.Catalog.Dtos.Feature;
using FreeCourse.Services.Catalog.Dtos.Category;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public record ListCourseDto
        (
        string Id,
        string Name,
        decimal Price,
        string Title,
        string Description,
        string PictureUrl,
        DateTime Created,
        string UserId,
        ListFeatureDto Feature,
        string CategoryId,
        ListCategoryDto Category
        )
    {}
}
