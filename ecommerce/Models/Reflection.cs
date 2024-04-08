using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Models
{
    public class DataNamesMapper<TEntity> where TEntity : class, new()
    {
        public IEnumerable<TEntity> Map(DataTable table)
        {
            //Step 1 - Get the Column Names
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

            //Step 2 - Get the Property Data Names
            var properties = (typeof(TEntity)).GetProperties().Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any()).ToList();

            //Step 3 - Map the data
            List<TEntity> entities = new List<TEntity>();
            foreach (DataRow row in table.Rows)
            {
                TEntity entity = new TEntity();
                foreach (var prop in properties)
                {
                    Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        public static void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = GetDataNames(type, prop.Name);

            foreach (var columnName in columnNames)
            {
                if (!String.IsNullOrWhiteSpace(columnName) && row.Table.Columns.Contains(columnName))
                {
                    var propertyValue = row[columnName];
                    if (propertyValue != DBNull.Value)
                    {
                        ParsePrimitive(prop, entity, row[columnName]);
                        break;
                    }
                    else
                    {
                        if ((prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)))
                        {
                            //ParsePrimitive(prop, entity, Convert.ToDateTime("01/01/1900"));
                            ParsePrimitive(prop, entity, null);
                            break;
                        }
                    }
                }
            }
        }

        public static List<string> GetDataNames(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType().Name == "DataNamesAttribute").FirstOrDefault();
            if (property != null)
            {
                return ((DataNamesAttribute)property).ValueNames;
            }
            return new List<string>();
        }

        private static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
            }
            else if ((prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?)) || (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Int32?)))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, int.Parse(value.ToString()), null);
                }
            }
            else if ((prop.PropertyType == typeof(Int64) || prop.PropertyType == typeof(Int64?)))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, Convert.ToInt64(value.ToString()), null);
                }
            }
            else if ((prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?)))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, Convert.ToDecimal(value.ToString()), null);
                }
            }
            else if ((prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, Convert.ToDateTime(value.ToString()), null);
                }
            }
            else if ((prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(Boolean)))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, Convert.ToBoolean(value.ToString()), null);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DataNamesAttribute : Attribute
    {
        protected List<string> _valueNames { get; set; }

        public List<string> ValueNames
        {
            get
            {
                return _valueNames;
            }
            set
            {
                _valueNames = value;
            }
        }

        public DataNamesAttribute()
        {
            _valueNames = new List<string>();
        }

        public DataNamesAttribute(params string[] valueNames)
        {
            _valueNames = valueNames.ToList();
        }
    }
}
