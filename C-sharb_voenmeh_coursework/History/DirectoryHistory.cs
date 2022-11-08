using app.History;
using System;
using System.Collections;
using System.Collections.Generic;

namespace C_sharb_voenmeh_coursework.History
{
    internal class DirectoryHistory :  IDirectoryHistory
    {
        #region Variebles

        public DirectoryNode Current { get; set; } //Поле Current находится в этом классе и сюда наследуется интерфейс IEnumerable для перечисления Сurrent. В IEnumerable поле Сurrent хранится как бы в массиве Сurrent1, Сurrent2.., а у каждого Сurrent есть поле с сылкой на предыдущий и следующий Сurrent. Так у Сurrent3 будет PreviousNode=Сurrent2, а NextNode=Сurrent4

        #endregion

        #region Constructors

        public DirectoryHistory() { }

        public DirectoryHistory(string directoryPath, string directoryPathName)
        {

            Current = new DirectoryNode(directoryPath, directoryPathName);
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
            DirectoryNode node = new DirectoryNode(filePath, name);  //новый нод
            Current.NextNode = node; // текущему ноду ставим в NextNode новый нод
            node.PreviousNode = Current; // А у нового нода PreviousNode записываем текущий нод
            Current = node; // Меняем текущий нод на новый

            RaiseHistoryChanged();
        }

        public void Clear()
        {
            DirectoryNode directoryNode = new DirectoryNode();
            Current.PreviousNode = directoryNode;
            Current.NextNode = directoryNode;
            Current = directoryNode;
            RaiseHistoryChanged();

        }


        public void MoveBack()
        {
            var prev = Current.PreviousNode;
            Current = prev;
            RaiseHistoryChanged();
        }

        public void MoveForward()
        {
            var next = Current.NextNode;
            Current = next;
            RaiseHistoryChanged();
        }

        private void RaiseHistoryChanged()
        {
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}