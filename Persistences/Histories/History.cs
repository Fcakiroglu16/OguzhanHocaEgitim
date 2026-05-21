using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistences.Histories;

public class History
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string TableName { get; set; } = null!;


    public string OldRow { get; set; } = null!;

    public string NewRow { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int UserId { get; set; }
}