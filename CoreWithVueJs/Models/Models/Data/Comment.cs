using CoreWithVueJs.Models.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreWithVueJs.Models.Models.Data
{
    /// <summary>
    /// A Comment used by users to give out a written opinion about a post or another comment
    /// </summary>
    public class Comment : IComment, IEquatable<Comment>
    {
        [Key]
        public Guid GUID { get; set; }

        [Key]
        public int ID { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Text { get; set; }

        public ICollection<ILike> Likes { get; set; }

        public ICollection<ILike> Dislikes { get; set; }

        public ICollection<IComment> Replies { get; set; }

        public bool Equals(Comment other)
        {
            if (other == null)
            {
                return false;
            }

            return GUID == other.GUID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Comment comment))
            {
                return false;
            }
            else
            {
                return Equals(comment);
            }
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
