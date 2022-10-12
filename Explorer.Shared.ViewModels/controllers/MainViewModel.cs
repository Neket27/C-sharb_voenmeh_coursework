using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Explorer.Shared.ViewModels;

public class MainViewModel:BaseViewModel
{
    #region publicProperties

    public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new ObservableCollection<FileEntityViewModel>();
    public ObservableCollection<DirectoryTabItemViewModel>DirectoryTabItemViewModels { get; set; }=new ObservableCollection<DirectoryTabItemViewModel>();
    public DirectoryTabItemViewModel CurrentDirectoryTabItem { get; set; }
    
    #endregion

    #region Commands 

    #endregion

    #region Constructor

    public MainViewModel()
    {
        AddTabItemViewModel();
        AddTabItemViewModel();
        CurrentDirectoryTabItem = DirectoryTabItemViewModels.FirstOrDefault(); //показываем вкладку модели(первая в списке поумолчанию будет)
    }

    #endregion


    #region Private Methods

    private void AddTabItemViewModel()
    {
        var vm = new DirectoryTabItemViewModel();
        vm.Closed += Vm_Closed;
        
        DirectoryTabItemViewModels.Add(vm);
        }

    private void Vm_Closed(object? sender, EventArgs e)
    {
       if(sender is DirectoryTabItemViewModel directoryTabItemViewModel)
        {
            CloseTab(directoryTabItemViewModel);
        }
    }

    private void CloseTab(DirectoryTabItemViewModel directoryTabItemViewModel)
    {
        directoryTabItemViewModel.Closed -= Vm_Closed; //отписываемся от модели (нужно для корректной работы сборщика мусора)
        DirectoryTabItemViewModels.Remove(directoryTabItemViewModel);
        CurrentDirectoryTabItem = DirectoryTabItemViewModels.FirstOrDefault();
    }

    #endregion



}