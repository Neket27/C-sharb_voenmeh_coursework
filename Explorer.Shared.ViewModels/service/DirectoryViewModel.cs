namespace Explorer.Shared.ViewModels;

public class DirectoryViewModel : FileEntityViewModel
{
    public DirectoryViewModel(string directoryName) : base(directoryName)
    {
        FullName = directoryName;
    }

    public DirectoryViewModel(DirectoryInfo directoryName):base(directoryName.Name) // � ������� ����������� ����� ������ ���, � � ���� ���������� ������ ����
    {
        FullName = directoryName.FullName; 
    }
    
}