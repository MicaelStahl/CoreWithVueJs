using CoreWithVueJs.Models.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWithVueJs.Business.Factories.Interfaces
{
    /// <summary>
    /// Exposes a CRUD Factory used to create content based on <see cref="IBase"/>
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// Creates a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type</typeparam>
        /// <param name="model">The model to create</param>
        /// <returns>Returns a <typeparamref name="TModel"/> with updated IDs</returns>
        public Task<TModel> CreateAsync<TModel>(TModel model) where TModel : IBase;
        /// <summary>
        /// Returns a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to return</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a content of <typeparamref name="TModel"/> or <see cref="null"/> if none was found</returns>
        public Task<TModel> GetAsync<TModel>(Guid ID) where TModel : IBase;
        /// <summary>
        /// Returns a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to return</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a content of <typeparamref name="TModel"/> or <see cref="null"/> if none was found</returns>
        public Task<TModel> GetAsync<TModel>(int ID) where TModel : IBase;
        /// <summary>
        /// Updates a content of type <typeparamref name="TModel"/>
        /// </summary>
        /// <typeparam name="TModel">The <typeparamref name="TModel"/> type to attempt to update</typeparam>
        /// <param name="ID">The ID</param>
        /// <returns>Returns a updated content of <typeparamref name="TModel"/></returns>
        public Task<TModel> UpdateAsync<TModel>(TModel model) where TModel : IBase;
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

        /// <summary>
        /// Gets all existing <see cref="IComment"/>s inside of the database
        /// </summary>
        /// <returns>Returns a list of <see cref="IComment"/></returns>
        public Task<IEnumerable<T>> GetAllOfTypeAsync<T>() where T : IBase;
    }
}
