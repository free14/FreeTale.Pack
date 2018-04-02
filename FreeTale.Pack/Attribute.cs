using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// base class of IAttribute
    /// </summary>
    public class Attribute : IAttribute
    {
        public Writable Name { get; set; }
        public Writable Value { get; set; }
    }
}
