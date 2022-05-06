using SqlServerDEID.Common.Globals.Models;

namespace SqlServerDEID.Common
{
    public interface IDEID
    {
        void RunTransform(string filePath);
        void RunTransform(string data, FileType fileType);
        void RunTransform(Database database);
        bool Cancel();
    }
}