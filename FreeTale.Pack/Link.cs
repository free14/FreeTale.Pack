using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public class Link : ILink
    {
        public INode From { get; set; }
        public INode To { get; set; }
    }
}
