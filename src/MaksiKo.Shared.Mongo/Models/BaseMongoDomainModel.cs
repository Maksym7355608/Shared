namespace MaksiKo.Shared.Mongo.Models;

public class BaseMongoDomainModel
{
    protected BaseMongoDomainModel() => this.Version = 1;

    public int Version { get; set; }
}