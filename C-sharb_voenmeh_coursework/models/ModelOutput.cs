using System.Collections.ObjectModel;
using app.models.Entity;

namespace app.models;

public class ModelOutput : BaseViewModel
{
    public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFilesLeftPanel { get; set; } =
        new ObservableCollection<EntityDirectoryAndFile>();

    public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFiles { get; set; } =
        new ObservableCollection<EntityDirectoryAndFile>();

    public EntityDirectoryAndFile SelectedFileEntity { get; set; }
    public string PathIcon { get; set; }
    public string TextInPreview { get; set; }
    public string FilePath { get; set; } = "���� ���������";
    public string Name { get; set; } = "���� ���������";
    private bool _isEnabled;

    private bool _isPanelVisible;

    public bool IsPanelVisible
    {
        get { return _isPanelVisible; }
        set
        {
            _isPanelVisible = value;
            OnPropertyChanged("IsPanelVisible");
        }
    }
}