using System;
using System.Collections.Generic;
using System.Text;

namespace FreeTale.Pack
{
    /// <summary>
    /// writeable value. string/int/float/array
    /// </summary>
    public class Writable 
    {

        public static Writable Null { get { return new Writable(); } }

        /// <summary>
        /// value object
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// value is string
        /// </summary>
        public bool IsString { get => Value is string; }
        /// <summary>
        /// value is integer format.(int/long)
        /// </summary>
        public bool IsInt { get => Value is int || Value is long; }
        /// <summary>
        /// value is boolean
        /// </summary>
        public bool IsBool { get => Value is bool; }
        /// <summary>
        /// value is float
        /// </summary>
        public bool IsFloat { get => Value is float || Value is double || Value is decimal; }

        /// <summary>
        /// value is null
        /// </summary>
        public bool IsNull { get => Value == null; }

        /// <summary>
        /// Writeable is flag as comment
        /// </summary>
        public bool IsComment { get; set; }

        public DataType DataType {
            get
            {
                if (IsNull)
                    return DataType.Null;
                if (IsFloat)
                    return DataType.Float;
                if (IsInt)
                    return DataType.Int;
                if (IsBool)
                    return DataType.Bool;
                if (IsString)
                    return DataType.String;
                return DataType.Unknow;
            }
        }

        public override string ToString()
        {
            if (IsNull)
                return "null";
            if (IsString)
                return (string)Value;
            return Value.ToString();
        }

        /// <summary>
        /// if <see cref="Value"/> type is string will replace spcial character and insert quote
        /// </summary>
        /// <returns>string with quote. or string if <see cref="Value"/> is not string</returns>
        public string ToQuoteString()
        {
            if (IsString)
            {
                string value = (string)this.Value;
                StringBuilder sb = new StringBuilder(value.Length + 2);
                sb.Append('"');
                for (int i = 0; i < value.Length; i++)
                {
                    switch (value[i])
                    {
                        case '\n':
                            sb.Append(@"\n");
                            break;
                        case '\r':
                            sb.Append(@"\r");
                            break;
                        case '\b':
                            sb.Append(@"\b");
                            break;
                        case '\t':
                            sb.Append(@"\t");
                            break;
                        case '\f':
                            sb.Append(@"\f");
                            break;
                        case '\\':
                            sb.Append(@"\\");
                            break;
                        case '/':
                            sb.Append(@"\/");
                            break;
                        case '"':
                            sb.Append("\\\"");
                            break;
                        default:
                            sb.Append(value[i]);
                            break;
                    }
                }
                sb.Append('"');
                return sb.ToString();
            }
            return ToString();
        }

        public static implicit operator Writable(string value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }

        public static implicit operator Writable(int value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }

        public static implicit operator Writable(long value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }

        public static implicit operator Writable(float value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }

        public static implicit operator Writable(double value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }

        public static implicit operator Writable(bool value)
        {
            Writable ins = new Writable()
            {
                Value = value
            };
            return ins;
        }
    }
}
