using FreeTale.Pack.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTale.Pack.Test
{
    [TestClass]
    public class XmlTest
    {
        string xml = "<?xml version=\"1.0\"?><name>value</name>";
        Node node;

        XmlUnpacker unpacker;
        [TestInitialize]
        public void Init()
        {
            node = new Node();
            node.Add("name", "value");
            unpacker = new XmlUnpacker();
            unpacker.Prepare(xml);
        }

        [TestMethod]
        public void XmlUnpack()
        {
            Node current = unpacker.Parse();
            //Assert.AreEqual(current.ToString(), "");
            Assert.AreEqual(current.SubNode[0].Name.ToString(), "name");
        }

        [TestMethod]
        public void XmlPack()
        {
            XmlPacker xmlPacker = new XmlPacker();
            xmlPacker.IgnoreWhitespace = true;
            xmlPacker.Parse(node);
            string result = xmlPacker.ToString();
            Assert.AreEqual(result,xml);
        }
    }
}
