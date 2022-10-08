namespace Explorer.Shared.ViewModels;

public class DirectoryViewModel : FileEntityViewModel
{
    public DirectoryViewModel(string DirectoryName) : base(DirectoryName)
    {
        FullName = DirectoryName;
    }

    public DirectoryViewModel(DirectoryInfo DirectoryName):base(DirectoryName.Name)
    {
        FullName = DirectoryName.Name;
    }
    
}