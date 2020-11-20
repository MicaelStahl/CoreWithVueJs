using CoreWithVueJs.Business.Database;
using CoreWithVueJs.Business.Factories.Interfaces;
using CoreWithVueJs.Models.Interfaces.Base;
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
        private const string COMMENT_CACHE_KEY = "__cache_comment_{0}";
        private const string POST_CACHE_KEY = "__cache_post_{0}";
        private const string CONTENT_CACHE_KEY = "__cache_content_{0}";
        private const int CACHE_EXPIRATION_IN_MINUTES = 15;

        public async Task<IBase> CreateAsync<TModel>(TModel model) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;

            var entity = await context.AddAsync(model).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            ObjectCache cache = MemoryCache.Default;

            var content = (TModel)entity.Entity;

            cache.Set(string.Format(CONTENT_CACHE_KEY, content.GUID), content, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return content;
        }

        public async Task<IBase> GetAsync<TModel>(Guid ID) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
            {
                return (TModel)cache.Get(string.Format(CONTENT_CACHE_KEY, ID));
            }
            else
            {
                var comment = await context.FindAsync<IBase>(ID).ConfigureAwait(false);

                cache.Set(string.Format(CONTENT_CACHE_KEY, ID), comment, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

                return (TModel)comment;
            }
        }

        public async Task<IBase> GetAsync<TModel>(int ID) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;

            return (TModel)await context.FindAsync<IBase>(ID).ConfigureAwait(false);
        }

        public async Task<IBase> UpdateAsync<TModel>(TModel model) where TModel : IBase
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var state = context.Update<IBase>(model);

            await context.SaveChangesAsync().ConfigureAwait(false);

            cache.Set(string.Format(CONTENT_CACHE_KEY, model.GUID), state.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return (TModel)state.Entity;
        }

        public async Task<bool> DeleteAsync<TModel>(Guid ID) where TModel : IBase
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                IBase comment = null;
                ObjectCache cache = MemoryCache.Default;

                if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
                {
                    comment = (TModel)cache.Get(string.Format(CONTENT_CACHE_KEY, ID));
                    cache.Remove(string.Format(CONTENT_CACHE_KEY, ID));
                }
                else
                {
                    comment = await context.FindAsync<IBase>(ID).ConfigureAwait(false);
                }

                var entity = context.Remove(comment);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
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

                var comment = await context.FindAsync<IBase>(ID).ConfigureAwait(false);

                if (cache.Contains(string.Format(CONTENT_CACHE_KEY, ID)))
                {
                    cache.Remove(string.Format(CONTENT_CACHE_KEY, ID));
                }

                var entity = context.Remove(comment);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(Guid ID) where T : ILikesDislikes
        {
            using CoreDbContext context = CoreDbContext.Default;

            var model = await context.FindAsync<ILikesDislikes>(ID).ConfigureAwait(false);

            return (new ReadOnlyCollection<ILike>(model.Dislikes.ToList()), new ReadOnlyCollection<ILike>(model.Likes.ToList()));
        }

        public async Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(int ID) where T : ILikesDislikes
        {
            using CoreDbContext context = CoreDbContext.Default;

            var model = await context.FindAsync<ILikesDislikes>(ID).ConfigureAwait(false);

            return (new ReadOnlyCollection<ILike>(model.Dislikes.ToList()), new ReadOnlyCollection<ILike>(model.Likes.ToList()));
        }

        public async Task<IComment> CreateCommentAsync(IComment comment)
        {
            using CoreDbContext context = CoreDbContext.Default;

            var entity = await context.AddAsync(comment).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            ObjectCache cache = MemoryCache.Default;

            cache.Set(string.Format(COMMENT_CACHE_KEY, entity.Entity.GUID), entity.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return entity.Entity;
        }

        public async Task<IPost> CreatePostAsync(IPost post)
        {
            using CoreDbContext context = CoreDbContext.Default;

            var entity = await context.Posts.AddAsync(post).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            ObjectCache cache = MemoryCache.Default;

            cache.Set(string.Format(POST_CACHE_KEY, entity.Entity.GUID), entity.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return entity.Entity;
        }

        public async Task<bool> DeleteCommentAsync(Guid ID)
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                IComment comment = null;
                ObjectCache cache = MemoryCache.Default;

                if (cache.Contains(string.Format(COMMENT_CACHE_KEY, ID)))
                {
                    comment = cache.Get(string.Format(COMMENT_CACHE_KEY, ID)) as IComment;
                    cache.Remove(string.Format(COMMENT_CACHE_KEY, ID));
                }
                else
                {
                    comment = await context.FindAsync<IComment>(ID).ConfigureAwait(false);
                }

                var entity = context.Remove(comment);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> DeleteCommentAsync(int ID)
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                ObjectCache cache = MemoryCache.Default;

                var comment = await context.FindAsync<IComment>(ID).ConfigureAwait(false);

                if (cache.Contains(string.Format(COMMENT_CACHE_KEY, comment.GUID)))
                {
                    cache.Remove(string.Format(COMMENT_CACHE_KEY, comment.GUID));
                }

                var entity = context.Remove(comment);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> DeletePostAsync(Guid ID)
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                ObjectCache cache = MemoryCache.Default;
                IPost post = null;

                if (cache.Contains(string.Format(POST_CACHE_KEY, ID)))
                {
                    post = cache.Get(string.Format(POST_CACHE_KEY, ID)) as IPost;
                }
                else
                {
                    post = await context.FindAsync<IPost>(ID).ConfigureAwait(false);
                }

                var entity = context.Remove(post);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<bool> DeletePostAsync(int ID)
        {
            try
            {
                using CoreDbContext context = CoreDbContext.Default;
                ObjectCache cache = MemoryCache.Default;

                var post = await context.FindAsync<IPost>(ID).ConfigureAwait(false);

                if (cache.Contains(string.Format(POST_CACHE_KEY, post.GUID)))
                {
                    cache.Remove(string.Format(POST_CACHE_KEY, post.GUID));
                }

                var entity = context.Remove(post);

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
            }

            return false;
        }

        public async Task<IComment> GetCommentAsync(Guid ID)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(string.Format(COMMENT_CACHE_KEY, ID)))
            {
                return cache.Get(string.Format(COMMENT_CACHE_KEY, ID)) as IComment;
            }
            else
            {
                var comment = await context.FindAsync<IComment>(ID).ConfigureAwait(false);

                cache.Set(string.Format(COMMENT_CACHE_KEY, ID), comment, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

                return comment;
            }
        }

        public async Task<IComment> GetCommentAsync(int ID)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var comment = await context.FindAsync<IComment>(ID).ConfigureAwait(false);

            if (!cache.Contains(string.Format(COMMENT_CACHE_KEY, comment.GUID)))
            {
                cache.Set(string.Format(COMMENT_CACHE_KEY, ID), comment, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));
            }

            return comment;
        }

        public async Task<IPost> GetPostAsync(Guid ID)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(string.Format(POST_CACHE_KEY, ID)))
            {
                return cache.Get(string.Format(POST_CACHE_KEY, ID)) as IPost;
            }
            else
            {
                var post = await context.FindAsync<IPost>(ID).ConfigureAwait(false);

                cache.Set(string.Format(POST_CACHE_KEY, ID), post, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

                return post;
            }
        }

        public async Task<IPost> GetPostAsync(int ID)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var post = await context.FindAsync<IPost>(ID).ConfigureAwait(false);

            if (!cache.Contains(string.Format(POST_CACHE_KEY, post.GUID)))
            {
                cache.Set(string.Format(POST_CACHE_KEY, ID), post, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));
            }

            return post;
        }

        public async Task<IComment> UpdateCommentAsync(IComment comment)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var state = context.Update(comment);

            await context.SaveChangesAsync().ConfigureAwait(false);

            cache.Set(string.Format(COMMENT_CACHE_KEY, comment.GUID), state.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return state.Entity;
        }

        public async Task<IPost> UpdatePostAsync(IPost post)
        {
            using CoreDbContext context = CoreDbContext.Default;
            ObjectCache cache = MemoryCache.Default;

            var state = context.Update(post);

            await context.SaveChangesAsync().ConfigureAwait(false);

            cache.Set(string.Format(POST_CACHE_KEY, post.GUID), state.Entity, DateTimeOffset.Now.AddMinutes(CACHE_EXPIRATION_IN_MINUTES));

            return state.Entity;
        }
    }
}
