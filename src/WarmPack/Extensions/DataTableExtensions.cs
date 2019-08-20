using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace WarmPack.Extensions
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(this DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (FieldInfo pro in temp.GetFields())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName]);
                    }
                    else
                    {
                        continue;
                    }
                }

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        public static string ToJson(this DataTable dt)
        {
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var renglon = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    renglon.Add(col.ColumnName, row[col]);
                }

                table.Add(renglon);
            }

            return ""; //JsonConvert.SerializeObject(table); wait for this
        }
    }
}
