using CoreWithVueJs.Business.Database;
using CoreWithVueJs.Business.Factories.Interfaces;
using CoreWithVueJs.Models.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CoreWithVueJs.Business.Factories
{
    /// <summary>
    /// Not meant to be used directly in code. Use <see cref="IDataFactory"/> to expose the factory.
    /// </summary>
    public abstract class DataFactory : IDataFactory
    {
        private const string CONTENT_CACHE_KEY = "__cache_content_{0}";
        private const int CACHE_EXPIRATION_IN_MINUTES = 15;

        public async Task<IEnumerable<IPost>> GetAllPostsAsync()
        {
            using CoreDbContext context = CoreDbContext.Default;

            return await context.Posts.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllOfTypeAsync<T>() where T : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;

            return await context.Entities.OfType<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<TModel> CreateAsync<TModel>(TModel model) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;

            var entity = await context.Entities.AddAsync(model).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            ObjectCache cache = MemoryCache.Default;

            var content = (TModel)entity.Entity;

            cache.Set(string.Format(CONTENT_CACHE_KEY, content.GUID), content, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return content;
        }

        public async Task<TModel> GetAsync<TModel>(Guid ID) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
            {
                return (TModel)cache.Get(string.Format(CONTENT_CACHE_KEY, ID));
            }
            else
            {
                var comment = await context.Entities.FindAsync(ID).ConfigureAwait(false);

                cache.Set(string.Format(CONTENT_CACHE_KEY, ID), comment, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

                return (TModel)comment;
            }
        }

        public async Task<TModel> GetAsync<TModel>(int ID) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;

            return (TModel)await context.Entities.FindAsync(ID).ConfigureAwait(false);
        }

        public async Task<TModel> UpdateAsync<TModel>(TModel model) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var state = context.Entities.Update(model);

            await context.SaveChangesAsync().ConfigureAwait(false);

            cache.Set(string.Format(CONTENT_CACHE_KEY, model.GUID), state.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return (TModel)state.Entity;
        }

        public async Task<bool> DeleteAsync<TModel>(Guid ID) where TModel : IBase
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                IBase entity = null;
                ObjectCache cache = MemoryCache.Default;

                if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
                {
                    entity = (TModel)cache.Get(string.Format(CONTENT_CACHE_KEY, ID));
                    cache.Remove(string.Format(CONTENT_CACHE_KEY, ID));
                }
                else
                {
                    entity = await context.Entities.FindAsync(ID).ConfigureAwait(false);
                }

                var entry = context.Entities.Remove(entity);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entry.State == EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> DeleteAsync<TModel>(int ID) where TModel : IBase
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                ObjectCache cache = MemoryCache.Default;

                var entity = await context.Entities.FindAsync(ID).ConfigureAwait(false);

                if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
                {
                    cache.Remove(string.Format(CONTENT_CACHE_KEY, ID));
                }

                var entry = context.Entities.Remove(entity);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entry.State == EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(Guid ID) where T : ILikesDislikes
        {
            using CoreDbContext context = CoreDbContext.Default;

            var model = await context.Entities.FindAsync(ID).ConfigureAwait(false) as ILikesDislikes;

            return (new ReadOnlyCollection<ILike>(model.Dislikes.ToList()), new ReadOnlyCollection<ILike>(model.Likes.ToList()));
        }

        public async Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(int ID) where T : ILikesDislikes
        {
            using CoreDbContext context = CoreDbContext.Default;

            var model = await context.Entities.FindAsync(ID).ConfigureAwait(false) as ILikesDislikes;

            return (new ReadOnlyCollection<ILike>(model.Dislikes.ToList()), new ReadOnlyCollection<ILike>(model.Likes.ToList()));
        }
    }
}
