using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
    public static class JsonPackerExtension
    {

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
