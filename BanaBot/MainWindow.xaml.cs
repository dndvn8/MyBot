using LoLLauncher;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ini;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BanaBot.Properties;
using LoLLauncher.RiotObjects.Platform.Game;


namespace BanaBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IComponentConnector
    {
        public int _currentThreadID = 0;
        private List<string> accountRunningList = new List<string>();
        public bool BuyBoost = false;
        public int connectedAccs;
        private StackPanel currentStackPanelSelection;
        public Process GarenaProcess;
        public string GarenaToken = "";
        public static MainWindow Instance;
        private bool isLogged = true;
        //private List<Tuple<StackPanel, DockPanel>> listAccAutoTuple = new List<Tuple<StackPanel, DockPanel>>();
        public string LmhtDirectory = "";
        public bool rndIcon;
        public bool RndSpell = false;
        public string Spell1 = "flash";
        public string Spell2 = "ignite";
        public string cversion = "";
        public int MaxLevel = 31;
        private bool stopScanMemory = false;
        private Thread updateScanMemory;
        private Thread updateFreezeMemory;
        public string UserWithToken = "";
        public string Champion;
        public string m_account;
        public string m_region;
        public QueueTypes queuetype;
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            
        }

        public string getClientVersion()
        {
            return VersionTextBox.Text;
        }

        
        public string GetLmhtPath(string name)
        {
            Process[] processesByName = Process.GetProcessesByName(name);
            if (processesByName.Length > 0)
            {
                string path = processesByName[0].MainModule.FileName.Replace(@"GameData\Apps\LoLVN\Air\LOLClient.exe", "");
                StringBuilder builder = new StringBuilder(path);
                builder.Append(@"GameData\Apps\lolVN\");
                return builder.ToString();
            }
            return string.Empty;
        }


        public string getUserName()
        {
            Process[] p = Process.GetProcessesByName("GarenaMessenger");
            int index = 0;
            if (p.Count() > 0)
            {
                Process process = p[0];
                ProcessMemory memory = new ProcessMemory(process.Id);
                memory.StartProcess();
                string str = memory.ReadStringAscii(memory.ImageAddress() + 0x91be1c, 0x10);
                GarenaProcess = process;
                return str;
            }
            return string.Empty;
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            var SettingLocation = AppDomain.CurrentDomain.BaseDirectory + @"settings.ini";
            {
                var newfile = File.Create(SettingLocation);
                newfile.Close();

                string content =
                    "[General]\nChampion=" + SelectChampionComboBox.Text + "\nQueueType=" + QueueTypeComboBox.Text + "\nSpell1=" + Spell1ComboBox.Text +
                    "\nSpell2=" + Spell2ComboBox.Text + "\nRegion=" + RegionComboBox.Text + "\nMaxLevel=" + MaxLevelTextBox.Text +
                    "\nClientVersion=" + VersionTextBox.Text + "\nDirectories=" + DirectoryTextBox.Text;
                var separator = new string[] { "\n" };
                string[] contentlines = content.Split(separator, StringSplitOptions.None);
                File.WriteAllLines(SettingLocation, contentlines);
            }
            stopScanMemory = true;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Print("++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Print("VN BanaBot version 1.0.0.3 loaded");
            Print("++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Print("brought to you by imsosharp.",2);
            Print("Edited by Hi I'm Banana - 0936.898.626");
            RegionComboBox.ItemsSource = Enums.regions;
            Spell1ComboBox.ItemsSource = Enums.spells;
            Spell2ComboBox.ItemsSource = Enums.spells;
            QueueTypeComboBox.ItemsSource = Enums.queues;
            SelectChampionComboBox.ItemsSource = Enums.champions;
            InitChecks();
            LoadSettings();
            ScanMemory();
            FreezeMemory();

        }

        public void ScanMemory()
        {
            updateScanMemory = new Thread(() =>
            {
                Action method = null;
                while (!stopScanMemory)
                {
                    StartLmht.KillBug();
                    if (getUserName() != null)
                    {
                        if (method == null)
                        {
                            method = () => UsernameTextBox.Text = getUserName();
                        }
                        UsernameTextBox.Dispatcher.BeginInvoke(method, new object[0]);
                    }
                    Thread.Sleep(1000);
                }
            });
            updateScanMemory.Start();
        }

       

        public void FreezeMemory()
        {
            updateFreezeMemory = new Thread(() =>
            {
                Action method = null;
                while (true)
                {
                    Process p = Process.GetProcessesByName("GarenaMessenger")[0];
                    ProcessMemory memory_ = new ProcessMemory(p.Id);
                    memory_.StartProcess();
                    memory_.WriteStringAscii(memory_.ImageAddress() + 0x91BEAC, "Banabot VN - Hi I'm Banana - 0936.898.626");
                    Thread.Sleep(100);
                }
            });
            updateFreezeMemory.Start();
        }

        public void Print(string text)
        {
            base.Dispatcher.Invoke(() => ConsoleRichTextBox.AppendText(string.Concat(new object[] { "[", DateTime.Now, "] : ", text, "\n" })));
            base.Dispatcher.Invoke(() => ConsoleRichTextBoxTT.AppendText(string.Concat(new object[] { "[", DateTime.Now, "] : ", text, "\n" })));
        }

        public void Print(string text, int newlines)
        {
            ConsoleRichTextBox.AppendText(string.Concat(new object[] { "[", DateTime.Now, "] : ", text }));
            for (int i = 0; i < newlines; i++)
            {
                ConsoleRichTextBox.AppendText("\n");
            }
        }

        public void Print(RichTextBox richTextBox, string text)
        {
            base.Dispatcher.Invoke(() => richTextBox.AppendText(string.Concat(new object[] { "[", DateTime.Now, "] : ", text, "\n" })));
        }

        public void InitChecks()
        {
            var SettingLocation = AppDomain.CurrentDomain.BaseDirectory + @"settings.ini";
            if (!File.Exists(SettingLocation))
            {
                var newfile = File.Create(SettingLocation);
                newfile.Close();
                string content =
                    "[General]\nChampion=AMUMU\nQueueType=ARAM\nSpell1=EXHAUST\nSpell2=HEAL\nRegion=VN\nMaxLevel=31\nClientVersion=5.3.15_02_09_18_53\nDirectories=C:";
                var separator = new string[] { "\n" };
                string[] contentlines = content.Split(separator, StringSplitOptions.None);
                File.WriteAllLines(SettingLocation, contentlines);
            }
        }

        private void MaxLevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(MaxLevelTextBox.Text) > 31)
                {
                    MaxLevelTextBox.Text = "31";
                    e.Handled = true;
                }
                if (Convert.ToInt32(MaxLevelTextBox.Text) < 1)
                {
                    MaxLevelTextBox.Text = "1";
                    e.Handled = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnlyNumberValidation(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryTextBox.IsEnabled = false;
            QueueTypeComboBox.IsEnabled = false;
            SelectChampionComboBox.IsEnabled = false;
            Spell1ComboBox.IsEnabled = false;
            Spell2ComboBox.IsEnabled = false;
            RegionComboBox.IsEnabled = false;
            VersionTextBox.IsEnabled = false;
            MaxLevelTextBox.IsEnabled = false;
            Start();
        }

        public void Start()
        {
            try
            {
                if (isLogged && UsernameTextBox.Text != "")
                {
                    GarenaToken = StartLmht.RunAndGetToken();
                    if (GarenaToken == "")
                    {
                        Print("Lấy mã token thất bại.");
                        StartLmht.Kill_LMHT();
                    }
                    else
                    {
                        
                        StartLmht.Kill_LMHT();
                        Print("Lấy mã token thành công => chạy auto nào...");
                        m_account = UsernameTextBox.Text;
                        m_region = RegionComboBox.Text;
                        cversion = getClientVersion();
                        LmhtDirectory = DirectoryTextBox.Text;
                        Champion = SelectChampionComboBox.Text;
                        Spell1 = Spell1ComboBox.Text;
                        Spell2 = Spell2ComboBox.Text;
                        MaxLevel = Convert.ToInt32(MaxLevelTextBox.Text);
                        cversion = VersionTextBox.Text;
                        queuetype = (QueueTypes)Enum.Parse(typeof(QueueTypes), QueueTypeComboBox.Text);
                        Volibot bot = new Volibot(m_account, GarenaToken, m_region, LmhtDirectory, connectedAccs, queuetype, SummonerLabel,LevelLabel,CurrentIpLabel,GainIpLabel,SIconImage);
                    }
                    
                }
            }
            catch (Exception)
            {
            }
        }
        public void StartAgain()
        {
                    
                    GarenaToken = StartLmht.RunAndGetTokenAgain();
                    StartLmht.Kill_LMHT();
                    //StartLmht.Kill_LMHT();
                    Print("Lấy mã token thành công => chạy auto nào...");
                    Volibot bot = new Volibot(m_account, GarenaToken, m_region, LmhtDirectory, connectedAccs, queuetype, SummonerLabel, LevelLabel, CurrentIpLabel, GainIpLabel, SIconImage);
        }

        public void LoadSettings()
        {
            try
            {
                IniFile iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + "settings.ini");
                SelectChampionComboBox.Text = iniFile.IniReadValue("General", "Champion");
                QueueTypeComboBox.Text = iniFile.IniReadValue("General", "QueueType");
                Spell1ComboBox.Text = iniFile.IniReadValue("General", "Spell1");
                Spell2ComboBox.Text = iniFile.IniReadValue("General", "Spell2");
                RegionComboBox.Text = iniFile.IniReadValue("General", "Region");
                MaxLevelTextBox.Text = iniFile.IniReadValue("General", "MaxLevel");
                VersionTextBox.Text = iniFile.IniReadValue("General", "ClientVersion");
                DirectoryTextBox.Text = iniFile.IniReadValue("General", "Directories");
                //DirectoryTextBox.Text = GetLmhtPath("LolClient");
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }

        public void updateLabelName(Label label, string name)
        {
            base.Dispatcher.Invoke(delegate
            {
                string str = "Tên nhân vật: " + name;
                if (str.Length > 50)
                {
                    str = str.Substring(0, 0x2f) + "...";
                }
                label.Content = str;
            });
        }

        public void updateSummonerIcon(Image currentSummonerIcon, int profileIconId)
        {
            base.Dispatcher.Invoke(delegate
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(string.Concat(new object[] { "http://ddragon.leagueoflegends.com/cdn/",  VersionTextBox.Text.Substring(0, 5), "/img/profileicon/", profileIconId, ".png" }));
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                currentSummonerIcon.Source = image;
            });
        }

        public void updateIpBalance(Label currentIp, double ipBalance)
        {
            base.Dispatcher.Invoke(delegate
            {
                string str = "IP: " + ipBalance;
                if (str.Length > 50)
                {
                    str = str.Substring(0, 0x2f) + "...";
                }
                currentIp.Content = str;
            });
        }

        public void updateIpGain(Label gainIp, double ipBalance)
        {
            base.Dispatcher.Invoke(delegate
            {
                string str = "IP kiếm được: " + ipBalance;
                if (str.Length > 50)
                {
                    str = str.Substring(0, 0x2f) + "...";
                }
                gainIp.Content = str;
            });
        }
        public void updateLabelLevel(Label label, string name)
        {
            base.Dispatcher.Invoke(delegate
            {
                string str = "Cấp độ: " + name;
                if (str.Length > 50)
                {
                    str = str.Substring(0, 0x2f) + "...";
                }
                label.Content = str;
            });
        }

        public void updateLabelCount(Label label, double name)
        {
            base.Dispatcher.Invoke(delegate
            {
                string str = "Số trận đã chơi: " + name;
                if (str.Length > 50)
                {
                    str = str.Substring(0, 0x2f) + "...";
                }
                label.Content = str;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LmhtDirectory = DirectoryTextBox.Text;
            
            ProcessStartInfo startInfo = new ProcessStartInfo();
            DirectoryInfo info2 = new DirectoryInfo(MainWindow.Instance.LmhtDirectory);
            DirectoryInfo parent = info2.Parent.Parent.Parent;
            startInfo.FileName = parent.FullName + @"\Lien Minh Huyen Thoai.exe";
            Process.Start(startInfo);
            //Process.Start(startInfo);

        }




       

    }

}


