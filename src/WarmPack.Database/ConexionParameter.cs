using System;
using System.Collections.Generic;
using System.Data;

namespace WarmPack.Database
{
    public class ConexionParameter
    {
        public string Name { get; set; }
        public DbType Type { get; set; }
        public object Value { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }

        public ConexionParameter()
        {
            Name = "";
            Type = DbType.String;
            Size = 0;
            Direction = ParameterDirection.Input;
        }

        public ConexionParameter(string name, ConexionDbType type, object value, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            Type = (DbType)type;
            Value = value ?? DBNull.Value;
            Size = size;
            Direction = direction;
        }
    }

    public class ConexionParameter<T>
    {
        public string Name { get; set; }
        public DbType Type { get; set; }
        public T Value { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }

        public ConexionParameter()
        {
            Name = "";
            Type = DbType.String;
            Size = 0;
            Direction = ParameterDirection.Input;
        }

        public ConexionParameter(string name, ConexionDbType type, T value, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            Type = (DbType)type;                        
            Value = value;
            Size = size;
            Direction = direction;
        }
    }
}
