using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using WarmPack.Helpers;

namespace WarmPack.Extensions
{
    public static class IEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> lst)
        {
            var result = new ObservableCollection<T>();

            for (int i = 0; i < lst.Count(); i++)
            {
                result.Add(lst.ElementAt(i));
            }

            return result;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data, string tableName, params Expression<Func<T, object>>[] includeOnly)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            if (includeOnly.Length == 0)
            {

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                object[] values = new object[props.Count];

                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
            }
            else
            {
                foreach (var property in includeOnly)
                {
                    var propertyName = ExpressionsHelper.GetPropertyName<T>(property);

                    //propertyList.Add(propertyName);
                    table.Columns.Add(propertyName);
                }

                foreach (T item in data)
                {
                    var row = table.NewRow();

                    foreach (DataColumn col in table.Columns)
                    {
                        row[col.ColumnName] = props[col.ColumnName].GetValue(item);
                    }

                    table.Rows.Add(row);
                }

            }

            table.TableName = tableName;

            return table;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data, string tableName = "")
        {
            return ToDataTable(data, tableName);
        }
    }
}
