using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack.Xml
{

    /// <summary>
    /// extension for xml document
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// pack document to xml format
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string XmlDocument(this IDocument document)
        {
            XmlPacker packer = new XmlPacker();
            packer.Parse(document);
            return packer.ToString();
        }

        /// <summary>
        /// pack node to xml format
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string XmlDocument(this INode node)
        {
            XmlPacker packer = new XmlPacker();
            packer.Parse(node);
            return packer.ToString();
        }

        /// <summary>
        /// read xml doucument
        /// </summary>
        /// <param name="unpacker"></param>
        /// <returns>Document</returns>
        public static Document XmlDocument(this Unpacker unpacker)
        {
            XmlUnpacker xmlUnpacker = new XmlUnpacker();
            unpacker.CopyTo(xmlUnpacker);
            return xmlUnpacker.Parse();
        }

    }
}
