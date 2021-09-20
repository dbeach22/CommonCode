using FatHead.Enums;
using FatHead.Files.Interfaces;
using FatHead.Loggers;
using FatHead.Loggers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FatHead.Files
{
    public class FileAccess : IFileAccess
    {
        private ILogger _logger;

        /// <summary>
        /// Constructor default ILogger
        /// </summary>
        /// <param name="logger">ILogger</param>
        public FileAccess(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Load in data from files with comma seperated values and with column headers that match the generic model's properties.
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="filePath">System.String File path</param>
        /// <returns>A List of T</returns>
        public IList<T> LoadFromFile<T>(string filePath) where T : class, new()
        {
            T model = new T();
            IList<T> loadFile = new List<T>();
            IList<string> lines = new List<string>();
            IList<string> headers = new List<string>();
            IList<PropertyInfo> cols = model.GetType().GetProperties().ToList();

            try
            {
                lines = System.IO.File.ReadAllLines(filePath).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            //The first line is the headers and the second line is the first record.
            //If the number of rows is less than 2 then there is a header, but no rows or
            //The file was not read correctly.
            if (lines.Count < 2)
            {
                throw new IndexOutOfRangeException("File is empty or missing.");
            }

            //Loads the first row headers.
            headers = lines[0].Split(',').ToList();

            //Removes the headers from the list of rows after storing them in headers so we dont have to skip that row later.
            lines.RemoveAt(0);


            foreach (string row in lines)
            {
                model = new T();

                //Splits the data to match the order of the headers
                List<string> dataRow = row.Split(',').ToList();

                //Loop through the headers and compare them to the property names of T
                //If they are the same store the data in the model's property
                for (int i = 0; i < headers.Count; i++)
                {
                    foreach (PropertyInfo c in cols)
                    {
                        if (c.Name == headers[i])
                        {
                            c.SetValue(model, Convert.ChangeType(dataRow[i], c.PropertyType));
                        }
                    }
                }

                loadFile.Add(model);
            }

            return loadFile;
        }

        /// <summary>
        /// Saves a list of T with properties as headers to a specific file path 
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="models">A List of T</param>
        /// <param name="filePath">System.string File path</param>
        /// <param name="append">System.Boolean If append if false WriteAllLines if its true AppendAllLines</param>
        public void SaveToFile<T>(IList<T> models, string filePath, bool append) where T : class, new()
        {
            List<string> lines = new List<string>();
            StringBuilder builder = new StringBuilder();

            //Verify that the list is not null and that the list has at least one model to write to the file
            if (models == null || models.Count == 0)
            {
                throw new ArgumentException("models", "The list is null or empty.");
            }

            List<PropertyInfo> cols = models[0].GetType().GetProperties().ToList();

            //Add the headers
            for (int i = 0; i < cols.Count; i++)
            {
                builder.Append(cols[i].Name);
                //The last column does not need a comma after it
                if (i < cols.Count - 1)
                {
                    builder.Append(", ");
                }
            }

            lines.Add(builder.ToString());

            //Add the rows
            foreach (T row in models)
            {
                builder = new StringBuilder();

                for (int i = 0; i < cols.Count; i++)
                {
                    builder.Append(cols[i].GetValue(row));
                    //The last column does not need a comma after it
                    if (i < cols.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }

                lines.Add(builder.ToString());
            }

            try
            {
                if (append)
                {
                    System.IO.File.AppendAllLines(filePath, lines);
                }
                else
                {
                    System.IO.File.WriteAllLines(filePath, lines);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }
        }
    }
}
