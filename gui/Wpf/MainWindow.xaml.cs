using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;
using System.Media;
using System.Text;
using System.Windows.Media.Animation;
using LiveCharts.Defaults;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;

//using System.Data;

namespace Wpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        SoundPlayer plr = new SoundPlayer("oof.wav");

        private bool isInfoOn = false;

        private List<User> friendsList = new List<User>();
        private List<StackPanel> spList = new List<StackPanel>();

        private SolidColorBrush[] defaultColors = new SolidColorBrush[3];
        private SolidColorBrush[] alternativeColors = new SolidColorBrush[3];
        string[] defaultHex = new string[] { "#6B7A8F", "#DCC7AA", "#F7882F" };
        string[] alternativeHex = new string[] { "#597392", "#3a75d8", "#4ef6f4" };

        private Button changeImgBtn = new Button();
        private Button saveBtn = new Button();
        private Button cancelBtn = new Button();
        private string currTag;
        private ImageBrush currImageBrush = new ImageBrush();
        private ObservableValue[] ob;


        private int[] statusAmount = new int[] { 1, 5, 10 };

        private delegate void ChangeWin(string text);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        #endregion
        #region Propeties
        public bool IsInfoOn
        {
            get
            {
                return isInfoOn;
            }
            set
            {
                isInfoOn = value;
            }
        }
        public object SeriersCollection { get; private set; }
        public SeriesCollection SeriesCollection { get; set; }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(path).ToString();
            path = Directory.GetParent(path).ToString();

            this.FontSize = 16;
            this.Icon = new BitmapImage(new Uri(path + @"\ProfilePicture\sales.png"));

            InitColor(defaultColors, defaultHex);
            InitColor(alternativeColors, alternativeHex);

            SetBackgroundColor(defaultColors);

            #region User profile
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(path + @"\ProfilePicture\default.png"));
            myBrush.Stretch = Stretch.UniformToFill;
            profPic.Fill = myBrush;
            profPic.Height = 60;
            profPic.Width = 60;
            #endregion

            popUpSetting.VerticalOffset = -btnSetting.ActualHeight;
            popUpSetting.HorizontalOffset = -btnSetting.ActualWidth;

            //EEEE
            //Add the friend.txt-file in the debug
            //Will change to read from database
            ReadFile("friends.txt");
            CreateSPItem();
            friendsView.ItemsSource = spList;
            CreateStats();

            

            sendCall_Grid.Visibility = Visibility.Hidden;
            addBtn.Click += TypeTagNumber;

            btnBlue.IsEnabled = false;
            currImageBrush = (ImageBrush)profPic.Fill;
            
            Thread statusThread = new Thread(GetActiveWindowTitle);
            statusThread.IsBackground = true;

            statusThread.Start();

        }


        private void GetActiveWindowTitle()
        {

            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            while (true)
            {
                IntPtr handle = GetForegroundWindow();
                if (GetWindowText(handle, Buff, nChars) > 0)
                {
                    ChangeWin dele = new ChangeWin(ChangeText);
                    Thread.Sleep(700);
                    Status.Dispatcher.BeginInvoke(dele, Buff.ToString());
                }
            }
        }

        private void ChangeText(string text)
        {
            Status.Text = text;
        }
        private void InitColor(SolidColorBrush[] br, string[] colors)
        {
            Color c;
            SolidColorBrush scb;
            for (int i = 0; i < br.Length; i++)
            {
                c = new Color();
                c = (Color)ColorConverter.ConvertFromString(colors[i]);
                scb = new SolidColorBrush();
                scb.Color = c;
                br[i] = scb;
            }
        }

        private void SetBackgroundColor(SolidColorBrush[] color)
        {
            left_Grid.Background = color[0];
            center_Grid.Background = color[1];
            right_Grid.Background = color[2];
        }
        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(btnBlue))
                SetBackgroundColor(defaultColors);
            else
                SetBackgroundColor(alternativeColors);

            btnBlue.IsEnabled = !btnBlue.IsEnabled;
            btnVio.IsEnabled = !btnVio.IsEnabled;
        }
        /// <summary>
        /// Create the friends for the list and the listview
        /// </summary>
        /// <param name="path"></param>
        /// <param name="spList"></param>
        private void CreateSPItem()
        {
            spList = new List<StackPanel>();
            TextBlock tbTag;
            TextBlock tbName;
            StackPanel mainSP;
            StackPanel tempSP;


            for (int i = 0; i < friendsList.Count; i++)
            {
                //name
                tbName = new TextBlock();
                tbName.FontSize = 16;
                tbName.VerticalAlignment = VerticalAlignment.Center;
                tbName.Text = friendsList[i].Name;
                //tag
                tbTag = new TextBlock();
                tbTag.FontSize = 14;
                tbTag.VerticalAlignment = VerticalAlignment.Center;
                tbTag.Text = friendsList[i].Tag;
                //picture
                Ellipse ellImg = new Ellipse();
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = friendsList[i].Img;
                imgBrush.Stretch = Stretch.UniformToFill;
                ellImg.Fill = imgBrush;
                ellImg.Height = 56;
                ellImg.Width = ellImg.Height;
                ellImg.Margin = new Thickness(10);

                mainSP = new StackPanel();
                mainSP.Orientation = Orientation.Horizontal;

                //SP for name and tag
                tempSP = new StackPanel();
                tempSP.Orientation = Orientation.Vertical;
                tempSP.VerticalAlignment = VerticalAlignment.Center;
                tempSP.Children.Add(tbName);
                tempSP.Children.Add(tbTag);

                //add to Stackpanel
                mainSP.Children.Add(ellImg);
                mainSP.Children.Add(tempSP);

                spList.Add(mainSP);
            }
        }
        /// <summary>
        /// Reads the files and sets the list
        /// File needs to be in the BIN/DEBUG-directory!!!
        /// </summary>
        /// <param name="filepath"></param>    
        public void ReadFile(string filepath)
        {
            string[] row = File.ReadAllLines(filepath, Encoding.UTF8);
            for (int i = 0; i < row.Length; i++)
            {
                string[] elem = row[i].Split(';');

                User friend;
                if (elem.Length == 2)
                    friend = new User(elem[0], elem[1]);

                else
                    friend = new User(elem[0], elem[1], elem[2]);

                friendsList.Add(friend);
            }
            friendsList.Sort();
        }
        /// <summary>
        /// Opens tag popup for the tag-number input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeTagNumber(object sender, RoutedEventArgs e)
        {
            if (popUpTag.IsOpen)
            {
                (sender as Button).Content = "+";
                (sender as Button).Width = 30;
            }
            else
            {
                (sender as Button).Content = "Cancel";
                (sender as Button).Width = 60;
            }
            popUpTag.IsOpen = !popUpTag.IsOpen;
        }
        /// <summary>
        /// Send the message via "enter"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendingMessage(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        /// <summary>
        /// Key handle for enter/sending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                SendMessage();
        }
        public void SendMessage()
        {
            Regex reg = new Regex(@"\w+");

            if (!reg.IsMatch(InputBox.Text))
            {
                InputBox.Text = String.Empty;
                return;
            }

            DateTime dateTime = DateTime.Now;
            User friend = GetCurrentFriend();

            string dateLine = tBoxName.Text + ": " + dateTime.ToString("dd.MM.yy hh:mm") + "\n" + "    " + InputBox.Text + "\n";

            friend.CurrMessageAmount++;
            friend.AmountSent++;
            friend.MessageContainer = dateLine;
            ShowInputBlock.Text += dateLine;
            InputBox.Text = String.Empty;
            scrollView.ScrollToEnd();

            UpdateChart();
        }

        /// <summary>
        /// Creates the two button remove and stats
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Border CreateCenterButton(string text)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(12);
            Button btn = new Button();
            btn.Content = text;
            border.Child = btn;
            if (text == "Stats")
                btn.Click += ShowStats;
            else
                btn.Click += Removefriend;

            return border;
        }

        /// <summary>
        /// Removes friend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Removefriend(object sender, RoutedEventArgs e)
        {
            User friend = GetCurrentFriend();
            friendsList.Remove(friend);

            CreateSPItem();
            friendsView.ItemsSource = spList;

            sendCall_Grid.Visibility = Visibility.Hidden;
            InputBox.Visibility = Visibility.Hidden;
            Chat_Border.Visibility = Visibility.Hidden;
            messageChart.Visibility = Visibility.Hidden;
            right_Grid.Children.Clear();
        }




        /// <summary>
        /// Shows the friend's name, image and 2 buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectFriend(object sender, SelectionChangedEventArgs e)
        {
            selFriendGrid.Children.Clear();
            StackPanel selectedItemSP;
            StackPanel spNameTag = new StackPanel();
            spNameTag.Orientation = Orientation.Vertical;
            spNameTag.VerticalAlignment = VerticalAlignment.Center;
            StackPanel mainSP = new StackPanel();
            mainSP.Orientation = Orientation.Horizontal;

            //Create name and Tag
            TextBlock tbName = new TextBlock();
            tbName.FontSize = 16;
            tbName.VerticalAlignment = VerticalAlignment.Center;

            TextBlock tbTag = new TextBlock();
            tbTag.FontSize = 14;
            tbTag.VerticalAlignment = VerticalAlignment.Center;

            //Create picture
            Ellipse ellImg = new Ellipse();
            ellImg.Width = 56;
            ellImg.Height = ellImg.Width;
            ellImg.Margin = new Thickness(10);

            //read selected friend
            //save the select elem to tempSP
            selectedItemSP = (StackPanel)friendsView.SelectedItem;
            if (selectedItemSP == null)
            {
                remStatGrid.Children.Clear();
                return;
            }

            //set "selectSP(friend)" with selectedItemSP's data
            ellImg.Fill = (selectedItemSP.Children[0] as Ellipse).Fill;
            StackPanel tempSP = new StackPanel();
            tempSP = selectedItemSP.Children[1] as StackPanel;
            tbName.Text = (tempSP.Children[0] as TextBlock).Text;
            tbTag.Text = (tempSP.Children[1] as TextBlock).Text;
            spNameTag.Children.Add(tbName);
            spNameTag.Children.Add(tbTag);

            currTag = tbTag.Text;
            ShowInputBlock.Clear();
            int tempIndex = 0;

            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Tag == currTag)
                    tempIndex = i;
            }

            ShowInputBlock.Text = friendsList[tempIndex].MessageContainer;

            mainSP.Children.Add(ellImg);
            mainSP.Children.Add(spNameTag);

            selFriendGrid.Children.Add(mainSP);

            //add buttons
            Border remBtnBdr = new Border();
            remBtnBdr = CreateCenterButton("Remove");

            Border statsBtnBdr = new Border();
            statsBtnBdr = CreateCenterButton("Stats");
            Grid.SetColumn(statsBtnBdr, 1);

            sendCall_Grid.Visibility = Visibility.Visible;

            remStatGrid.Children.Add(remBtnBdr);
            remStatGrid.Children.Add(statsBtnBdr);

            sendCall_Grid.Visibility = Visibility.Visible;
            InputBox.Visibility = Visibility.Visible;
            Chat_Border.Visibility = Visibility.Visible;
            right_Grid.Children.Clear();
        }
        /// <summary>                 
        /// Shows the stats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowStats(object sender, RoutedEventArgs e)
        {
            //change button's clickevent and name 
            Button btn = sender as Button;
            btn.Content = "Chat";

            btn.Click -= ShowStats;
            btn.Click += ShowChat;

            GenerateStatusInfo();

            sendCall_Grid.Visibility = Visibility.Hidden;
            InputBox.Visibility = Visibility.Hidden;
            Chat_Border.Visibility = Visibility.Hidden;
            messageChart.Visibility = Visibility.Visible;

        }

        private void CreateStats()
        {
            ob = new ObservableValue[7];
            for (int i = 0; i < ob.Length; i++)
            {
                ob[i] = new ObservableValue(0);
            }

            messageChart.AxisX.Clear();
            messageChart.AxisY.Clear();
            messageChart.AxisY.Add(new Axis
            {
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 10
                },
                MinValue = 0,
                FontSize = 16,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0,0,0)),
                Title = "Messages"
            });
            messageChart.AxisX.Add(new Axis
            {
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1
                },
                MaxValue = 7, 
                MinValue = 0,
                FontSize = 16,
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                Title = "Week",
                Labels = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }
        });



            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Messages amount",
                    Values = new ChartValues<ObservableValue> {ob[0], ob[1], ob[2], ob[3], ob[4], ob[5], ob[6]}
                }
            };

            DataContext = this;

        }

        private void UpdateChart()
        {
            User u = GetCurrentFriend();
            ob[0].Value = Convert.ToDouble(u.CurrMessageAmount);
            ob[6].Value = Convert.ToDouble(u.CurrMessageAmount);

            //


            messageChart.Update(true);

        }
        private User GetCurrentFriend()
        {
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Tag == currTag)
                {
                    return friendsList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Shows the chat-history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowChat(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Content = "Stats";
            btn.Click -= ShowChat;
            btn.Click += ShowStats;

            sendCall_Grid.Visibility = Visibility.Visible;
            InputBox.Visibility = Visibility.Visible;
            Chat_Border.Visibility = Visibility.Visible;
            right_Grid.Children.Clear();

        }
        /// <summary>
        /// Create the information for the right corner 
        /// </summary>
        public void GenerateStatusInfo()
        {
            StackPanel s = new StackPanel();

            TextBlock[] tb = new TextBlock[5];

            for (int i = 0; i < tb.Length; i++)
            {
                tb[i] = new TextBlock();
            }
            User friend = GetCurrentFriend();

            tb[0].Text = "Friends since: " + DateTime.Today.ToString("dd.MM.yyyy");
            tb[1].Text = "Messages sent: " + friend.AmountSent;
            tb[2].Text = "Messages received: " + friend.AmountReceive;
            tb[3].Text = "Total Messages: " + friend.GetTotalMessages();
            tb[4].Text = CreateStatus(friend);

            for (int i = 0; i < tb.Length; i++)
            {
                s.Children.Add(tb[i]);
            }
            s.Margin = new Thickness(8);
            right_Grid.Children.Add(s);
        }

        private string CreateStatus(User friend)
        {
            string status = "Friendship: ";

            if (friend.GetTotalMessages() < statusAmount[0])
                status += "Acquaintance";
            else if (friend.GetTotalMessages() > statusAmount[1])
                status += "Buddies";

            //else if (friend.GetTotalMessages() > statusAmount[2] && friend.GetTotalMessages() < statusAmount[1])
            //    status = "ABF";
            else
                status += "Friends";

            return status;
        }

        /// <summary>
        /// Open setting popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings(object sender, RoutedEventArgs e)
        {
            popUpSetting.IsOpen = !popUpSetting.IsOpen;
        }
        /// <summary>
        /// Set the information for the user e.g. username, ...
        /// </summary>
        public void SetInformation()
        {
            Animation(0);
            //friendsView.Visibility = Visibility.Collapsed;
            Info.Background = new SolidColorBrush(Colors.White);
            tBoxEditName.Text = tBoxName.Text;
            lbTag.Content = "#1236";
            lbDate.Content = DateTime.Now.ToString("dd.MM.yyyy");
            lbTotalFriends.Content = friendsList.Count;
        }

        private void Animation(int width)
        {
            DoubleAnimation db = new DoubleAnimation();
            db.To = width;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            friendsView.BeginAnimation(StackPanel.WidthProperty, db);

        }

        /// <summary>
        /// Shows the user's information e.g. name, message amount, tag, etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ShowUserInfo(object sender, RoutedEventArgs e)
        {
            if (!IsInfoOn)
            {
                SetInformation();
                IsInfoOn = true;
            }
            else
            {
                Animation(250);
                DeleteButtonsInInfo();
                isInfoOn = false;
            }
        }
        /// <summary>
        /// Change the user information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeInformation(object sender, RoutedEventArgs e)
        {
            if (!isInfoOn)
            {
                SetInformation();
                IsInfoOn = true;
            }
            popUpSetting.IsOpen = !popUpSetting.IsOpen;
            tBoxEditName.IsReadOnly = false;

            changeImgBtn.Width = 100;
            changeImgBtn.Height = 35;
            changeImgBtn.VerticalAlignment = VerticalAlignment.Center;
            changeImgBtn.Content = "Change Image";
            changeImgBtn.Click += OpenFileDiaForImg;
            Grid.SetRow(changeImgBtn, 4);
            Grid.SetColumnSpan(changeImgBtn, 2);

            saveBtn.Content = "save";
            saveBtn.Click += SaveInfo;
            Grid.SetRow(saveBtn, 5);

            cancelBtn.Content = "cancel";
            cancelBtn.Click += CancelInfoChanges;
            Grid.SetRow(cancelBtn, 5);
            Grid.SetColumn(cancelBtn, 1);


            Info.Children.Add(changeImgBtn);
            Info.Children.Add(saveBtn);
            Info.Children.Add(cancelBtn);
        }

        /// <summary>
        /// Delete the button, which pop up by editing the infos
        /// </summary>
        public void DeleteButtonsInInfo()
        {
            while (Info.Children.Count > 8)
            {
                Info.Children.RemoveAt(8);
            }
        }
        /// <summary>
        /// Save the user's changes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveInfo(object sender, RoutedEventArgs e)
        {
            tBoxName.Text = tBoxEditName.Text;
            tBoxEditName.IsReadOnly = true;
            currImageBrush = (ImageBrush)profPic.Fill;
            DeleteButtonsInInfo();
        }
        /// <summary>
        /// Cancel the information's changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelInfoChanges(object sender, RoutedEventArgs e)
        {
            profPic.Fill = currImageBrush;
            tBoxEditName.Text = tBoxName.Text;
            tBoxEditName.IsReadOnly = true;
            DeleteButtonsInInfo();
        }
        /// <summary>
        /// Open a filedialog for changing the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileDiaForImg(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDia = new OpenFileDialog();

            fileDia.Filter = "Images (*.png, *.jpg)|*.png; *jpg";
            if (fileDia.ShowDialog() == true)
            {
                ImageBrush tempImgBrush = new ImageBrush();
                tempImgBrush.ImageSource = new BitmapImage(new Uri(fileDia.FileName));

                profPic.Fill = tempImgBrush;
            }
        }

        #region PopUp methods
        private void btnSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            ppuSetName.IsOpen = true;
        }
        private void btnSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            ppuSetName.IsOpen = false;
        }
        private void popUpSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            popUpSetting.IsOpen = false;
        }
        #endregion
    }
}
