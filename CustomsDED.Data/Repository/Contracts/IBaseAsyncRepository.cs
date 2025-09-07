namespace CustomsDED.Data.Repository.Contracts
{
    public interface IBaseAsyncRepository<TEntity> where TEntity : class, new()
    {
        Task<bool> AddAsync(TEntity entity);

        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(object primaryKey);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> DeleteAsync(TEntity entity);

    }
}
