using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BookStore.Helper
{
    public class LogExceptions
    {
        public void LogException(Exception ex)
        {
            string logFilePath = @"C:\Users\Hp1\Desktop\log\error.log";

            // Append the exception details to the log file
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] Exception: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine();
            }

            Console.WriteLine("Exception logged to {0}", logFilePath);
        }

    }
}