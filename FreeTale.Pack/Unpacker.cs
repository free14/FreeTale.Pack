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
        /// read by spacify length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReadString(int length)
        {
            int begin = Position;
            for (int i = 0; i < length; i++)
                Read();
            return input.Substring(begin, Position - begin);
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
                }
                else if (escape)
                {
                    switch (c)
                    {
                        case '"':
                            sb.Append('"');
                            break;
                        case '\\':
                            sb.Append(@"\");
                            break;
                        case '/':
                            sb.Append(@"/");
                            break;
                        case 'b':
                            sb.Append("\b");
                            break;
                        case 'f':
                            sb.Append('\f');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case 'u':
                            int code = int.Parse(ReadString(4),System.Globalization.NumberStyles.HexNumber);
                            sb.Append((char)code);
                            break;
                        default:
                            throw new FormatException(string.Format("escape character '{0}' invalid", c));
                            break;
                    }
                    escape = false;
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// read writable value. Detect quote string integer floating-point true/false null
        /// not detect bool if set as Yes/n 
        /// </summary>
        /// <returns>writeable contains value</returns>
        public Writable ReadWritable()
        {
            char c = char.ToLower(Peek());
            if(c == 'n')
            {
                if (ReadNull())
                    return Writable.Null;
            }
            if (c == '"')
                return ReadQuoteString();
            if (c == 't' || c == 'f')
                return ReadBool();
            if (char.IsDigit(c) || c == '-')
                return ReadNumber();
            throw new FormatException(string.Format("unable to detect writable format for '{0}'",c));
        }

        /// <summary>
        /// read boolean data. case in-sentitive
        /// </summary>
        /// <returns>boolean contain</returns>
        /// <exception cref="FormatException">if current text is not 'true/fasle/yes/n'</exception>
        public bool ReadBool()
        {
            string current = ReadString();
            current = current.ToLower();
            switch (current)
            {
                case "true":
                case "yes":
                case "y":
                    return true;
                case "false":
                case "no":
                case "n":
                    return false;
                default:
                    throw new FormatException("boolean format must be true/false/yes/n.");
            }
        }

        /// <summary>
        /// read number as writable. number may be int or double or format if following format(ilfdm)
        /// </summary>
        /// <returns></returns>
        public Writable ReadNumber()
        {
            bool dotable = true; //found .
            bool eable = true; //found e
            bool minusable = true; //
            char c = char.ToLower(Peek());
            StringBuilder sb = new StringBuilder();
            char flag = ' '; //value flag as type of number?
            while (char.IsDigit(c) || ".e-ilfdm".IndexOf(c) != -1)
            {
                if(c == '-' && minusable)
                {
                    sb.Append('-');
                }
                else if (c == '.' && dotable)
                {
                    sb.Append('.');
                    dotable = false;
                }
                else if(c == 'e' && eable)
                {
                    sb.Append('e');
                    eable = false;
                    minusable = true;
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else if ("ilfdm".IndexOf(c) != -1)
                {
                    flag = c;
                    Read();
                    break;
                }
                else
                {
                    throw new FormatException(string.Format("invalid '{0}' char.", c));
                }

                minusable = false;
                c = char.ToLower(ReadNext());
            }
            string num = sb.ToString();
            if(flag == ' ' && dotable && eable)
            {
                return int.Parse(num);
            }
            if(flag == ' ')
            {
                return double.Parse(num);
            }
            if(flag == 'i')
                return int.Parse(num);
            if (flag == 'l')
                return long.Parse(num);
            if (flag == 'f')
                return float.Parse(num);
            if(flag == 'd')
                return double.Parse(num);
            if(flag == 'm')
            {
                Writable writable = new Writable();
                writable.Value = decimal.Parse(num);
                return writable;
            }
            throw new FormatException(string.Format("invlid flag '{0}'",flag));
        }

        /// <summary>
        /// read null value
        /// </summary>
        /// <returns>true string is "null"</returns>
        /// <exception cref="FormatException">current text is not null</exception>
        public bool ReadNull()
        {
            string current = ReadString();
            current = current.ToLower();
            if (current == "null")
                return true;
            throw new FormatException("null format must be null.");
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
