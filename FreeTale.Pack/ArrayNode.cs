using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// node object as array
    /// </summary>
    public class ArrayNode : INode
    {
        public INode this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public INode this[string index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<INode> SubNode { get; set; }
        public List<IAttribute> Attribute { get; set; }
        public Writable Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Writable Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Writable INode.Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
