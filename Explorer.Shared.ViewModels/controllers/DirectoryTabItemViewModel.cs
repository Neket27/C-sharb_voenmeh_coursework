using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Explorer.Shared.ViewModels
{
    public class DirectoryTabItemViewModel : BaseViewModel
    {
        #region publicProperties

        public string MainDiskName { get; set; }
        public string FilePath { get; set; }
        public string Title { get; set; } = "Мой компьютер";

        public FileEntityViewModel SelectedFileEntity { get; set; }
        public ObservableCollection<FileEntityViewModel> DirectoriesAndFilesLeftPanel { get; set; } = new ObservableCollection<FileEntityViewModel>();
        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new ObservableCollection<FileEntityViewModel>();


        #endregion

        #region Commands 
        public ICommand OpenCommand { get; set; }
        public ICommand CloseCommand { get; set; }


        #endregion

        #region Events

        public event EventHandler Closed;

        #endregion

        #region Constructor

        public DirectoryTabItemViewModel()
        {
            OpenCommand = new DelegateCommand(Open);
            CloseCommand = new DelegateCommand(OnClose);
            MainDiskName = Environment.SystemDirectory; // Передача системной дириктории 
            foreach (var logicalDrive in Directory.GetLogicalDrives()) // Считываем все диски пк в коллекцию
            {
                DirectoriesAndFiles.Add(new DirectoryViewModel(logicalDrive));
            }

            FileEntityViewModel DirDesktop = new DirectoryViewModel("C:\\Users\\Nikita\\Desktop");
            FileEntityViewModel DirVideo = new DirectoryViewModel("C:\\Users\\Nikita\\Videos");
            FileEntityViewModel DirDocumet = new DirectoryViewModel("C:\\Users\\Nikita\\Documents");
            FileEntityViewModel DirPictures = new DirectoryViewModel("C:\\Users\\Nikita\\Pictures");
            FileEntityViewModel DirMusic = new DirectoryViewModel("C:\\Users\\Nikita\\Music");

            DirDesktop.Name = "Рабочий стол";
            DirVideo.Name = "Видео";
            DirDocumet.Name = "Документы";
            DirPictures.Name = "Изображения";
            DirMusic.Name = "Музыка";
            DirectoriesAndFilesLeftPanel.Add(DirDesktop);
            DirectoriesAndFilesLeftPanel.Add(DirVideo);
            DirectoriesAndFilesLeftPanel.Add(DirPictures);
            DirectoriesAndFilesLeftPanel.Add(DirMusic);

        }

        public static string StrOut, StrIn;
        public static void GetPath(string path)
        {
            string[] astrFolders = Directory.GetFileSystemEntries(@path);
            foreach (string file in astrFolders)
            {
                if (Directory.Exists(@file)) { GetPath(@file); }
                else { FileInfo FN = new FileInfo(@file); File.Copy(FN.FullName, @StrIn + "\\" + FN.Name, true); }
            }

        }

        #endregion

        #region Commands Methods



        private void Open(object parameter)
        {
            if (parameter is DirectoryViewModel directoryViewModel)
            {

                FilePath = directoryViewModel.FullName;
                Title = directoryViewModel.Name;
                DirectoriesAndFiles.Clear();

                var DirectoryInfo = new DirectoryInfo(FilePath);

                foreach (var directory in DirectoryInfo.GetDirectories())
                {
                    DirectoriesAndFiles.Add(new DirectoryViewModel(directory));
                }

                foreach (var FileInfo in DirectoryInfo.GetFiles())
                {
                    DirectoriesAndFiles.Add(new FileViewModel(FileInfo));
                }

            }
        }

        private void OnClose(object ob)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        
    }
}