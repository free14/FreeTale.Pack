using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// human readable text writer
    /// </summary>
    public class Packer
    {

        /// <summary>
        /// if ture. will print without whitespace generate from Packer
        /// </summary>
        public bool IgnoreWhitespace { get; set; } = false;

        /// <summary>
        /// string to fill with <see cref="WriteSpace"/>
        /// </summary>
        public string SpaceString { get; set; } = " ";

        /// <summary>
        /// Indent string to insert
        /// </summary>
        public string IndentString { get; set; } = "\t";

        /// <summary>
        /// current indent
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// current line has indent?
        /// </summary>
        protected bool HasIndent = false;

        protected StringBuilder builder = new StringBuilder();

        public void Reset()
        {
            builder = new StringBuilder();
            IndentString = "\t";
            Indent = 0;
            HasIndent = false;
        }

        public void Write(string value)
        {
            WriteIndent();
            builder.Append(value);
        }

        /// <summary>
        /// write by <see cref="Writable.ToString"/>. ignore if <paramref name="value"/> is null
        /// </summary>
        /// <param name="value"></param>
        public void Write(Writable value)
        {
            if(value != null)
            {
                WriteIndent();
                builder.Append(value.ToString());
            }
        }

        /// <summary>
        /// write value and move to next line
        /// </summary>
        /// <param name="value"></param>
        public void WriteLine(string value)
        {
            WriteIndent();
            builder.Append(value);
            if (!IgnoreWhitespace)
                builder.AppendLine();
            HasIndent = false;
        }

        /// <summary>
        /// move to next line. if format is new line sensitive use <see cref="ForceWriteLine"/>
        /// </summary>
        public void WriteLine()
        {
            if(!IgnoreWhitespace)
                builder.AppendLine();
            HasIndent = false;
        }

        /// <summary>
        /// move to nextline ignore <see cref="IgnoreWhitespace"/> setting. use for new line sensitive format
        /// </summary>
        public void ForceWriteLine()
        {
            builder.AppendLine();
        }

        /// <summary>
        /// write indent if current line not write
        /// </summary>
        public void WriteIndent()
        {
            if (!HasIndent && !IgnoreWhitespace)
            {
                for (int i = 0; i < Indent; i++)
                {
                    builder.Append(IndentString);
                }
            }
        }

        /// <summary>
        /// write whitespace
        /// </summary>
        public void WriteSpace()
        {
            if (!IgnoreWhitespace)
                builder.Append(" ");
        }

        public override string ToString()
        {
            return builder.ToString();
        }

        
    }
}
