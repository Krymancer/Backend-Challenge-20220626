using Domain.SeedWork.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Products
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Barcode { get; set; }
        public EnumProductStatus status { get; set; }
        public DateTime Imported { get; set; }
        public string? Url { get; set; }
        public string? ProductName { get; set; }
        public string? Quantity { get; set; }
        public string? Categories { get; set; }
        public string? Packaging { get; set; }
        public string? Brands { get; set; }
        public string? ImageUrl { get; set; }
    }
}