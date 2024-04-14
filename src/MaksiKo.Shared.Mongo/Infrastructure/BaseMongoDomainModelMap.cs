using MaksiKo.Shared.Mongo.Models;
using MongoDB.Bson.Serialization;

namespace MaksiKo.Shared.Mongo.Infrastructure;

public class BaseMongoDomainModelMap : IDomainClassMap
{
    public BaseMongoDomainModelMap()
    {
        BsonClassMap.RegisterClassMap<BaseMongoDomainModel>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.Version).SetIgnoreIfDefault(true);
        });
    }
}