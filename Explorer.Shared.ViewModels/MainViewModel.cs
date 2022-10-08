using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Explorer.Shared.ViewModels;

public class MainViewModel: INotifyPropertyChanged
{
    private string _mainDiskName;

    public string MainDiskName { get; set; }
 
    
        
    public MainViewModel()
    {
        MainDiskName = Environment.SystemDirectory;// Передача системной дириктории 
            
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}