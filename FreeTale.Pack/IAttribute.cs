using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public interface IAttribute
    {
        /// <summary>
        /// attribute name
        /// </summary>
        Writable Name { get; set; }
        /// <summary>
        /// attribute value
        /// </summary>
        Writable Value { get; set; }
    }
}
