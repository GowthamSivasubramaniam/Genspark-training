using System.IO;

namespace VSM.Misc
{
    public interface IFileLogger
    {
        void LogData(string message);
        void LogError(string message, Exception? ex = null);
    }

    public class FileLogger : IFileLogger
    {
        private readonly string _dataLogPath;
        private readonly string _errorLogPath;

        public FileLogger()
        {
            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(logDir);
            _dataLogPath = Path.Combine(logDir, "data.log");
            _errorLogPath = Path.Combine(logDir, "error.log");
        }

        public void LogData(string message)
        {
            File.AppendAllText(_dataLogPath, $"{DateTime.Now:u} [DATA] {message}{Environment.NewLine}");
        }

        public void LogError(string message, Exception? ex = null)
        {
            var errorMsg = $"{DateTime.Now:u} [ERROR] {message}";
            if (ex != null)
                errorMsg += $"{Environment.NewLine}{ex}{Environment.NewLine}";
            File.AppendAllText(_errorLogPath, errorMsg + Environment.NewLine);
        }
    }
}