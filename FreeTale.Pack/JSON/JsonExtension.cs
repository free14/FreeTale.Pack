using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.JSON
{
    public static class JsonExtension
    {
        public static INode JsonDocument(this Unpacker unpacker)
        {
            if (unpacker.Read() != '{')
                throw new FormatException("json document must start with '{'");
            else
                return unpacker.ReadObject();
        }

        private static INode ReadObject(this Unpacker unpacker)
        {
            unpacker.SkipWhiteSpace();
            string name = unpacker.ReadQuoteString();
            Node node = new Node();
            node.Name = name;
            
        }

        
        
    }
}
