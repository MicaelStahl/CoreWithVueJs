using CoreWithVueJs.Models.Interfaces.Base;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreWithVueJs.Models.Models.Data
{
    public class Post : IPost
    {
        [Key]
        public Guid GUID { get; set; }

        [Key]
        public int ID { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Automatically receives the "fullname" property from a User upon creation.
        /// </summary>
        public string Creator { get; }

        [Required]
        public HtmlString MainText { get; set; }

        public ICollection<ILike> Likes { get; set; }

        public ICollection<ILike> Dislikes { get; set; }

        public ICollection<IComment> Comments { get; set; }
    }
}
