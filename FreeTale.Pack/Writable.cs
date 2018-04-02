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
        /// <summary>
        /// get Writeable with Null value
        /// </summary>
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
        public bool IsInt
        {
            get => Value is short || Value is int || Value is long ||
                Value is ushort || Value is uint || Value is ulong;
        } 
        /// <summary>
        /// value is boolean
        /// </summary>
        public bool IsBool { get => Value is bool; }
        /// <summary>
        /// value is float
        /// </summary>
        public bool IsFloat { get => Value is float || Value is double || Value is decimal; }

        /// <summary>
        /// value is enum
        /// </summary>
        public bool IsEnum { get => Value is Enum; }

        /// <summary>
        /// value is null
        /// </summary>
        public bool IsNull { get => Value == null; }

        /// <summary>
        /// get value type full name. include namespace
        /// </summary>
        /// <returns>fullname of value</returns>
        public string GetTypeName()
        {
            return Value.GetType().FullName;
        }


        /// <summary>
        /// Writeable is flag as comment
        /// </summary>
        public bool IsComment { get; set; }

        /// <summary>
        /// get <see cref="Pack.DataType"/> value from type
        /// </summary>
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

        /// <summary>
        /// create as <see cref="Null"/>
        /// </summary>
        public Writable() { }
        
        /// <summary>
        /// create with value
        /// </summary>
        /// <param name="value"></param>
        public Writable(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Convert value to string
        /// </summary>
        /// <returns>lowwer case string. except datatype is string.</returns>
        public override string ToString()
        {
            if (IsNull)
                return "null";
            if (IsString)
                return (string)Value;
            if (IsBool)
                return (bool)Value ? "true" : "false";
            return Value.ToString();
        }

        /// <summary>
        /// if <see cref="Value"/> type is string will replace spcial character. and insert quote
        /// </summary>
        /// <returns>string with quote.</returns>
        public string ToQuoteString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            if (IsString)
            {
                string value = (string)this.Value;
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
            }
            else
                Value.ToString();
            sb.Append('"');
            return sb.ToString();
            
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


        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (obj is Writable right)
            {
                if (this.IsString && right.IsString)
                    return (string)Value == (string)right.Value;
                return right.Value == this.Value;
            }
            return false;
        }

        public static bool operator ==(Writable left, Writable right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return true;
            else if (!object.ReferenceEquals(left, null))
                return left.Equals(right);
            return false;
        }

        public static bool operator !=(Writable left, Writable right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return false;
            return !left.Equals(right);
        }
    }
}
