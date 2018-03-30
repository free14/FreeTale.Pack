using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public class Node : INode
    {
        public INode[] SubNode { get; set; }
        public IAttribute[] Attribute { get; set; }
        public Writable Name { get; set; }
        public Writable Value { get; set; }
    }
}
