using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
    /// <summary>
    /// json packer class
    /// </summary>
    /// <remarks>
    /// Json feature
    /// <para>Node : pack as JSON object or "name":"value"</para>
    /// <para>Attribute : not include</para>
    /// <para>Writeable : quote string integer float</para>
    /// <para>Document : not include</para>
    /// <para>Comment : not include</para>
    /// <para>Array : create from Node without name</para>
    /// </remarks>
    public class JsonPacker : Packer
    {
        public INode Node;

        public void Parse(INode node)
        {
            WriteObject(node);
        }

        public void WriteObject(INode node)
        {
            WriteLine("{");
            Indent++;
            bool firstvlaue = true;
            for (int i = 0; i < node.SubNode.Count; i++)
            {
                if (!node[i].IsComment)
                {
                    if(!firstvlaue)
                    {
                        //first value
                        Write(",");
                        WriteSpace();

                    }
                    WriteNameValue(node[i]);
                    firstvlaue = false;
                }
            }
            Indent--;
            Write("}");
        }

        public void WriteNameValue(INode node)
        {
            Write(node.Name.ToQuoteString());
            WriteSpace();
            Write(":");
            WriteSpace();
            WriteValue(node);
        }

        public void WriteArray(INode[] array)
        {
            WriteLine("[");
            Indent++;
            bool firstvalue = true;
            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].IsComment)
                {
                    if (!firstvalue)
                    {
                        Write(",");
                        WriteSpace();
                    }
                    WriteValue(array[i]);
                }
            }
            Indent--;
            WriteLine("]");

        }

        public void WriteValue(INode node)
        {
            if (node.Value != null)
            {
                //is has value
                WriteLine(node.Value.ToQuoteString());
            }
            else if (node.SubNode != null && node[0].Name != null)
            {
                //is object
                WriteObject(node);
            }
            else
            {
                //is array
                WriteArray(node.SubNode.ToArray());
            }
        }
        
    }
}
