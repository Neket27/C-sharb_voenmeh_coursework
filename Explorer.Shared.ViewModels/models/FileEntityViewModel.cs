namespace Explorer.Shared.ViewModels;

public abstract class FileEntityViewModel : BaseViewModel
{
    public string FullName { get; set; }
    public string Name { get; set; }

    protected FileEntityViewModel(string name)
    {
        this.Name = name;
    }
}