using JetBrains.Annotations;

namespace Explorer.Shared.ViewModels;

public class FileViewModel : FileEntityViewModel
{
    
    public FileViewModel(string fileInfo) : base(fileInfo)
    {
    }
    
    public FileViewModel(FileInfo fileInfo) : base(fileInfo.Name)
    {
        FullName = fileInfo.FullName;
    }
    
    
}