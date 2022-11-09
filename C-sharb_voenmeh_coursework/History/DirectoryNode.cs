namespace app.History;

public class DirectoryNode
{
    #region Variebles

    public string DirectoryPath { get; set; }
    public string? DirectoryPathName { get; internal set; }
    public DirectoryNode PreviousNode { get; set; }
    public DirectoryNode NextNode { get; set; }

    #endregion

    #region Constructors

    public DirectoryNode() { }
    
    public DirectoryNode(string directoryPath, string directoryPathName)
    {
        DirectoryPath = directoryPath;
        DirectoryPathName = directoryPathName;
    }

    #endregion
  
}