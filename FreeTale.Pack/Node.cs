using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public class Node : INode
    {
        #region indexer
        
        public INode this[int index] {
            get => SubNode[index];
            set => SubNode[index] = value;
        }

        
        public INode this[string index]
        {
            get
            {
                foreach (INode item in SubNode)
                {
                    if (item.Name.IsString && (string)item.Name.Value == index)
                        return item;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                for (int i = 0; i < SubNode.Count; i++)
                {
                    if(SubNode[i].Name.IsString && (string)SubNode[i].Name.Value == index)
                    {
                        SubNode[i] = value;
                        return;
                    }
                }
                SubNode.Add(value);
            }
        }

        #endregion

        public List<INode> SubNode { get; set; }// = new List<INode>();
        public List<IAttribute> Attribute { get; set; }
        public Writable Name { get; set; }
        public Writable Value { get; set; }
    }
}
