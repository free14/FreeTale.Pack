using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Xml
{
    /// <include file='XmlDocument.xml' path='docs/members[@name="Xml"]/Xml/*'/>
    public class XmlUnpacker : Unpacker
    {
        
        public Document Parse()
        {
            Document document = new Document();
            if (Read() != '<')
                throw new FormatException();
            if (Peek() == '?')
            {
                ReadUntil('>'); //skip xml header
            }
            SkipWhiteSpace();
            document.SubNode = ReadAnySubNode();
            return document;
        }

        /// <summary>
        /// read node . "&lt;name&gt;" position at n
        
        /// </summary>
        /// <returns></returns>
        protected Node ReadNode()
        {
            Node node = new Node();
            //start with node name
            node.Name = new Writable(ReadString());
            SkipWhiteSpace();
            char c = Peek();
            if (c == '/')
                ReadString(2);
            if (c == '>')
                Read();
            //skip empty node
            else
            {
                SkipWhiteSpace();
                char sub = Peek();
                if (sub == '<')
                {
                    node.SubNode = ReadAnySubNode();
                }
                else
                {
                    //node contains value
                    string value = ReadUntil('<');
                    node.Value = new Writable(value);
                }
            }
            return node;
        }

        /// <summary>
        /// read any sub node. "&lt;name&rt" position at &lt;
        /// </summary>
        /// <param name="node"></param>
        public List<INode> ReadAnySubNode()
        {
            List<INode> node = new List<INode>();
            
            while (Peek() == '<')
            {
                char sub = ReadNext();
                if (sub == '/')
                {
                    Read();
                    ReadEndNode();
                    return node;
                }
                else if (sub == '!')
                {
                    ReadString("-");
                    node.Add(ReadComment());
                }
                else
                {
                    node.Add(ReadNode());
                }
                SkipWhiteSpace();
            }
            throw new FormatException("section");
        }

        /// <summary>
        /// read comment section
        /// </summary>
        /// <returns>node flag as comment</returns>
        public Node ReadComment()
        {
            string comment = ReadUntil('-');
            while (true)
            {
                string extend = ReadString(3);
                if (extend == "-->")
                    break;
                comment += extend;
            }
            Node node = new Node();
            node.Value = new Writable(comment);
            node.IsComment = true;
            return node;
        }

        /// <summary>
        /// end and node, "&lt;/name&gt;" position is n
        /// </summary>
        /// <returns>name of node to end</returns>
        public string ReadEndNode()
        {
            string name = ReadString();
            if (Read() != '>')
                throw new FormatException("end node");
            return name;
        }

        /// <summary>
        /// read name=value attrbute
        /// </summary>
        /// <returns></returns>
        protected Attribute ReadAttribute()
        {
            Writable name, value;
            name = new Writable(ReadString());
            Read(); // = char
            value = new Writable(ReadQuoteString());
            return new Attribute(name, value);
        }

        /// <summary>
        /// read xml value. position at 1st char of value
        /// </summary>
        /// <returns></returns>
        protected Writable ReadValue()
        {
            string value = ReadUntil('<');
            value = value.Replace('\n', ' ');
            value = value.Replace("  ", " "); // remove double writespace
            return new Writable(value);
        }
    }
}
