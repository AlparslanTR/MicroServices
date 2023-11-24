using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catalog.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Created { get; set; }
        public string UserId { get; set; } = null!;

        // İlişkiler
        public Feature? Feature { get; set; }
        public string? CategoryId { get; set; }

        [BsonIgnore]
        public Category? Category { get; set; }
    }
}
