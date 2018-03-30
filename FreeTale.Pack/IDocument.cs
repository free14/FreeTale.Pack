using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// for some format require document
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// document comment
        /// </summary>
        Writeable Comment { get; set; }
        /// <summary>
        /// document version
        /// </summary>
        Writeable Version { get; set; }
        /// <summary>
        /// Document subnode 
        /// </summary>
        INode[] SubNode { get; set; }
    }
}
