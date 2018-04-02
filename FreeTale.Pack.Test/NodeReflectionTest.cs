using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FreeTale.Pack.Test
{
    public class Common : Object
    {
        public int A;
        public float B;
    }

    [TestClass]
    public class NodeReflectionTest
    {
        public Common commonClass;

        [TestMethod]
        public void SaveAndReload()
        {
            commonClass = new Common()
            {
                A = 30,
                B = 10.5f
            };
            NodeReflection reflection = new NodeReflection();
            
            Node node = reflection.GetReflectNode(commonClass);
            Assert.AreEqual((int)node["A"].Value.Value, 30);
            Assert.AreEqual((float)node["B"].Value.Value, 10.5f);

            Common result = reflection.CreateObject<Common>(node);
            Assert.AreEqual(result.A, commonClass.A);
            Assert.AreEqual(result.B, commonClass.B);

        }
    }
}
