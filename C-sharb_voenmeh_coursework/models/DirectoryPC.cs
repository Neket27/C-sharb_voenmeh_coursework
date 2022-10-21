using System.IO;
using app.models.Entity;

namespace app.models
{
    public class DirectoryPC : EntityDirectoryAndFile
    {
        #region Constructors

        public DirectoryPC(string directoryName) : base(directoryName)
        {
            FullName = directoryName;
        }

        public DirectoryPC(DirectoryInfo directoryName) :
            base(directoryName.Name) // в базовый конструктор отдаём только имя, а в путь записываем полный путь
        {
            FullName = directoryName.FullName;
        }

        #endregion
    }
}