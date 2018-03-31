using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
    public static class JsonPackerExtension
    {
        /// <summary>
        /// pack this node into json format.
        /// 
        /// </summary>
        /// <remarks>
        /// Json feature
        /// <para>Node : pack as JSON object or "name":"value"</para>
        /// <para>Attribute : not include</para>
        /// <para>Writeable : quote string integer float</para>
        /// <para>Document : not include</para>
        /// <para>Array : create from Node without name</para>
        /// </remarks>
        /// <param name="node"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static string JsonPack(this INode node,bool min)
        {
            JsonPacker packer = new JsonPacker();
            packer.Reset();
            if (min)
                packer.IgnoreWhitespace = true;
            packer.Parse(node);
            return packer.ToString();

        }
    }
}
