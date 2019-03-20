using System;
using System.Text;
using Interfaces.Logic;

namespace Logic
{
    public class ExceptionHelper : IExceptionHelper
    {
        public string GetExceptionInfo(Exception exception)
        {
            return GetExInfo(exception);
        }

        private string GetExInfo(Exception ex, StringBuilder exInfoBuilder = null)
        {
            if (exInfoBuilder == null)
                exInfoBuilder = new StringBuilder();

            exInfoBuilder.AppendLine($"ExceptionType: {ex.GetType()}");
            exInfoBuilder.AppendLine();
            exInfoBuilder.AppendLine($"ExceptionMessage: {ex.Message}");
            exInfoBuilder.AppendLine();
            exInfoBuilder.AppendLine($"Call Stack:");
            exInfoBuilder.AppendLine(ex.StackTrace?.Trim() ?? string.Empty);

            if (ex.InnerException != null)
            {
                exInfoBuilder.AppendLine();
                exInfoBuilder.AppendLine("InnerException");
                GetExInfo(ex.InnerException, exInfoBuilder);
            }

            return exInfoBuilder.ToString();
        }
    }
}