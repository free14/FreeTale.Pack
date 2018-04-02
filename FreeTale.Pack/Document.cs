using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    public class Document : IDocument
    {
        public INode this[int index]
        {
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
                    if (SubNode[i].Name.IsString && (string)SubNode[i].Name.Value == index)
                    {
                        SubNode[i] = value;
                        return;
                    }
                }
                SubNode.Add(value);
            }
        }

        public Writable Comment { get; set; }
        public Writable Version { get; set; }
        public List<INode> SubNode { get; set; }
        public List<IAttribute> Attribute { get; set; }
        public Writable Name { get; set; }
        public Writable Value { get; set; }
        public bool IsComment { get; set; }

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
            if (name.Length > 1)
            {
                if (sub != null)
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
    }
}
