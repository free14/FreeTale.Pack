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

        Document document;
        [TestInitialize]
        public void Init()
        {
            node = new Node();
            node.Add("name", "value");
            document = new Document();
            document.Version = "1.0";
            document.Add("name", "value");
        }

        [TestMethod]
        public void XmlUnpack()
        {
            unpacker = new XmlUnpacker();
            unpacker.Prepare(xml);
            Node current = unpacker.Parse();
            //Assert.AreEqual(current.ToString(), "");
            Assert.AreEqual(current[0].Name.ToString(), "name");
        }

        [TestMethod]
        public void XmlPack()
        {
            XmlPacker xmlPacker = new XmlPacker();
            xmlPacker.IgnoreWhitespace = true;
            xmlPacker.Encoding = null;
            xmlPacker.Parse(document);
            string result = xmlPacker.ToString();
            Assert.AreEqual(result,xml);
        }
    }
}
