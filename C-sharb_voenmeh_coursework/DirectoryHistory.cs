using app.History;
using System;
using System.Collections;
using System.Collections.Generic;

namespace app
{
    internal class DirectoryHistory : IDirectoryHistory
    {
        #region Variebles

        private DirectoryNode _head;
        public DirectoryNode Current { get; set; }

        #endregion

        #region Constructors

        public DirectoryHistory(string directoryPath, string directoryPathName)
        {
            _head = new DirectoryNode(directoryPath, directoryPathName);
            Current = _head;
        }

        #endregion

        #region Events

        public event EventHandler HistoryChanged;

        #endregion

        #region Functions

        public bool CanMoveBack => Current.PreviousNode != null;
        public bool CanMoveForward => Current.NextNode != null;

      
        public void Add(string filePath, string name)
        {
            DirectoryNode node = new DirectoryNode(filePath, name);
            Current.NextNode = node;
            node.PreviousNode = Current;
            Current = node;

            RaiseHistoryChanged();
        }


        public void MoveBack()
        {
            var prev = Current.PreviousNode;
            Current = prev!;
            RaiseHistoryChanged();
        }

        public void MoveForward()
        {
            var next = Current.NextNode;

            Current = next!;

            RaiseHistoryChanged();
        }

        public IEnumerator<DirectoryNode> GetEnumerator()
        {
            yield return Current;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
          return  GetEnumerator();
        }
        private void RaiseHistoryChanged()
        {
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}