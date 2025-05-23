using proxy.Interfaces;
using proxy.Models;
namespace proxy.Services
{
    public class ProxyFile : IFile
    {
        private string _filePath;
        private File _realFile;
        private User _user;
        public ProxyFile(string filePath, User user)
        {
            _filePath = filePath;
            _realFile = new File(_filePath);
            _user = user;
        }
        public string ReadMetadata()
        {
            return _realFile.ReadMetadata();
        }
        public string Read()
        {
          
            switch (_user.role)
            {
                case "admin":
                     
                    return $"User: {_user.Name} [{_user.role}] \n {ReadMetadata()} \n{_realFile.Read()}";
                case "user":
                    return $"User: {_user.Name} [{_user.role}] \n {ReadMetadata()}";
                case "guest":
                    return $"User: {_user.Name} [{_user.role}] \n Access Denied";
                default:
                    return "Unknown Access Denied";
            }
        }
    }
}
