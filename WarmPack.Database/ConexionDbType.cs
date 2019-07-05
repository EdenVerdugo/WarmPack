using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Database
{
    //basado en https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
    public enum ConexionDbType
    {
        BigInt = DbType.Int64,
        Binary = DbType.Binary,
        Bit = DbType.Boolean,
        Char = DbType.String,
        Date = DbType.Date,
        DateTime = DbType.DateTime,
        DateTime2 = DbType.DateTime2,
        DateTimeOffset = DbType.DateTimeOffset,
        Decimal = DbType.Decimal,
        Float = DbType.Double,
        Image = DbType.Binary,
        Int = DbType.Int32,
        Money = DbType.Decimal,
        NChar = DbType.StringFixedLength,
        NText = DbType.String,
        Numeric = DbType.Decimal,
        NVarChar = DbType.String,
        Real = DbType.Single,
        SmallDateTime = DbType.DateTime,
        SmallInt = DbType.Int16,
        SmallMoney = DbType.Decimal,
        Text = DbType.String,
        Time = DbType.Time,
        Timestamp = DbType.Binary,
        TinyInt = DbType.Byte,
        UniqueIdentifier = DbType.Guid,
        VarBinary = DbType.Binary,
        VarChar = DbType.String,
        Xml = DbType.Xml
    }
}
