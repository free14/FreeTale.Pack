using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
    /// <summary>
    /// json packer class
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
    /// <remarks>
    /// Root <see cref="INode"/> not include name in this format
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
                if(node.SubNode != null)
                    WriteArray(node.SubNode.ToArray());
            }
        }
        
    }
}
