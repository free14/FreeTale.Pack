using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// default class of <see cref="IDocument"/>
    /// </summary>
    public class Document : Node, IDocument
    {
        
        public Writable Comment { get; set; }
        public Writable Version { get; set; }
        
        
    }
}
