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
        List<INode> SubNode { get; set; }

        /// <summary>
        /// node attribute. for langage like XML
        /// </summary>
        List<IAttribute> Attribute { get; set; }
        /// <summary>
        /// node name.
        /// </summary>
        Writable Name { get; set; }

        /// <summary>
        /// node value.
        /// </summary>
        Writable Value { get; set; }

        /// <summary>
        /// node is flag as comment
        /// </summary>
        bool IsComment { get; set; }

        /// <summary>
        /// get or set value by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        INode this[int index] { get;set; }

        /// <summary>
        /// get or set subnode by name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        INode this[string index] { get;set; }

        /// <summary>
        /// safe add subnode avoid SubNode is null
        /// </summary>
        /// <param name="node">SubNode to add</param>
        void Add(INode node);

        /// <summary>
        /// safe add SubNode by name-value
        /// </summary>
        /// <param name="name">name of subnode</param>
        /// <param name="value">value of subnode</param>
        void Add(Writable name, Writable value);

        /// <summary>
        /// safe add SubNode thas operation create Tree of SubNode
        /// </summary>
        /// <param name="name">Name list to add</param>
        /// <param name="value">value of last node</param>
        void Add(Writable[] name, Writable value);
    }
}
