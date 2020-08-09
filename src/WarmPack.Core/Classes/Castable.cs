using System;

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

        public int TryInt32(Func<Castable,int> onError)
        {
            if (_value == null) return onError(this);

            if (!int.TryParse(_value.ToString(), out int resultado))
            {
                resultado = onError(this);                
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

        public uint TryUint32(Func<Castable, uint> onError)
        {
            if (_value == null) return onError(this);

            if (!uint.TryParse(_value.ToString(), out uint resultado))
            {
                resultado = onError(this);
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

        public decimal TryDecimal(Func<Castable, decimal> onError)
        {
            if (_value == null) return onError(this);

            if (!decimal.TryParse(_value.ToString(), out decimal resultado))
            {
                resultado = onError(this);
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

        public float TrySingle(Func<Castable, float> onError)
        {
            if (_value == null) return onError(this);

            if (!float.TryParse(_value.ToString(), out float resultado))
            {
                resultado = onError(this);
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

        public double TryDouble(Func<Castable, double> onError)
        {
            if (_value == null) return onError(this);

            if (!double.TryParse(_value.ToString(), out double resultado))
            {
                resultado = onError(this);
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

        public long TryInt64(Func<Castable, long> onError)
        {
            if (_value == null) return onError(this);

            if (!long.TryParse(_value.ToString(), out long resultado))
            {
                resultado = onError(this);
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

        public ulong TryUInt64(Func<Castable, ulong> onError)
        {
            if (_value == null) return onError(this);

            if (!ulong.TryParse(_value.ToString(), out ulong resultado))
            {
                resultado = onError(this);
            }
            return resultado;
        }

        public bool ToBoolean()
        {
            return Convert.ToBoolean(_value.ToString());
        }

        public bool TryBoolean(Func<Castable, bool> onError)
        {
            if (_value == null) return onError(this);

            if (bool.TryParse(_value.ToString(), out bool resultado))
            {
                resultado = onError(this);
            }

            return resultado;
        }

        public byte ToByte()
        {
            if (!byte.TryParse(_value.ToString(), out byte resultado))
            {
                throw ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public byte TryByte(Func<Castable, byte> onError)
        {
            if (_value == null) return onError(this);

            if (!byte.TryParse(_value.ToString(), out byte resultado))
            {
                resultado = onError(this);
            }

            return resultado;
        }

        public byte[] ToBytes()
        {
            byte[] resultado = (byte[])_value;

            return resultado;
        }

        public byte[] TryBytes(Func<Castable, byte[]> onError)
        {
            if (_value == null) return onError(this);

            try
            {
                byte[] resultado = (byte[])_value;

                if(resultado == null)
                {
                    onError(this);
                }

                return resultado;
            }
            catch
            {
                return onError(this);                
            }   
        }

        public DateTime ToDateTime()
        {
            if (!DateTime.TryParse(_value.ToString(), out DateTime resultado))
            {
                ThrownException(resultado.GetType());
            }
            return resultado;
        }

        public DateTime TryDateTime(Func<Castable, DateTime> onError)
        {
            if (_value == null) return onError(this);

            if (!DateTime.TryParse(_value.ToString(), out DateTime resultado))
            {
                resultado = onError(this);
            }
            return resultado;
        }

        public override string ToString()
        {
            return _value != null ? _value.ToString() : "";
        }

        public string TryString(Func<Castable, string> onError)
        {
            if(_value == null)
            {
                return onError(this);
            }            

            return _value.ToString();           
        }

        private Exception ThrownException(Type tipo)
        {
            return new Exception($"Unable to convert the value \"{_value}\" to the type \"{tipo.Name}\"");
        }
    }
}
