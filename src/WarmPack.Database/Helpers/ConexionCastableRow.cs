using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Classes;

namespace WarmPack.Database.Helpers
{
    public class ConexionCastableRow
    {
        internal IDataReader _reader;

        public ConexionCastableRow(IDataReader reader)
        {
            this._reader = reader;
        }

        public Castable this[string columnName]
        {
            get
            {
                return new Castable(this._reader[columnName]);
            }
        }

        public Castable this[int columnIndex]
        {
            get
            {
                return new Castable(this._reader[columnIndex]);
            }
        }

        public Castable Column(string columnName)
        {
            return this[columnName];
        }

        public Castable Column(int columnIndex)
        {
            return this[columnIndex];
        }
    }
}
