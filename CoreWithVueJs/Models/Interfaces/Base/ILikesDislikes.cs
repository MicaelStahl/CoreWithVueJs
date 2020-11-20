using System.Collections.Generic;

namespace CoreWithVueJs.Models.Interfaces.Base
{
    /// <summary>
    /// Interface used to identity content containing Likes and Dislikes
    /// </summary>
    public interface ILikesDislikes
    {
        public ICollection<ILike> Likes { get; set; }
        public ICollection<ILike> Dislikes { get; set; }
    }
}
