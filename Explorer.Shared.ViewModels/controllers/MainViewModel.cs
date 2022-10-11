using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Explorer.Shared.ViewModels;

public class MainViewModel:BaseViewModel
{
    #region publicProperties

    public string MainDiskName { get; set; }
    public string FilePath { get; set; }
    public FileEntityViewModel SelectedFileEntity{ get; set; }
    public ObservableCollection<FileEntityViewModel> DerectoriesAndFiles { get; set; } = new ObservableCollection<FileEntityViewModel>();

    #endregion

    #region Commands 
    public ICommand OpenCommand { get; set; }
    


    #endregion

    #region Constructor

    public MainViewModel()
    {
        OpenCommand = new DelegateCommand(Open);
        MainDiskName = Environment.SystemDirectory; // Передача системной дириктории 
        foreach (var logicalDrive in Directory.GetLogicalDrives()) // Считываем все диски пк в коллекцию
        {
            DerectoriesAndFiles.Add(new DirectoryViewModel(logicalDrive));
        }
    }

    #endregion

    #region Commands Methods

    private void Open(object parameter)
    {
        if (parameter is DirectoryViewModel directoryViewModel)
        {

            FilePath = directoryViewModel.FullName;
            DerectoriesAndFiles.Clear();
            var DirectoryInfo = new DirectoryInfo(FilePath);

            foreach (var directory in DirectoryInfo.GetDirectories())
            {
                DerectoriesAndFiles.Add(new DirectoryViewModel(directory.FullName));
            }

            foreach (var FileInfo in DirectoryInfo.GetFiles())
            {
                DerectoriesAndFiles.Add(new FileViewModel(FileInfo));
            }
        }
    }

    #endregion
    
}