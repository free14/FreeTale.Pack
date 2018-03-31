using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FreeTale.Pack.Json;

namespace FreeTale.Pack.Test
{
    [TestClass]
    public class JsonPackerTest
    {
        string Json;

        INode node;

        [TestInitialize]
        public void Prepare()
        {
            Json = "{\"a\":10,\"c\":{\"b\":-0.5}}";
            Unpacker unpacker = new Unpacker();
            unpacker.Prepare(Json);
            node = unpacker.JsonDocument();
            INode n = new Node();
            n.Value = "Hello";
            node.JsonPack();

        }
        [TestMethod]
        public void Pack()
        {
            string repack = node.JsonPack(true);
            Assert.AreEqual(repack,Json);
        }
    }
}
