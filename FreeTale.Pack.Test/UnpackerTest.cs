using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FreeTale.Pack.Test
{
    [TestClass]
    public class UnpackerTest
    {
        Unpacker unpacker;

        [TestMethod]
        public void Create()
        {
            unpacker = new Unpacker();
            unpacker.Prepare("0123456     Hello! \n World");
            
        }

        [TestMethod]
        public void Read()
        {
            Create();
            Assert.AreEqual(unpacker.Read(), '0');
            Assert.AreEqual(unpacker.Peek(), '1');
            Assert.AreEqual(unpacker.ReadNext(), '2');
            Assert.AreEqual(unpacker.ReadString(), "23456");
            Assert.AreEqual(unpacker.Read(), ' ');
            unpacker.SkipWhiteSpace();
            Assert.AreEqual(unpacker.Read(), 'H');
            Assert.AreEqual(unpacker.ReadString("elo!"), "ello!");
            unpacker.SkipWhiteSpace();
            Assert.AreEqual(unpacker.Read(),'W');
            Debug.WriteLine($"line :{unpacker.LineCount} col :{unpacker.CollumCount}");
            Assert.AreEqual(unpacker.CollumCount, 3);
            Assert.AreEqual(unpacker.LineCount, 2);
            Assert.AreEqual(unpacker.Indent, 1);
        }


    }
}
