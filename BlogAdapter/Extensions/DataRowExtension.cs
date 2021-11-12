using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlogAdapter.Extensions
{
    public static class DataRowExtension
    {
        /// <summary>
        /// Returns an object with the specified type, from a DataRow. New object will have as properties the DataRow columns
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned</typeparam>
        /// <param name="dataRow">Row used as source to create the typed object</param>
        /// <returns></returns>
        public static T ToTypedObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);
                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }
            return item;
        }

        /// <summary>
        /// Returns an collection of objects of the specified type, from a DataRow. New objects will have as properties the DataRow columns
        /// </summary>
        /// <typeparam name="T">Type of the objects to be returned in a collection</typeparam>
        /// <param name="dataRows">Rows used as source to create the list of with the typed object</param>
        /// <returns></returns>
        public static List<T> ToTypedCollection<T>(this DataRowCollection dataRows) where T : new() 
        {
            List<T> typedCollection = new List<T>();
            foreach (DataRow row in dataRows)
            {
                T typedObject = row.ToTypedObject<T>();
                typedCollection.Add(typedObject);
            }
            return typedCollection;
        }
        
        //TODO: Fix error when consuming API due to casting
        public static dynamic ToBlendObject(this DataRow dataRow)
        {
            IDictionary<string, object> entity = new ExpandoObject();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                entity.Add(column.ColumnName, dataRow[column]);
            }
            return entity;
        }

        /// <summary>
        /// Returns the PropertyInfo of the given type and given attribute name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>        
        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(ColumnAttribute), false) && p.GetCustomAttributes(typeof(ColumnAttribute), false).Cast<ColumnAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
