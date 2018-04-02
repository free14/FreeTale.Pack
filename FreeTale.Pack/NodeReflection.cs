using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace FreeTale.Pack
{
    /// <summary>
    /// create node tree from custom object
    /// </summary>
    public class NodeReflection
    {
        /// <summary>
        /// how field discover
        /// </summary>
        public BindingFlags FieldFlag = BindingFlags.Public | BindingFlags.Instance;
        
        /// <summary>
        /// get node by any custom object
        /// </summary>
        /// <param name="obj">object to create</param>
        /// <returns></returns>
        public Node GetReflectNode(object obj)
        {
            Node node = new Node();
            Type type = obj.GetType();
            if (type.IsPrimitive)
            {
                node.AddAttribute("FieldType", type.FullName);
                node.Value = new Writable(obj);
                return node;
            }
            FieldInfo[] fieldInfo = type.GetFields(FieldFlag);
            foreach (FieldInfo field in fieldInfo)
            {
                object fieldValue = field.GetValue(obj);
                Node sub = GetReflectNode(fieldValue);
                sub.Name = field.Name;
                node.Add(sub);
            }
            return node;
        }

        /// <summary>
        /// create object from packed node
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">node packed</param>
        /// <returns></returns>
        public T CreateObject<T>(INode node)
        {
            Type type = typeof(T);
            T instance = Activator.CreateInstance<T>();
            return BindObject<T>(node, instance);
        }

        /// <summary>
        /// bind value to existing object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="instance"></param>
        public T BindObject<T>(INode node,T instance)
        {
            Type type = typeof(T);
            FieldInfo[] fieldInfo = type.GetFields(FieldFlag);

            foreach (var field in fieldInfo)
            {
                INode sub = node[field.Name];
                Type fieldType = field.FieldType;
                if (sub.Value != null && fieldType.IsPrimitive)
                {
                    if (fieldType.IsAssignableFrom(sub.Value.Value.GetType()))
                    {
                        field.SetValue(instance, sub.Value.Value);
                    }
                }
                else if (sub.SubNode != null)
                {
                    object fieldValue = field.GetValue(instance);
                    object subResult = BindObject(sub, fieldValue);
                    field.SetValue(instance, subResult);
                }
            }
            return instance;
        }
    }
}
