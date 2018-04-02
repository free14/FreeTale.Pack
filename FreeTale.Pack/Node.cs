using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// default class of INode
    /// </summary>
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
                if (SubNode == null)
                    return null;
                return SubNode.Find((node) => node.Name.IsString && node.Name.ToString() == index);
            }
            set
            {
                if(SubNode == null)
                {
                    SubNode = new List<INode>();
                    SubNode.Add(value);
                    return;
                }
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

        public List<INode> SubNode { get; set; }
        public List<IAttribute> Attribute { get; set; }
        public Writable Name { get; set; }
        public Writable Value { get; set; }
        public bool IsComment { get; set; }

        #region constructer

        public Node() { }
        public Node(Writable name,Writable value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region INode method

        public void Add(INode node)
        {
            if (SubNode == null)
                SubNode = new List<INode>();
            SubNode.Add(node);
        }

        public void Add(Writable name, Writable value)
        {
            Add(new Node(name, value));
        }

        public void Add(Writable[] name, Writable value)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentNullException("name");
            Writable[] tree = new Writable[name.Length - 1];
            Array.Copy(name, tree, tree.Length);
            INode sub = SubNode.Find((node) => node.Name == name[0]);
            if(name.Length > 1)
            {
                if(sub != null)
                {
                    sub.Add(tree, value);
                }
                else
                {
                    Node node = new Node(name[0], null);
                    Add(node);
                    node.Add(tree, value);
                }
            }
            else
            {
                if (sub != null)
                    sub.Value = value;
                else
                    Add(name[0], value);
            }
        }

        public void Merge(INode other)
        {
            if (other.SubNode == null)
                return;
            if (this.SubNode == null)
            {
                SubNode = other.SubNode;
                return;
            }
            foreach (INode item in other.SubNode)
            {
                INode sub = this.SubNode.Find((node) => node.Name == item.Name);
                if(sub != null)
                {
                    // found current node with same name
                    if (item.Value != null)
                        sub.Value = item.Value;

                    if (item.SubNode != null)
                        sub.Merge(item);
                    else
                        sub = item;
                }
                else
                {
                    SubNode.Add(item);
                }
            }
        }

        #endregion

        

    }
}
