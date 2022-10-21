using System.IO;
using app.models.Entity;

namespace app.models
{
    public class FilePC : EntityDirectoryAndFile
    {
        #region Constructors

        public FilePC(string directoryName) : base(directoryName)
        {
        }

        public FilePC(FileInfo fileInfo) : base(fileInfo.Name)
        {
            FullName = fileInfo.FullName;
        }

        #endregion
    }
}