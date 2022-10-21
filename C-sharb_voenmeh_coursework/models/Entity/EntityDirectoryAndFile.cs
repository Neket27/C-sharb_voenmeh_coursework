namespace app.models.Entity
{
    public abstract class EntityDirectoryAndFile : BaseViewModel
    {
        #region Variebles

        public string FullName { get; set; }
        public string Name { get; set; }
        public string DirectoryName { get; }

        #endregion

        #region Constructors

        protected EntityDirectoryAndFile(string directoryName)
        {
            DirectoryName = directoryName;
        }

        #endregion
    }
}