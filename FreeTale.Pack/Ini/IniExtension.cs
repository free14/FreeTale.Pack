using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Ini
{
    public static class IniExtension
    {
        /// <summary>
        /// ini packer
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// INode layer 1 if not has value output as [<see cref="INode.Name"/>].
        /// layer 2 or 1 with value output as <see cref="INode.Value"/>=<see cref="INode.Value"/>
        /// </description>
        /// </item>
        /// <item>
        /// <description><see cref="IAttribute"/> : ignore</description>
        /// </item>
        /// <item>
        /// <description><see cref="Writable"/> : normal string</description>
        /// </item>
        /// <item>
        /// <description><see cref="INode.IsComment"/> : ;<see cref="INode.Value"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <see cref="INode.Name"/> with = include will generate error.
        /// format is new line sensitive
        /// </remarks>
        public static string IniPack(this INode node)
        {
            StringBuilder sb = new StringBuilder();
            foreach (INode item in node.SubNode)
            {
                if(item.Value == null)
                {
                    sb.AppendLine("[" + item.Name.ToString() + "]");
                    foreach (INode subSection in item.SubNode)
                    {
                        if (subSection.IsComment)
                            sb.AppendLine(";" + subSection.Value);
                        else
                            sb.AppendLine(subSection.Name.ToString() + "=" + subSection.Value.ToString());
                    }
                }
                else if (item.IsComment)
                {
                    sb.AppendLine(";" + item.Value);
                }
                else
                {
                    sb.AppendLine(item.Name.ToString() + "=" + item.Value.ToString());
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// unpack ini document
        /// </summary>
        /// <param name="unpacker"></param>
        /// <returns></returns>
        public static IDocument IniDocument(this Unpacker unpacker)
        {
            IDocument document = new Document();
            List<INode> nodeTag = new List<INode>();
            Node currentNode = null;
            List<INode> currentSection = new List<INode>();
            while (!unpacker.IsEnd && !unpacker.IsLast)
            {
                string line = unpacker.ReadLine();
                if(line.Length >= 2)
                {
                    if (line[0] == '[' && line[line.Length - 1] == ']')
                    {
                        line = line.Substring(1, line.Length - 2);
                        
                        if (currentNode != null)
                        {
                            nodeTag.Add(currentNode);
                        }
                        else
                        {
                            //no section define
                            nodeTag = currentSection;
                            currentSection = new List<INode>();
                        }
                        if (currentSection.Count != 0)
                        {
                            //clean up this section
                            currentNode.SubNode = currentSection;
                            currentSection = new List<INode>();
                        }
                        currentNode = new Node();
                        currentNode.Name = line;
                    }
                    else if(line[0] == ';')
                    {
                        Node node = new Node
                        {
                            IsComment = true,
                            Value = line.Substring(1)
                        };
                        currentSection.Add(node);
                    }
                    else if (line.Contains("="))
                    {
                        int index = line.IndexOf('=');
                        Node node = new Node()
                        {
                            Name = line.Substring(0, index),
                            Value = line.Substring(index + 1)
                        };
                        currentSection.Add(node);
                    }
                    //ignore line not match format
                }
            }//end while

            if(currentNode != null)
            {
                currentNode.SubNode = currentSection;
                nodeTag.Add(currentNode);
            }
            else
            {
                nodeTag = currentSection;
            }
            document.SubNode = nodeTag;
            return document;
        }
    }
}
