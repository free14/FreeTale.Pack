using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// human readable text writer
    /// </summary>
    public class Packer
    {
        public string IndentString { get; set; } = "\t";
        public int Indent { get; set; }
    }
}
