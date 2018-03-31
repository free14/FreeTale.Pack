using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{

    public static class JsonExtension
    {
        /// <summary>
        /// pack this node into json format. for setting see <seealso cref="JsonPacker"/>
        /// <para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="INode"/></term>
        /// <description>pack as JSON object or "name":"value"</description>
        /// </item>
        /// <item>
        /// <term><see cref="IAttribute"/></term>
        /// <description>ignore</description>
        /// </item>
        /// <item>
        /// <term><see cref="Writable"/></term>
        /// <description>quote string integer float</description>
        /// </item>
        /// <item>
        /// <term><see cref="IDocument"/></term>
        /// <desicription>ignore</desicription></item>
        /// <item>
        /// <term><see cref="INode.IsComment"/></term>
        /// <description>Node not include</description>
        /// </item>
        /// <item>
        /// <term>Array</term>
        /// <description>Node without name will see as array element</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="min">remove white space</param>
        /// <remarks>
        /// Root <see cref="INode"/> not include name in this format
        /// </remarks>
        public static string JsonPack(this INode node, bool min = false)
        {
            JsonPacker packer = new JsonPacker();
            packer.Reset();
            if (min)
                packer.IgnoreWhitespace = true;
            packer.Parse(node);
            return packer.ToString();

        }

        /// <summary>
        /// Unpack json document.
        /// </summary>
        /// <remarks>
        /// Json support 110% some value not suppport in json format may not generate error and output perfactly
        /// <para> "name" : "value" output as INode with Name value</para>
        /// <para> </para>
        /// </remarks>
        /// <param name="unpacker"></param>
        /// <returns><see cref="Node"/> of json</returns>
        /// <exception cref="FormatException">document invalid format <seealso cref="Unpacker.LineCount"/> to find where error locate</exception>
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
