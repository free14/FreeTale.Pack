using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
    public static class JsonUnpackerExtension
    {
        public static INode JsonDocument(this Unpacker unpacker)
        {
            if (unpacker.Peek() != '{')
                throw new FormatException("json document must start with '{'");
            else
            {
                INode json = new Node()
                {
                    SubNode = unpacker.ReadObject()
                };
                return json;
            }
        }
        /// <summary>
        /// read json object.
        /// </summary>
        /// <param name="unpacker"></param>
        /// <returns>array of name value object</returns>
        private static List<INode> ReadObject(this Unpacker unpacker)
        {
            char c = unpacker.Read();// skip {
            List<INode> nodes = new List<INode>();
            while (c != '}')
            {
                unpacker.SkipWhiteSpace();
                INode node = unpacker.ReadNameValue();
                nodes.Add(node);
                unpacker.SkipWhiteSpace();
                c = unpacker.Read(); // only , or }
            }
            return nodes;
        }

        private static INode ReadNameValue(this Unpacker unpacker)
        {
            string name = unpacker.ReadQuoteString();
            Node node = new Node();
            node.Name = name;
            unpacker.SkipWhiteSpace();
            if (unpacker.Read() != ':')
                throw new FormatException("invalid json name:value format");
            unpacker.SkipWhiteSpace();
            // json value
            char c = unpacker.Peek();
            if (c == '{')
                node.SubNode = unpacker.ReadObject();
            else if (c == '[')
                node.SubNode = unpacker.ReadArray();
            else
                node.Value = unpacker.ReadWritable(); //fully compartale with json
            return node;
        }

        private static List<INode> ReadArray(this Unpacker unpacker)
        {
            //array contains object or writable only
            List<INode> nodes = new List<INode>();
            
            char c = unpacker.Read(); // [
            while (c != ']')
            {
                unpacker.SkipWhiteSpace();
                // node contain value or subnode only
                INode node = new Node();
                if(c == '{') // object
                {
                    node.SubNode = unpacker.ReadObject();
                }
                else 
                {
                    node.Value = unpacker.ReadWritable();
                }
                nodes.Add(node);
                unpacker.SkipWhiteSpace();
                c = unpacker.Read(); // , or ]
            }
            return nodes;
        }
        
    }
}
