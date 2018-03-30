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
        public INode[] SubNode { get; set; }
        public IAttribute[] Attribute { get; set; }
        public Writeable Name { get; set; }
    }
}
