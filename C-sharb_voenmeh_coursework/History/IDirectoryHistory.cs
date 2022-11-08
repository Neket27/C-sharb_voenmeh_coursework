﻿using System;
using System.Collections.Generic;

namespace app.History
{
    internal interface IDirectoryHistory 
    {
        #region Variebles

        bool CanMoveBack { get;}
        bool CanMoveForward { get;}
        DirectoryNode Current { get; }

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