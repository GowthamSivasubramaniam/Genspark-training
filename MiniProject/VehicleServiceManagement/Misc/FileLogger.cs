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



// using System;
// using System.IO;
// using System.Text;
// using System.Threading.Tasks;
// using Azure.Storage.Blobs;
// using Azure.Storage.Blobs.Specialized;

// namespace VSM.Misc
// {
//     public interface IFileLogger
//     {
//         Task LogData(string message);
//         Task LogError(string message, Exception? ex = null);
//     }

//     public class BlobLogger : IFileLogger
//     {
//         private readonly AppendBlobClient _dataLogBlob;
//         private readonly AppendBlobClient _errorLogBlob;

//         public BlobLogger(string connectionString, string containerName = "logs")
//         {
//             var blobServiceClient = new BlobServiceClient(connectionString);
//             var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
//             containerClient.CreateIfNotExists();

//             _dataLogBlob = containerClient.GetAppendBlobClient("data.log");
//             _errorLogBlob = containerClient.GetAppendBlobClient("error.log");

           
//             if (!_dataLogBlob.Exists())
//                 _dataLogBlob.CreateIfNotExists();

//             if (!_errorLogBlob.Exists())
//                 _errorLogBlob.CreateIfNotExists();
//         }

//         public async Task LogData(string message)
//         {
//             string logMessage = $"{DateTime.UtcNow:u} [DATA] {message}{Environment.NewLine}";
//             using var ms = new MemoryStream(Encoding.UTF8.GetBytes(logMessage));
//             await _dataLogBlob.AppendBlockAsync(ms);
//         }

//         public async Task LogError(string message, Exception? ex = null)
//         {
//             var errorMsg = $"{DateTime.UtcNow:u} [ERROR] {message}";
//             if (ex != null)
//                 errorMsg += $"{Environment.NewLine}{ex}{Environment.NewLine}";

//             using var ms = new MemoryStream(Encoding.UTF8.GetBytes(errorMsg + Environment.NewLine));
//             await _errorLogBlob.AppendBlockAsync(ms);
//         }
//     }
// }
