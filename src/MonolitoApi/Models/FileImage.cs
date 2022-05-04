using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonolitoApi.Models
{
    public class FileImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }
        public string FileContent { get; set; }
    }
}