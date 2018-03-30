using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FreeTale.Pack.Json;

namespace FreeTale.Pack.Test
{
    /// <summary>
    /// Summary description for JsonUnpackerTest
    /// </summary>
    [TestClass]
    public class JsonUnpackerTest
    {
        public JsonUnpackerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        Unpacker unpacker;

        [TestInitialize]
        public void Init()
        {
            unpacker = new Unpacker();
            unpacker.Prepare("{\"a\":10,\"c\":{\"b\":-0.5}}");
        }

        [TestMethod]
        public void JsonUnpack()
        {
            INode node = unpacker.JsonDocument();
            Assert.AreEqual(node.SubNode[0].Name.ToString(), "a");
            Assert.AreEqual((int)node["a"].Value.Value, 10);
            Assert.AreEqual(node["c"]["b"].Value.DataType, DataType.Float);
            Assert.AreEqual((double)node["c"]["b"].Value.Value, -0.5d);
        }
    }
}
