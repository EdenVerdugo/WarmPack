using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Classes
{
    public class Castable
    {
        public Castable(object obj)
        {
            _value = obj;
        }

        object _value { get; set; }

        public int ToInt32()
        {
            if (!int.TryParse(_value.ToString(), out int resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public uint ToUint32()
        {
            if (!uint.TryParse(_value.ToString(), out uint resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public decimal ToDecimal()
        {
            if (!decimal.TryParse(_value.ToString(), out decimal resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public float ToSingle()
        {
            if (!float.TryParse(_value.ToString(), out float resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public double ToDouble()
        {
            if (!double.TryParse(_value.ToString(), out double resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public long ToInt64()
        {
            if (!long.TryParse(_value.ToString(), out long resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public ulong ToUInt64()
        {
            if (!ulong.TryParse(_value.ToString(), out ulong resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public bool ToBoolean()
        {
            return Convert.ToBoolean(_value.ToString());
        }

        public byte ToByte()
        {
            if (!byte.TryParse(_value.ToString(), out byte resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }
        public byte[] ToBytes()
        {
            byte[] resultado = (byte[])_value;

            return resultado;
        }

        public DateTime ToDateTime()
        {
            if (!DateTime.TryParse(_value.ToString(), out DateTime resultado))
            {
                ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public override string ToString()
        {
            return _value != null ? _value.ToString() : "";
        }

        private Exception ThrownException(Type tipo)
        {
            return new Exception($"Unable to convert the value \"{_value}\" to the type \"{tipo.Name}\"");
        }
    }
}
