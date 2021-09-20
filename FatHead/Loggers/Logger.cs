using FatHead.Loggers.Interfaces;
using System;
using System.IO;

namespace FatHead.Loggers
{
    public class Logger : ILogger
    {
        private string _directory;
        private string _fileName;
        private string _filePath;

        /// <summary>
        /// Constructor
        /// </summary>
        public Logger()
        {
            _directory = Directory.GetCurrentDirectory();
            _fileName = "Logs.txt";
            _filePath = string.Format("{0}/{1}", _directory, _fileName);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="directory">System.String File Directory</param>
        /// <param name="fileName">System.String File Name</param>
        /// <param name="filePath">System.String Full File Path</param>
        public Logger(string directory, string fileName, string filePath)
        {
            _directory = directory;
            _fileName = fileName;
            _filePath = filePath;
        }

        /// <summary>
        /// Writes the log event to the file path
        /// </summary>
        /// <param name="log">FatHead.Loggers.Interfaces.ILog</param>
        public void Log(ILog log)
        {
            string logMessage = string.Format("Error code: {0} Message: {1} Time: {2}{3}", log.ErrorCode, log.Message, log.LogTime.ToLongDateString(), Environment.NewLine);

            try
            {
                System.IO.File.AppendAllText(_filePath, logMessage);
            }
            catch
            {

            }
        }
    }
}
