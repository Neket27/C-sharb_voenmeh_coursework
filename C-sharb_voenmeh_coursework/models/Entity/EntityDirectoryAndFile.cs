namespace app.models.Entity
{
    public abstract class EntityDirectoryAndFile : BaseViewModel
    {
        #region Variebles

        public string FullName { get; set; }
        public string Name { get; set; }

        #endregion

        #region Constructors

        protected EntityDirectoryAndFile(string name)
        {
            Name = name;
        }

        #endregion
    }
}