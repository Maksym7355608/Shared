using MaksiKo.Shared.Common.Validation;
using MaksiKo.Shared.Mongo.Models;
using MongoDB.Driver;

namespace MaksiKo.Shared.Mongo.Infrastructure;

#nullable disable
public abstract class BaseMongoCollectionRepository<TCollection, TId>
    where TCollection : BaseMongoEntity<TId>, IMongoCollectionRepository<TCollection, TId>
{
    private readonly string _collectionName;
    protected readonly IUnitOfWork Work;
    protected IMongoCollection<TCollection> Collection => Work.GetCollection<TCollection>(_collectionName);
    protected FilterDefinitionBuilder<TCollection> Filter => Builders<TCollection>.Filter;

    public BaseMongoCollectionRepository(IUnitOfWork work)
    {
        Work = work;
        _collectionName = nameof(TCollection);
    }

    public BaseMongoCollectionRepository(string collectionName, IUnitOfWork work) : this(work)
    {
        _collectionName = collectionName;
    }

    public async Task<TCollection> GetAsync(TId id)
    {
        var entity = await Collection.Find(Filter.Eq(x => x.Id, id))
            .FirstOrDefaultAsync();
        if (entity == null)
            throw new EntityNotFoundException(id);
        return entity;
    }
    
    public Task<List<TCollection>> GetAllAsync()
    {
        return Collection.Find(Filter.Empty).ToListAsync();
    }

    public Task<List<TCollection>> SearchAsync(object filter)
    {
        var filters = GetSearchFilter(filter);
        return Collection.Find(filters).ToListAsync();
    }

    protected abstract FilterDefinition<TCollection> GetSearchFilter(object filter);

    public async Task<bool> CreateAsync(TCollection entity)
    {
        var duplicatesFilter = GetDuplicateSearchFilter(entity);
        var duplicateIds = await Collection.Find(duplicatesFilter)
            .Project(x => x.Id)
            .ToListAsync();
        if (duplicateIds.Count > 0)
            throw new EntityExistingException(string.Join(", ", duplicateIds));
        await Collection.InsertOneAsync(entity);
        return true;
    }

    protected abstract FilterDefinition<TCollection> GetDuplicateSearchFilter(TCollection entity);

    public async Task<bool> ReplaceAsync(TCollection entity)
    {
        var result = await Collection.ReplaceOneAsync(Filter.Eq(x => x.Id, entity.Id), entity);
        return result.ModifiedCount == 1;
    }

    public async Task<bool> UpdateAsync(object filter)
    {
        var search = GetUpdateSearchFilter(filter);
        var update = GetSet(filter);

        var result = await Collection.UpdateOneAsync(search, update);
        return result.ModifiedCount == 1;
    }

    protected abstract FilterDefinition<TCollection> GetUpdateSearchFilter(object filter);
    protected abstract UpdateDefinition<TCollection> GetSet(object filter);
    

    public async Task<bool> DeleteAsync(TId id)
    {
        var result = await Collection.DeleteOneAsync(Filter.Eq(x => x.Id, id));
        return result.DeletedCount == 1;
    }
}