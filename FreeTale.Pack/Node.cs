using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public class Node : INode
    {
        public INode[] SubNode { get; set; }
        public IAttribute[] Attribute { get; set; }
        public Writeable Name { get; set; }
        public Writeable Value { get; set; }
    }
}
