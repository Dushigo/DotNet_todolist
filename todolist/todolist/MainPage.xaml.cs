using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace todolist
{
    public sealed partial class MainPage : Page
    {
        private int Nb;
        private int PopUpId;
        private string tmpName;
        private List<Task> TasksList;
        public MainPage()
        {
            Nb = 0;
            TasksList = new List<Task>();
            this.InitializeComponent();
            ReadFile();
        }
        private void ItemClickParam(object sender, ItemClickEventArgs e)
        {
            Task task = (Task)e.ClickedItem;
            tmpName = task.TaskName;
            if (task.TaskName != "")
                NameItem.Text = task.TaskName;
            else
                NameItem.PlaceholderText = "Name";
            if (task.Comment != "")
                CommentItem.Text = task.Comment;
            else
                CommentItem.PlaceholderText = "Comment";
            if (task.DateTask != "")
                DateItem.Date = DateTime.Parse(task.DateTask);
            if (task.TimeTask != "")
                TimeItem.Time = TimeSpan.Parse(task.TimeTask);
            BoxState.IsChecked = task.IsChecked;
            PopUpId = task.Id;
            ShowPopupOffsetClicked(new object(), new RoutedEventArgs());
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (AddText.Visibility == Visibility.Collapsed)
            {
                AddText.Visibility = Visibility.Visible;
                sendButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddText.Visibility = Visibility.Collapsed;
                sendButton.Visibility = Visibility.Collapsed;
            }
        }
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (i < TaskList.Items.Count())
            {
                Task task = (Task)TaskList.Items[i];
                if (task.IsChecked == true)
                {
                    TaskList.Items.RemoveAt(i);
                    TasksList.RemoveAt(i);
                    --Nb;
                }
                else
                    ++i;
            }
            WriteFile();
        }
        private void SendClick(object sender, RoutedEventArgs e)
        {
            if (AddText.Text != "")
            {
                Task tmp = new Task
                {
                    TaskName = AddText.Text,
                    IsChecked = false,
                    Comment = "",
                    DateTask = "",
                    TimeTask = "",
                    Id = Nb
                };
                TaskList.Items.Add(tmp);
                TasksList.Add(tmp);
                WriteFile();
                ++Nb;
                AddText.Text = "";
            }
            AddText.Visibility = Visibility.Collapsed;
            sendButton.Visibility = Visibility.Collapsed;
        }
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
        private string ConvertDateToString(DatePicker date)
        {
            return (date.Date.ToString());
        }
        private string ConvertTimeToString(TimePicker time)
        {
            return (time.Time.ToString());
        }
        private void CloseWithSavePopupClicked(object sender, RoutedEventArgs e)
        {
            if (NameItem.Text == String.Empty)
                NameItem.Text = tmpName;
            tmpName = "";
            Task tmp = new Task
            {
                TaskName = NameItem.Text,
                IsChecked = (bool)BoxState.IsChecked,
                Comment = CommentItem.Text,
                DateTask = ConvertDateToString(DateItem),
                TimeTask = ConvertTimeToString(TimeItem),
                Id = PopUpId
            };
            TaskList.Items.Insert(PopUpId, tmp);
            TasksList.Insert(PopUpId, tmp);
            TaskList.Items.RemoveAt(PopUpId + 1);
            TasksList.RemoveAt(PopUpId + 1);
            WriteFile();
            CommentItem.Text = "";
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }
        public async void ReadFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("test.json");
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            TasksList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Task>>(text);
            int i = 0;
            while (i < TasksList.Count())
            {
                TaskList.Items.Add(new Task
                {
                    TaskName = TasksList[i].TaskName,
                    IsChecked = TasksList[i].IsChecked,
                    Comment = TasksList[i].Comment,
                    DateTask = TasksList[i].DateTask,
                    TimeTask = TasksList[i].TimeTask,
                    Id = i
                });
                ++i;
            }
        }
        public async void WriteFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("test.json",
                                                            Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, Newtonsoft.Json.JsonConvert.SerializeObject(TasksList));
        }
    }
}
