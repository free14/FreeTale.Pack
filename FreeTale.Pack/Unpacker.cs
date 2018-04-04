using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// human readable text reader
    /// </summary>
    public class Unpacker
    {
        #region field
        /// <summary>
        /// input string
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
        /// current position is out of input
        /// </summary>
        public bool IsEnd => Position >= input.Length;

        /// <summary>
        /// current position is last char of input
        /// </summary>
        public bool IsLast => Position + 2 == input.Length;

        /// <summary>
        /// begin escape stage when escape char trigger
        /// </summary>
        public char EscapeChar = '\\';

        /// <summary>
        /// escape char config
        /// </summary>
        public Dictionary<char, string> EscapeList;

        /// <summary>
        /// char following escape char detect as unicode escape
        /// </summary>
        public char UnicodeEscape = 'u';

        /// <summary>
        /// indent of line has successful read?
        /// </summary>
        protected bool ReadIndent;



        #endregion

        public Unpacker()
        {
            Reset();
            ResetEscape();
        }

        public Unpacker(string input)
        {
            Reset();
            this.input = input;
            ResetEscape();
        }

        

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
        /// reset <see cref="EscapeChar"/> <see cref="EscapeList"/> <see cref="UnicodeEscape"/> to defualt
        /// </summary>
        public virtual void ResetEscape()
        {
            EscapeChar = '\\';
            EscapeList = new Dictionary<char, string>
            {
                { '"', "\"" },
                { '\\', "\\" },
                { '/', "/" },
                { 'b', "\b" },
                {'f', "\f"},
                {'n', "\n" },
                {'r', "\r" },
                { 't', "\t" },
            };
            UnicodeEscape = 'u';
        }

        /// <summary>
        /// copy current unpacker to other instance
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(Unpacker target)
        {
            target.input = input;
            target.Position = Position;
            target.LineCount = LineCount;
            target.CollumCount = CollumCount;
            target.Indent = Indent;
        }

        #region read method

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
        /// read from current position to new line or end of input
        /// </summary>
        /// <returns>string exclude new line char</returns>
        public virtual string ReadLine()
        {
            string result = ReadUntil('\n');
            if(!IsLast)
                Read(); // skip \n
            if (result[result.Length - 1] == '\r')
            {
                //exclude for \r\n
                result = result.Substring(0, result.Length - 1);
            }
            return result;
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
            char c = Peek();
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
        /// <returns>string allow</returns>
        public string ReadString(string allowChar)
        {
            int beginPosition = Position;
            char c = Peek();
            while (allowChar.IndexOf(c) != -1)
            {
                c = ReadNext();
            }
            return input.Substring(beginPosition, Position - beginPosition);
        }


        /// <summary>
        /// read string inside sigle or double quote with defualt string format (json string)
        /// </summary>
        /// <returns>string with out quote</returns>
        public virtual string ReadQuoteString()
        {
            char c = Read();
            char quote;
            if (c != '"' && c != '\'') 
                throw new FormatException("Quote string must begin with \" ");
            quote = c;
            bool escape = false;
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                c = Read();
                if (!escape && c == quote)
                    break;
                if (!escape && c == EscapeChar)
                {
                    escape = true;
                }
                else if (escape)
                {
                    sb.Append(ReadEscape());
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
        /// read writable value. Detect quoteString integer floating-point true/false null
        /// not detect bool if set as Yes/n 
        /// </summary>
        /// <returns>writeable contains value</returns>
        public virtual Writable ReadWritable()
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
        public virtual bool ReadBool()
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
        public virtual Writable ReadNumber()
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
                    throw new FormatException(string.Format("invalid number format '{0}' .", c));
                }

                minusable = false;
                c = char.ToLower(ReadNext());
            }
            string num = sb.ToString();
            if(flag == ' ' && dotable && eable)
            {
                if (int.TryParse(num, out int result))
                    return result;
                return long.Parse(num);
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
                return new Writable(decimal.Parse(num));
            }
            throw new FormatException(string.Format("invlid flag '{0}'",flag));
        }

        /// <summary>
        /// read null value
        /// </summary>
        /// <returns>true string is "null"</returns>
        /// <exception cref="FormatException">current text is not null</exception>
        public virtual bool ReadNull()
        {
            string current = ReadString();
            current = current.ToLower();
            if (current == "null")
                return true;
            throw new FormatException("null format must be null.");
        }

        /// <summary>
        /// read string and stop at breakchar or end of input
        /// </summary>
        /// <param name="breakchar"></param>
        /// <returns></returns>
        /// <example>
        /// "012345678" position at 0 ReadUntil('6') return 012345 and set position at 6
        /// </example>
        public virtual string ReadUntil(char breakchar)
        {
            int beginPosition = Position;
            char c = Peek();
            while (c != breakchar && !IsLast)
            {
                c = ReadNext();
            }
            return input.Substring(beginPosition, Position - beginPosition);
        }

        /// <summary>
        /// call detect escape char
        /// </summary>
        /// <returns>string in <see cref="EscapeList"/></returns>
        /// <exception cref="FormatException">escape char not found</exception>
        public virtual string ReadEscape()
        {
            char c = Read();
            if (c == UnicodeEscape)
            {
                string hex = ReadString(4);
                int code = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                return ((char)code).ToString();
            }
            else if(EscapeList.ContainsKey(c))
            {
                return EscapeList[c];
            }
            throw new FormatException("escape character");
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

        #endregion


    }
}
