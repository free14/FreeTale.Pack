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
        Writable Comment { get; set; }
        /// <summary>
        /// document version
        /// </summary>
        Writable Version { get; set; }
        /// <summary>
        /// Document subnode 
        /// </summary>
        INode[] SubNode { get; set; }
    }
}
