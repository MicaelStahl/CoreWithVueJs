using System;

namespace CoreWithVueJs.Models.Interfaces.Base
{
    /// <summary>
    /// Holds the most basic of properties used for the portal.
    /// </summary>
    public interface IBase
    {
        public Guid GUID { get; set; }

        public int ID { get; set; }

        public DateTime Created { get; set; }
    }
}
