using System;
using System.Diagnostics;

namespace FreeTale.Pack.TestConsole
{
    public class Common
    {
        public int A = 10;
        public int B = 20;
        public float C = 3.14f;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Common common = new Common()
            {
                B = 30
            };

            NodeReflection reflection = new NodeReflection();
            Node node = reflection.GetReflectNode(common);
            Debug.WriteLine(node.ToString());
            Common result = reflection.CreateObject<Common>(node);
            Debug.WriteLine("value {0} {1} {2}", result.A, result.B, result.C);
        }
    }
}
