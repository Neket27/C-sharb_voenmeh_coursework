using System.Collections.ObjectModel;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Explorer.Shared.ViewModels;

public class MainViewModel: BaseViewModel
{
    #region publicProperties

    public string MainDiskName { get; set; }
    public string FilePath { get; set; }
    public ObservableCollection<string> DerectoriesAndFiles { get; set; } = new ObservableCollection<string>();

    #endregion

    #region Constructor
    public MainViewModel()
    {
        MainDiskName = Environment.SystemDirectory;// Передача системной дириктории 
        foreach (var logicalDrive in Directory.GetLogicalDrives())
        {
            DerectoriesAndFiles.Add(logicalDrive);
        }
    }
    #endregion
}