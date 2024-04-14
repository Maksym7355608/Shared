using MaksiKo.Shared.Mongo.Models;

namespace MaksiKo.Shared.Mongo;

public interface IMongoCollectionRepository<TCollection, in TId> where TCollection : BaseMongoEntity<TId> 
{
    Task<TCollection> GetAsync(TId id);
    Task<IEnumerable<TCollection>> GetAllAsync();
    Task<IEnumerable<TCollection>> SearchAsync(object filter);
    Task<bool> CreateAsync(TCollection entity);
    Task<bool> ReplaceAsync(TCollection entity);
    Task<bool> UpdateAsync(object filter);
    Task<bool> DeleteAsync(TId id);
}