using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// link node for dynamic link format
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// where link from
        /// </summary>
        INode From { get; set; }
        /// <summary>
        /// node link to
        /// </summary>
        INode To { get; set; }
    }
}
