using System;
using System.Collections.Generic;

namespace app.History
{
    public interface IDirectoryHistory 
    {
        #region Variebles

        bool CanMoveBack { get;}
        bool CanMoveForward { get; set; }
        bool CanMoveClose { get; set; }
        DirectoryNode Current { get; set; }

        #endregion

        #region Events

        event EventHandler HistoryChanged;

        #endregion

        #region Functions

        void MoveBack();
        void MoveForward();
        void Add(string filePath, string name);
        void Clear();

        #endregion
    }
}