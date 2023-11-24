using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Dtos.Feature;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public record AddCourseDto
        (
        string Name,
        decimal Price,
        string Title,
        string Description,
        DateTime Created,
        string PictureUrl,
        string UserId,
        ListFeatureDto Feature,
        string CategoryId
        )
    {}
}
