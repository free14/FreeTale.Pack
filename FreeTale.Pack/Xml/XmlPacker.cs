using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Xml
{
    public class XmlPacker : Packer
    {
        /// <summary>
        /// encoding display in xml file
        /// </summary>
        public string Encoding = "UTF-8";

        /// <summary>
        /// skip node with name = null? or output as <see cref="DummyName"/>
        /// </summary>
        public bool SkipNoName = false;

        /// <summary>
        /// name insert when <see cref="INode.Name"/> is null
        /// </summary>
        public string DummyName = "node";

        /// <summary>
        /// parse document to xml
        /// </summary>
        /// <param name="document"></param>
        public void Parse(IDocument document)
        {
            Write("<?xml");
            if (document.Version != null)
                Write(" version=" + document.Version.ToQuoteString());
            if (Encoding != null)
                Write(" encoding=\"" + Encoding + "\"");
            WriteLine("?>");
            foreach (var item in document.SubNode)
            {
                WriteNode(item);
            }
        }

        /// <summary>
        /// parse node to xml
        /// </summary>
        /// <param name="node"></param>
        public void Parse(INode node)
        {
            WriteNode(node);
        }

        /// <summary>
        /// write xml node
        /// </summary>
        /// <param name="node"></param>
        protected void WriteNode(INode node)
        {
            if (SkipNoName && node.Name == null)
                return;
            Write("<");
            if (node.Name == null)
                Write(DummyName);
            else
                Write(node.Name.ToString());
            if (node.Attribute != null)
                WriteAttribute(node.Attribute.ToArray());
            if(node.SubNode == null && node.Value == null)
            {
                //empty tag
                WriteLine("/>");
            }
            else
            {
                WriteLine(">");
                Indent++;
                if(node.SubNode != null)
                {
                    for (int i = 0; i < node.SubNode.Count; i++)
                    {
                        WriteNode(node.SubNode[i]);
                    }
                }
                else
                {
                    //value
                    WriteValue(node.Value);
                }
                Indent--;
                Write("</");
                if (node.Name == null)
                    Write(DummyName);
                else
                    Write(node.Name.ToString());
                WriteLine(">");
            }
        }

        /// <summary>
        /// write attrbute array
        /// </summary>
        /// <param name="attr"></param>
        protected void WriteAttribute(IAttribute[] attr)
        {
            for (int i = 0; i < attr.Length; i++)
            {
                Write(" ");
                Write(attr[i].Name);
                Write("=");
                Write(attr[i].Value.ToQuoteString());
            }
        }

        protected void WriteValue(Writable value)
        {
            WriteLine(value.ToString());
        }
    }
}
