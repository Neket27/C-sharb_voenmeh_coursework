using System;
using System.Windows.Input;
using app.History;
using C_sharb_voenmeh_coursework.History;

namespace C_sharb_voenmeh_coursework.Command;

public class Command: FunctionCommand
{
    #region Commands
    
    

    public ICommand OpenCommand { get; set; }
    public ICommand ClickCommand { get; set; }
    public ICommand CloseCommand { get; set; }

    public ICommand CopyCommand { get; set; }
    public ICommand PasteCommand { get; set; }
    public ICommand CutCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public DelegateCommand MoveBackCommand { get; set; }
    public DelegateCommand MoveForwardCommand { get; set; }

    #endregion


    #region Constructors

    public Command()
    {
        History = new DirectoryHistory("Этот компьютер", "Этот компьютер");
        OpenCommand = new DelegateCommand(Open);
        CloseCommand = new DelegateCommand(Close);
        ClickCommand = new Explorer.Shared.ViewModels.DelegateCommand(Click);
        MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
        MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

        CopyCommand = new DelegateCommand(Copy);
        PasteCommand = new DelegateCommand(Paste);
        DeleteCommand = new DelegateCommand(Delete);
        CutCommand = new DelegateCommand(Cut);

        Name = History.Current.DirectoryPathName;
        FilePath = History.Current.DirectoryPath;

        OpenDirectory();
        History.HistoryChanged += History_HistoryChanged;
    }

    #endregion
    
    # region Function
    
    private void History_HistoryChanged(object? sender, EventArgs e)
    {
        MoveBackCommand?.RaiseCanExecuteChanged();
        MoveForwardCommand?.RaiseCanExecuteChanged();
    }

    private bool OnCanMoveForward(object obj) => History.CanMoveForward;
    private bool OnCanMoveBack(object obj) => History.CanMoveBack;

    private void OnMoveForward(object obj)
    {
        History.MoveForward();
        DirectoryNode current = History.Current;
        FilePath = current.DirectoryPath;
        Name = current.DirectoryPathName;
        OpenDirectory();
    }

    private void OnMoveBack(object obj)
    {
        History.MoveBack();
        DirectoryNode current = History.Current;
        FilePath = current.DirectoryPath;
        Name = current.DirectoryPathName;
        OpenDirectory();
    }
    
    #endregion
    
    
}