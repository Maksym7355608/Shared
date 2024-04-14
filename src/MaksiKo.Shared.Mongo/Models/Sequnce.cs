using MongoDB.Bson.Serialization.Attributes;

namespace MaksiKo.Shared.Mongo.Models;

public class Sequnce<T> : BaseMongoDomainModel where T : struct
{
    public T Value { get; set; }

    [BsonId]
    public string Name { get; set; }
}