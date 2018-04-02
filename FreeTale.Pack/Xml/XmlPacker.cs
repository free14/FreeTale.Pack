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
        /// parse document to xml
        /// </summary>
        /// <param name="document"></param>
        public void Parse(IDocument document)
        {
            WriteLine(string.Format("<?xml version={0} encoding={1}?>", document.Version.ToQuoteString(), '"' + Encoding + '"'));
            WriteNode(document);
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
            Write("<");
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
                Write("<");
                Write(node.Name);
                Write(">");
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
            Write(value);
        }
    }
}
