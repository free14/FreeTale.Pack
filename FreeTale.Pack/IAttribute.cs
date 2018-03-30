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
        Writeable Name { get; set; }
        /// <summary>
        /// attribute value
        /// </summary>
        Writeable Value { get; set; }
    }
}
