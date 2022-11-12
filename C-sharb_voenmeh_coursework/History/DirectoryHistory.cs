using System;
using app.History;

namespace C_sharb_voenmeh_coursework.History
{
    internal class DirectoryHistory : IDirectoryHistory
    {
        #region Variebles
        
        public DirectoryNode Current { get; set; } //Поле Current находится в этом классе и сюда наследуется интерфейс IEnumerable для перечисления Сurrent. В IEnumerable поле Сurrent хранится как бы в массиве Сurrent1, Сurrent2.., а у каждого Сurrent есть поле с сылкой на предыдущий и следующий Сurrent. Так у Сurrent3 будет PreviousNode=Сurrent2, а NextNode=Сurrent4

        #endregion

        #region Constructors

        public DirectoryHistory(string directoryPath, string directoryPathName)
        {
            Current = new DirectoryNode(directoryPath, directoryPathName);
        }

        #endregion

        #region Events

        public event EventHandler HistoryChanged;

        #endregion

        #region Functions

        public bool CanMoveBack => Current.PreviousNode != null; //если у текущего Current предыдущая директория не равна null, то в CanMoveBack присваиваем true
        public bool CanMoveForward
        {
            get => Current.NextNode != null;
            set => throw new NotImplementedException();
        }

        public bool CanMoveClose { get; set; } = false;


        public void Add(string filePath, string name)
        {
            DirectoryNode node = new DirectoryNode(filePath, name); //новый нод
            Current.NextNode = node; // текущему ноду ставим в NextNode новый нод
            node.PreviousNode = Current; // А у нового нода PreviousNode записываем текущий нод
            Current = node; // Меняем текущий нод на новый
            CanMoveClose = true;
            RaiseHistoryChanged();
        }

        public void Clear()
        {
            DirectoryNode directoryNode = new DirectoryNode();
            Current.PreviousNode = directoryNode;
            Current.NextNode = directoryNode;
            Current = directoryNode;
            CanMoveClose = false;
            RaiseHistoryChanged();
        }


        public void MoveBack()
        {
            Current = Current.PreviousNode;
            RaiseHistoryChanged();
        }

        public void MoveForward()
        {
            Current = Current.NextNode;
            RaiseHistoryChanged();
        }

        private void RaiseHistoryChanged() // обновление активности кнопок на форме
        {
            HistoryChanged?.Invoke(null, null);
        }

        #endregion
    }
}