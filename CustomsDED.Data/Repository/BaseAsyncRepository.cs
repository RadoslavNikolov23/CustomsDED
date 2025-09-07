namespace CustomsDED.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Repository.Contracts;

    public class BaseAsyncRepository<TEntity> : IBaseAsyncRepository<TEntity> where TEntity : class, new()
    {
        internal readonly CustomsDB dbContext;

        public BaseAsyncRepository(CustomsDB dbContext)
        {
            this.dbContext = dbContext;
        }

        internal async Task EnsureInitialized()
        {
            await dbContext.Init();
        }


        public async Task<bool> AddAsync(TEntity entity)
        {
            await EnsureInitialized();

            int result = 0;

            try
            {

                result = await this.dbContext.Database
                                             .InsertAsync(entity);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddAsync, in the BaseAsyncRepository class.");
                throw;
            }

            return result > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await EnsureInitialized();
            int result = 0;

            try
            {
                result = await this.dbContext.Database
                                             .InsertAllAsync(entities);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddRangeAsync, in the BaseAsyncRepository class.");
                throw;
            }


            return result == entities.Count();
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            await EnsureInitialized();
            int result = 0;

            try
            {
                result = await this.dbContext.Database
                                             .UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in UpdateAsync, in the BaseAsyncRepository class.");
                throw;
            }

            return result > 0;
        }

        public async Task<TEntity?> GetByIdAsync(object primaryKey)
        {
            await EnsureInitialized();
            TEntity? entity = null;

            try
            {
                entity = await this.dbContext.Database
                                             .FindAsync<TEntity>(primaryKey);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetByIdAsync, in the BaseAsyncRepository class.");
                throw;
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            await EnsureInitialized();

            IEnumerable<TEntity> entities = new List<TEntity>();

            try
            {

                entities = await this.dbContext.Database
                                               .Table<TEntity>()
                                               .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllAsync, in the BaseAsyncRepository class.");
                throw;
            }

            return entities;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            await EnsureInitialized();
            int result = 0;

            try
            {
                result = await this.dbContext.Database
                                             .DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddSchedule, in the BaseAsyncRepository class.");
                throw;
            }
            return result > 0;
        }
    }
}
