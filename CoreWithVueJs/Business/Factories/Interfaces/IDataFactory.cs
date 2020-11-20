using CoreWithVueJs.Models.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWithVueJs.Business.Factories.Interfaces
{
    /// <summary>
    /// Exposes a GUID Factory used to create content based on <see cref="IBase"/>
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Creates a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type</typeparam>
        /// <param name="model">The model to create</param>
        /// <returns>Returns a <typeparamref name="TModel"/> with updated IDs</returns>
        public Task<IBase> CreateAsync<TModel>(TModel model) where TModel : IBase;
        /// <summary>
        /// Returns a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to return</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a content of <typeparamref name="TModel"/> or <see cref="null"/> if none was found</returns>
        public Task<IBase> GetAsync<TModel>(Guid ID) where TModel : IBase;
        /// <summary>
        /// Returns a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to return</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a content of <typeparamref name="TModel"/> or <see cref="null"/> if none was found</returns>
        public Task<IBase> GetAsync<TModel>(int ID) where TModel : IBase;
        /// <summary>
        /// Updates a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to update</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a updated content of <typeparamref name="TModel"/></returns>
        public Task<IBase> UpdateAsync<TModel>(TModel model) where TModel : IBase;
        /// <summary>
        /// Deletes a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to delete</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a boolean indicating whether the content was successfully removed or not</returns>
        public Task<bool> DeleteAsync<TModel>(Guid ID) where TModel : IBase;
        /// <summary>
        /// Deletes a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to delete</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a boolean indicating whether the content was successfully removed or not</returns>
        public Task<bool> DeleteAsync<TModel>(int ID) where TModel : IBase;
        /// <summary>
        /// Gets the likes and dislikes of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="ID">The GUID</param>
        /// <returns>Returns a <see cref="(ILike Dislikes, ILike Likes)"/></returns>
        public Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(Guid ID) where T : ILikesDislikes;

        /// <summary>
        /// Gets the likes and dislikes of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="ID">The int ID</param>
        /// <returns>Returns a <see cref="(ILike Dislikes, ILike Likes)"/></returns>
        public Task<(IReadOnlyCollection<ILike> Dislikes, IReadOnlyCollection<ILike> Likes)> GetLikesDislikesAsync<T>(int ID) where T : ILikesDislikes;

        // Old
        /// <summary>
        /// Creates a <see cref="IPost"/>
        /// </summary>
        /// <param name="post">The post</param>
        /// <returns>Returns the created post</returns>
        public Task<IPost> CreatePostAsync(IPost post);

        /// <summary>
        /// Creates a <see cref="IComment"/>
        /// </summary>
        /// <param name="comment">The comment</param>
        /// <returns>Returns the created comment</returns>
        public Task<IComment> CreateCommentAsync(IComment comment);
        /// <summary>
        /// Gets a post given the <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The GUID</param>
        /// <returns>Returns the post with the given ID</returns>
        public Task<IPost> GetPostAsync(Guid ID);

        /// <summary>
        /// Gets a post given the <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns the post with the given ID</returns>
        public Task<IPost> GetPostAsync(int ID);

        /// <summary>
        /// Gets a comment given the <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The GUID</param>
        /// <returns>Returns the comment with the given ID</returns>
        public Task<IComment> GetCommentAsync(Guid ID);

        /// <summary>
        /// Gets a comment given the <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns the comment with the given ID</returns>
        public Task<IComment> GetCommentAsync(int ID);

        /// <summary>
        /// Updates the requested post with new params
        /// </summary>
        /// <param name="post">The post to update with</param>
        /// <returns>Returns a updated post with the new values</returns>
        public Task<IPost> UpdatePostAsync(IPost post);

        /// <summary>
        /// Updates the requested comment with new params
        /// </summary>
        /// <param name="comment">The comment to update with</param>
        /// <returns>Returns a updated post with the new values</returns>
        public Task<IComment> UpdateCommentAsync(IComment comment);

        /// <summary>
        /// Deletes a post with the existing <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a bool indicating whether the post was successfully removed</returns>
        public Task<bool> DeletePostAsync(Guid ID);

        /// <summary>
        /// Deletes a post with the existing <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a bool indicating whether the post was successfully removed</returns>
        public Task<bool> DeletePostAsync(int ID);

        /// <summary>
        /// Deletes a comment with the existing <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a bool indicating whether the comment was successfully removed</returns>
        public Task<bool> DeleteCommentAsync(Guid ID);

        /// <summary>
        /// Deletes a comment with the existing <paramref name="ID"/>
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a bool indicating whether the comment was successfully removed</returns>
        public Task<bool> DeleteCommentAsync(int ID);
    }
}
