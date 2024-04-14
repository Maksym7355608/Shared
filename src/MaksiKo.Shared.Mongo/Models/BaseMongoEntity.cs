using MongoDB.Bson.Serialization.Attributes;

namespace MaksiKo.Shared.Mongo.Models;

public class BaseMongoEntity<T> : BaseMongoDomainModel
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public T Id { get; set; }
}