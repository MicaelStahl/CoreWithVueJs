using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace CoreWithVueJs.Models.Interfaces.Base
{
    public interface IPost : IBase, ILikesDislikes
    {
        public string Title { get; set; }

        /// <summary>
        /// Automatically receives the "fullname" property from a User upon creation.
        /// </summary>
        public string Creator { get; }

        public HtmlString MainText { get; set; }

        public ICollection<IComment> Comments { get; set; }
    }
}
