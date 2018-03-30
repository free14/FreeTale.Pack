using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// human readable text reader
    /// </summary>
    public class Unpacker
    {
        /// <summary>
        /// 
        /// </summary>
        public string input;

        /// <summary>
        /// position of string
        /// </summary>
        public int Position { get; protected set; }
        /// <summary>
        /// line position. 1 base index
        /// </summary>
        public int LineCount { get; protected set; } = 1;
        /// <summary>
        /// current line position. 1 base index
        /// </summary>
        public int CollumCount { get; protected set; } = 1;

        /// <summary>
        /// current indent. value show how many whihtspace char of current line
        /// </summary>
        public int Indent { get; protected set; }

        /// <summary>
        /// indent of line has successful read?
        /// </summary>
        protected bool ReadIndent;
        /// <summary>
        /// prepare input and randy to process 
        /// </summary>
        /// <param name="input">input string</param>
        public void Prepare(string input)
        {
            this.input = input;
            Reset();
            
        }
        /// <summary>
        /// reset position to beginning of input
        /// </summary>
        public void Reset()
        {
            Position = 0;
            LineCount = 1;
            CollumCount = 1;
            Indent = 0;
        }

        /// <summary>
        /// read current char and move cursor position.
        /// if CRLF detect return only CR('\r') and skip LF('\n')
        /// </summary>
        /// <returns>char at current position</returns>
        public char Read()
        {
            char c = input[Position];
            if(c == '\r' || c == '\n')
            {
                if(c == '\r' && input[Position + 1] == '\n')
                {
                    //detect as 1 newline
                    Position++;
                }
                //newline
                LineCount++;
                CollumCount = 1;
                Indent = 0;
                ReadIndent = false;
            }
            else
            {
                if (!ReadIndent && char.IsWhiteSpace(c))
                    Indent++;
                else
                    ReadIndent = true;
                CollumCount++;
            }
            Position++;
            return c;
        }

        /// <summary>
        /// peek 1 character. not move cursur position
        /// </summary>
        /// <returns>char at current position</returns>
        public char Peek()
        {
            return input[Position];
        }

        /// <summary>
        /// read input at position + 1 and move cursor position
        /// </summary>
        /// <example>
        /// current stage is "012" cursor point at 0
        /// method return 1 and set cursor to 1
        /// </example>
        /// <returns>next character</returns>
        public char ReadNext()
        {
            Read();
            return input[Position];
        }

        /// <summary>
        /// read string until char is not "a-zA-Z0-9_" and move cursor position
        /// </summary>
        /// <returns>string result</returns>
        public string ReadString()
        {
            int beginPosition = Position;
            char c = ReadNext();
            while (c == '_' || char.IsLetterOrDigit(c))
            {
                c = ReadNext();
            }
            return input.Substring(beginPosition, Position - beginPosition);
        }

        /// <summary>
        /// read string by spacify allow character
        /// </summary>
        /// <param name="allowChar">allow character</param>
        /// <returns>string</returns>
        public string ReadString(string allowChar)
        {
            int beginPosition = Position;
            char c = ReadNext();
            while (allowChar.IndexOf(c) != -1)
            {
                c = ReadNext();
            }
            return input.Substring(beginPosition, Position - beginPosition);
        }


        /// <summary>
        /// read string inside quote with defualt string format (json string)
        /// </summary>
        /// <returns></returns>
        public string ReadQuoteString()
        {
            char c = Read();
            if (c != '"')
                throw new FormatException("Quote string must begin with \" ");
            bool escape = false;
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                c = Read();
                if (!escape && c == '"')
                    break;
                if (!escape && c == '\\')
                {
                    escape = true;
                }else if (escape)
                {
                    switch (c)
                    {
                        case '\\':
                            sb.Append(@"\");
                            break;
                        case '/':
                            sb.Append(@"/");
                            break;
                        case 'b':
                            sb.Append("\b");
                            break;
                        default:
                            break;
                    }
                }
                
            }
            return sb.ToString();
        }

        /// <summary>
        /// read and ignore any whitespace
        /// </summary>
        public void SkipWhiteSpace()
        {
            if (!char.IsWhiteSpace(Peek()))
                return;
            while (char.IsWhiteSpace(ReadNext())){}
        }
    }
}
