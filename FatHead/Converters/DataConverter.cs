using FatHead.Attributes.Interfaces;
using FatHead.Converters.Interfaces;
using FatHead.Enums;
using FatHead.Loggers;
using FatHead.Loggers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FatHead.Converters
{
    public class DataConverter : IDataConverter
    {
        private ILogger _logger;

        /// <summary>
        /// Constructor with ILogger
        /// </summary>
        /// <param name="logger">ILogger</param>
        public DataConverter(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Converts rows in a DataTable to a list of generic models
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="dataTable">System.DataTable</param>
        /// <returns>List of T</returns>
        public IList<T> ConvertModelFromDataTable<T>(DataTable dataTable) where T : class, new()
        {
            T model = new T();
            IList<T> models = new List<T>();
            IList<string> columns = new List<string>();
            IList<PropertyInfo> cols = model.GetType().GetProperties().ToList();

            try
            {
                //Loads the column names in the correct order
                foreach (DataColumn dc in dataTable.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    model = new T();

                    for (int i = 0; i < columns.Count; i++)
                    {
                        foreach (PropertyInfo c in cols)
                        {
                            var attributes = c.GetCustomAttributes(false);
                            var mapping = attributes.FirstOrDefault();
                            var mapsto = mapping as IAttribute;
                            string attribute = string.Empty;
                            if (mapsto != null)
                            {
                                attribute = mapsto.AttributeValue;
                            }

                            if (c.Name == columns[i] || attribute == columns[i])
                            {
                                //Database values can be null and can cause converting the value to the model to error
                                //This will catch each null value and log the error.
                                try
                                {
                                    c.SetValue(model, Convert.ChangeType(dr.ItemArray[i], c.PropertyType));
                                }
                                catch (Exception ex)
                                {
                                    _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
                                }
                            }
                        }
                    }

                    models.Add(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            return models;
        }

        /// <summary>
        /// Copies one model's properties to another if the property names are the same.
        /// Copy database models to display models that may or may not have all of the same properties
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <typeparam name="U">Generic Model</typeparam>
        /// <param name="modelToCopy">The generic model to copy</param>
        /// <param name="modelToCopyTo">The generic model to copy to</param>
        public void ConvertModelFromModel<T, U>(T modelToCopy, U modelToCopyTo)
            where T : class, new()
            where U : class, new()
        {
            IList<PropertyInfo> main = modelToCopy.GetType().GetProperties().ToList();
            IList<PropertyInfo> copy = modelToCopyTo.GetType().GetProperties().ToList();

            try
            {
                foreach (PropertyInfo m in main)
                {
                    foreach (PropertyInfo c in copy)
                    {
                        if (m.Name == c.Name)
                        {
                            modelToCopyTo.GetType().GetProperty(c.Name).SetValue(modelToCopy, modelToCopy.GetType().GetProperty(m.Name));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }
        }
    }
}
