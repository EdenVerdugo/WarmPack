using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WarmPack.Classes;

namespace WarmPack.Database
{
    public class ConexionParameters
    {
        public List<ConexionParameter> Parameters { get; set; }

        public ConexionParameters()
        {
            Parameters = new List<ConexionParameter>();
        }

        public void Add(string name, ConexionDbType type, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, null, type == ConexionDbType.VarChar || type == ConexionDbType.NVarChar ? 300 : 0, direction));
        }

        public void Add(string name, ConexionDbType type, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, null, size, direction));
        }

        #region Add(name, type, value)

        public void Add<T>(string name, ConexionDbType type, T value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, byte value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, sbyte value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, int value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, uint value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, short value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, ushort value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, long value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, ulong value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, float value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, double value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, char value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, bool value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, string value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, decimal value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }

        public void Add(string name, ConexionDbType type, DateTime value)
        {
            Parameters.Add(new ConexionParameter(name, type, value));
        }
        #endregion

        #region Add(name, type, value, direction)

        public void Add<T>(string name, ConexionDbType type, T value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, type == ConexionDbType.VarChar || type == ConexionDbType.NVarChar ? 300 : 0, direction));
        }

        public void Add(string name, ConexionDbType type, byte value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, sbyte value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, int value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, uint value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, short value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, ushort value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, long value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, ulong value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, float value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, double value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, bool value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, string value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 500, direction));
        }

        public void Add(string name, ConexionDbType type, decimal value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }

        public void Add(string name, ConexionDbType type, DateTime value, ParameterDirection direction)
        {
            Parameters.Add(new ConexionParameter(name, type, value, 0, direction));
        }
        #endregion

        #region Add(name, type, value, direction, size)

        public void Add<T>(string name, ConexionDbType type, T value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, byte value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, sbyte value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, short value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, ushort value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, int value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, uint value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, long value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, ulong value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, float value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, double value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, decimal value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, char value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, bool value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, string value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }

        public void Add(string name, ConexionDbType type, DateTime value, ParameterDirection direction, int size)
        {
            Parameters.Add(new ConexionParameter(name, type, value, size, direction));
        }
        #endregion


        public Castable Value(string parameterName)
        {
            var p = Parameters.FirstOrDefault(x => x.Name == parameterName);

            if (p == null)
            {
                throw new Exception("No se encontro el parametro.");
            }

            return new Castable(p.Value);
        }

        public Castable this[string parameterName]
        {
            get
            {
                return Value(parameterName);
            }
        }

        public void Clear()
        {
            Parameters.Clear();
        }
    }
}
