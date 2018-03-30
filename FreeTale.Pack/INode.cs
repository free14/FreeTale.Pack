using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// base interface for node
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// child node
        /// </summary>
        INode[] SubNode { get; set; }

        /// <summary>
        /// node attribute. for langage like XML
        /// </summary>
        IAttribute[] Attribute { get; set; }
        /// <summary>
        /// node name.
        /// </summary>
        Writable Name { get; set; }

        /// <summary>
        /// node value.
        /// </summary>
        Writable Value { get; set; }
    }
}
