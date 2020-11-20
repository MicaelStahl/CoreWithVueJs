using System;
using System.Collections.Generic;

namespace CoreWithVueJs.Models.Interfaces.Base
{
    /// <summary>
    /// Contains basic properties used for comments
    /// </summary>
    public interface IComment : IBase, ILikesDislikes
    {
        public string Text { get; set; }
        public ICollection<IComment> Replies { get; set; }
    }
}
