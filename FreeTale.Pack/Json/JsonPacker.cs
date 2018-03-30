using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Json
{
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
            for (int i = 0; i < node.SubNode.Count; i++)
            {
                WriteNameValue(node[i]);
                if (i + 1 != node.SubNode.Count)
                {
                    //not end of array
                    Write(",");
                    WriteSpace();
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
            for (int i = 0; i < array.Length; i++)
            {
                WriteValue(array[i]);
                if(i + 1 != array.Length)
                {
                    //not end of array
                    Write(",");
                    WriteSpace();
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
