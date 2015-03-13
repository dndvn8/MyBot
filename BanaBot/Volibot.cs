/*
 * RiotBot.cs is part of the opensource VoliBot AutoQueuer project.
 * Credits to: shalzuth, Maufeat, imsosharp
 * Find assemblies for this AutoQueuer on LeagueSharp's official forum at:
 * http://www.joduska.me/
 * You are allowed to copy, edit and distribute this project,
 * as long as you don't touch this notice and you release your project with source.
 */

using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects;
using LoLLauncher.RiotObjects.Leagues.Pojo;
using LoLLauncher.RiotObjects.Platform.Game.Practice;
using LoLLauncher.RiotObjects.Platform.Harassment;
using LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto;
using LoLLauncher.RiotObjects.Platform.Login;
using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
using LoLLauncher.RiotObjects.Platform.Statistics.Team;
using LoLLauncher.RiotObjects.Platform.Summoner;
using LoLLauncher.RiotObjects.Platform.Summoner.Boost;
using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
using LoLLauncher.RiotObjects.Platform.Summoner.Runes;
using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using LoLLauncher.RiotObjects.Team;
using LoLLauncher.RiotObjects.Team.Dto;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using LoLLauncher.RiotObjects.Platform.Game.Map;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using BanaBot.Data;
using LoLLauncher.RiotObjects.Platform.Summoner.Icon;
using LoLLauncher.RiotObjects.Platform.Catalog.Icon;
using PvPNETConnect.RiotObjects.Platform.ServiceProxy.Dispatch;
using PvPNETConnect.RiotObjects.Platform.Trade;

namespace BanaBot
{
    internal class Volibot
    {
        public static Volibot Instance;
        public Label CurrentSummonerName;
        public Label CurrentSummonerStatus;
        public LoginDataPacket loginPacket = new LoginDataPacket();
        public GameDTO currentGame = new GameDTO();
        public LoLConnection connection = new LoLConnection();
        public List<ChampionDTO> availableChamps = new List<ChampionDTO>();
        public ChampionDTO[] availableChampsArray;
        public bool firstTimeInLobby = true;
        public bool firstTimeInPostChampSelect = true;
        public bool firstTimeInQueuePop = true;
        public bool firstTimeInCustom = true;
        public Process exeProcess;
        public string ipath;
        public string Accountname;
        public string Password;
        public int threadID;

        public string m_accessToken { get; set; }
        public int m_leaverBustedPenalty { get; set; }
        public int m_queueDodgerPenalty { get; set; }

        public Label CurrentIp { get; set; }

        public Label CurrentIpGain { get; set; }
        

        public Label CurrentLevel { get; set; }

        public double ipBalance { get; set; }
        public double countBattle { get; set; }

        public double ipGain { get; set; }
        public double ipGain1 { get; set; }

        public string Spell1 { get; set; }

        public string Spell2 { get; set; }
        public string ChampionId { get; set; }

        public Image CurrentSummonerIcon { get; set; }
        public double sumLevel { get; set; }
        public double archiveSumLevel { get; set; }
        public double rpBalance { get; set; }
        public QueueTypes queueType { get; set; }
        public QueueTypes actualQueueType { get; set; }

        public string region { get; set; }
        public string regionURL;
        public bool QueueFlag;

        public Volibot(string username, string password, string reg, string path, int threadid, QueueTypes QueueType, Label summonerLabel, Label leveLabel, Label lblIp, Label lblIpGain, Image imageSummonerIcon)
        {
            ipath = path;
            Accountname = username;
            Password = password;
            threadID = threadid;
            queueType = QueueType;
            region = reg;
            CurrentLevel = leveLabel;
            CurrentIp = lblIp;
            CurrentIpGain = lblIpGain;
            CurrentSummonerName = summonerLabel;
            CurrentSummonerIcon = imageSummonerIcon;
            connection.OnConnect += connection_OnConnect;
            connection.OnDisconnect += connection_OnDisconnect;
            connection.OnError += connection_OnError;
            connection.OnLogin += connection_OnLogin;
            connection.OnLoginQueueUpdate += connection_OnLoginQueueUpdate;
            connection.OnMessageReceived += connection_OnMessageReceived;
            switch (region)
            {
                case "TH":
                    connection.Connect(username, password, Region.TH, MainWindow.Instance.cversion);
                    break;
                case "SGMY":
                    connection.Connect(username, password, Region.SGMY, MainWindow.Instance.cversion);
                    break;
                case "TW":
                    connection.Connect(username, password, Region.TW, MainWindow.Instance.cversion);
                    break;
                case "PH":
                    connection.Connect(username, password, Region.PH, MainWindow.Instance.cversion);
                    break;
                case "VN":
                    connection.Connect(username, password, Region.VN, MainWindow.Instance.cversion);
                    break;
            }
        }


        public async void connection_OnMessageReceived(object sender, object message)
        {
            if (message.GetType() == typeof(LcdsServiceProxyResponse))
            {
                LcdsServiceProxyResponse response = message as LcdsServiceProxyResponse;
                 HandleProxyResponse(response);
            }
            if (!(message is GameDTO))
            {
                if (message.GetType() == typeof(TradeContractDTO))
                {
                    string asyncVariable0;
                    TradeContractDTO tradeDto = message as TradeContractDTO;
                    if ((tradeDto != null) && ((((asyncVariable0 = tradeDto.State) != null) && (asyncVariable0 == "PENDING")) && (tradeDto != null)))
                    {
                        await  connection.AcceptTrade(tradeDto.RequesterInternalSummonerName, (int)tradeDto.RequesterChampionId);
                    }
                }
                else if (message is PlayerCredentialsDto)
                {
                     firstTimeInPostChampSelect = true;
                    string str = ipath + "GAME\\";
                    PlayerCredentialsDto credentials = message as PlayerCredentialsDto;
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.WorkingDirectory = str;
                    startInfo.FileName = "League of Legends.exe";
                    startInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"" + credentials.ServerIp + " " +
                                          credentials.ServerPort + " " + credentials.EncryptionKey + " " +
                                          credentials.SummonerId + "\"";
                    updateStatus("Bật Game LMHT, vui lòng chờ...", Accountname);
                    new Thread(() =>
                    {
                        exeProcess = Process.Start(startInfo);
                        exeProcess.Exited += exeProcess_Exited;
                        while (exeProcess.MainWindowHandle == IntPtr.Zero) ;
                        exeProcess.PriorityClass = ProcessPriorityClass.Idle;
                        exeProcess.EnableRaisingEvents = true;
                    }).Start();
                }
                else if (!(message is GameNotification) && !(message is SearchingForMatchNotification))
                {
                    if (message is EndOfGameStats)
                    {
                        MatchMakerParams matchMakerParams = new MatchMakerParams();
                        if ( queueType == QueueTypes.INTRO_BOT)
                        {
                            matchMakerParams.BotDifficulty = "INTRO";
                        }
                        else if ( queueType == QueueTypes.BEGINNER_BOT)
                        {
                            matchMakerParams.BotDifficulty = "EASY";
                        }
                        else if ( queueType == QueueTypes.MEDIUM_BOT)
                        {
                            matchMakerParams.BotDifficulty = "MEDIUM";
                        }
                        if (( sumLevel == 3.0) && ( actualQueueType == QueueTypes.NORMAL_5x5))
                        {
                             queueType =  actualQueueType;
                        }
                        else if (( sumLevel == 6.0) && ( actualQueueType == QueueTypes.ARAM))
                        {
                             queueType =  actualQueueType;
                        }
                        else if (( sumLevel == 7.0) && ( actualQueueType == QueueTypes.NORMAL_3x3))
                        {
                             queueType =  actualQueueType;
                        }
                        matchMakerParams.QueueIds = new Int32[1] { (int)queueType };
                        SearchingForMatchNotification notification = await  connection.AttachToQueue(matchMakerParams);
                        SearchingForMatchNotification m = notification;
                        if (m.PlayerJoinFailures == null)
                        {
                             updateStatus("Trong hàng chờ: " + queueType.ToString(),  Accountname);
                        }
                        else
                        {
                            bool queuedodge = false;
                            List<QueueDodger>.Enumerator enumerator = m.PlayerJoinFailures.GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    QueueDodger current = enumerator.Current;
                                    updateStatus(String.Format("Không thể vào hàng chờ, nguyên nhân là: {0} - {1}.", current.Summoner.Name, current.ReasonFailed), Accountname);
                                    if (current.ReasonFailed == "LEAVER_BUSTED")
                                    {
                                         m_accessToken = current.AccessToken;
                                        if (current.LeaverPenaltyMillisRemaining >  m_leaverBustedPenalty)
                                        {
                                             m_leaverBustedPenalty = current.LeaverPenaltyMillisRemaining;
                                        }
                                    }
                                    if (current.ReasonFailed == "QUEUE_DODGER")
                                    {
                                        queuedodge = true;
                                        if (current.DodgePenaltyRemainingTime > m_queueDodgerPenalty)
                                        {
                                            m_queueDodgerPenalty = current.DodgePenaltyRemainingTime;
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                enumerator.Dispose();
                            }
                            if (String.IsNullOrEmpty(m_accessToken))
                            {
                                if (queuedodge)
                                {
                                    int seconds = (int) (m_queueDodgerPenalty/1000)%60;
                                    int minutes = (int) ((m_queueDodgerPenalty/(1000*60))%60);
                                    int hours = (int) ((m_queueDodgerPenalty/(1000*60*60))%24);
                                    updateStatus(
                                        "Rất tiếc, bạn cần đợi " + hours + " giờ, " + minutes + " phút, " + seconds +
                                        " giây phạt bởi hệ thống queue dodger của liên minh huyền thoại", Accountname);
                                    Thread.Sleep(TimeSpan.FromMilliseconds(m_queueDodgerPenalty));
                                    m = await connection.AttachToQueue(matchMakerParams);
                                    updateStatus(
                                        "Gia nhập thành công hàng chờ", Accountname);
                                }
                                else
                                {
                                    foreach (var failure in m.PlayerJoinFailures)
                                    {
                                        updateStatus("Lỗi: " + failure.ReasonFailed, Accountname);
                                        connection.Disconnect();
                                        
                                    }
                                }
                            }
                            else
                            {
                                 updateStatus("Rất tiếc, bạn cần đợi: " + (((float)( m_leaverBustedPenalty / 0x3e8)) / 60f) + "phút phạt bởi hệ thống LEAVER BUSTED của liên minh huyền thoại",  Accountname);
                                Thread.Sleep(TimeSpan.FromMilliseconds((double) m_leaverBustedPenalty));
                                notification = await  connection.AttachToLowPriorityQueue(matchMakerParams,  m_accessToken);
                                m = notification;
                                if (m.PlayerJoinFailures == null)
                                {
                                     updateStatus("Gia nhập thành công hàng chờ ưu tiên thấp!",  Accountname);
                                }
                                else
                                {
                                     updateStatus("Có lỗi khi tham gia hàng đợi ưu tiên thấp.\nĐang ngắt kết nối...",  Accountname);
                                     connection.Disconnect();
                                }
                            }
                        }
                    }
                    else if (message.ToString().Contains("EndOfGameStats"))
                    {
                        EndOfGameStats eog = new EndOfGameStats();
                         connection_OnMessageReceived(sender, eog);
                         exeProcess.Exited -= new EventHandler( exeProcess_Exited);
                         exeProcess.Kill();
                        Thread.Sleep(500);
                        if ( exeProcess.Responding)
                        {
                            Process.Start("taskkill /F /IM \"League of Legends.exe\"");
                        }
                        Volibot bot = this;
                        LoginDataPacket introduced70 = await  connection.GetLoginDataPacketForUser();
                        bot.loginPacket = introduced70;
                         archiveSumLevel =  sumLevel;
                         sumLevel =  loginPacket.AllSummonerData.SummonerLevel.Level;
                        ipGain1 = ipGain;
                        ipGain = loginPacket.IpBalance - ipBalance;
                        
                        if (CurrentIp != null)
                        {
                            MainWindow.Instance.updateIpBalance(CurrentIp, loginPacket.IpBalance);
                        }
                        if (CurrentIpGain != null)
                        {
                            updateStatus(string.Concat(new object[] { "Ip kiếm được: ", ipGain - ipGain1 }),
                                Accountname);
                            MainWindow.Instance.updateIpGain(CurrentIpGain, ipGain);
                        }
                        if (sumLevel != archiveSumLevel)
                        {
                            levelUp();
                        }
                    }
                }
            }
            else
            {
                GameDTO game = message as GameDTO;
                switch (game.GameState)
                {
                    case "START_REQUESTED":
                        break;

                    case "FAILED_TO_START":
                        Console.WriteLine("Failed to Start!");
                        break;

                    case "CHAMP_SELECT":
                        if ( firstTimeInLobby)
                        {
                            firstTimeInLobby = false;
                            updateStatus("Đang chọn tướng", Accountname);
                            object obj = await connection.SetClientReceivedGameMessage(game.Id, "CHAMP_SELECT_CLIENT");
                            if (queueType != QueueTypes.ARAM)
                            {
                                if (MainWindow.Instance.Champion != "" && MainWindow.Instance.Champion != "RANDOM")
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!MainWindow.Instance.RndSpell)
                                    {
                                        Spell1 = Enums.spellToId(MainWindow.Instance.Spell1);
                                        Spell2 = Enums.spellToId(MainWindow.Instance.Spell2);
                                    }
                                    else
                                    {
                                        Random random = new Random();
                                        List<int> spellList = new List<int> {13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4};

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);
                                    await connection.SelectChampion(Enums.championToId(MainWindow.Instance.Champion));
                                    await connection.ChampionSelectCompleted();
                                }
                                else if (MainWindow.Instance.Champion == "RANDOM")
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!MainWindow.Instance.RndSpell)
                                    {
                                        Spell1 = Enums.spellToId(MainWindow.Instance.Spell1);
                                        Spell2 = Enums.spellToId(MainWindow.Instance.Spell2);
                                    }
                                    else
                                    {
                                        Random random = new Random();
                                        List<int> spellList = new List<int> {13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4};

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);
                                    IEnumerable<ChampionDTO> randAvailableChampsArray = availableChampsArray.Shuffle();
                                    await
                                        connection.SelectChampion(
                                            randAvailableChampsArray.First(champ => champ.Owned || champ.FreeToPlay)
                                                .ChampionId);
                                    await connection.ChampionSelectCompleted();

                                }
                                else
                                {

                                    int Spell1;
                                    int Spell2;
                                    if (!MainWindow.Instance.RndSpell)
                                    {
                                        Spell1 = Enums.spellToId(MainWindow.Instance.Spell1);
                                        Spell2 = Enums.spellToId(MainWindow.Instance.Spell2);
                                    }
                                    else
                                    {
                                        Random random = new Random();
                                        List<int> spellList = new List<int> {13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4};

                                        int index = random.Next(spellList.Count);
                                        int index2 = random.Next(spellList.Count);

                                        int randomSpell1 = spellList[index];
                                        int randomSpell2 = spellList[index2];

                                        if (randomSpell1 == randomSpell2)
                                        {
                                            int index3 = random.Next(spellList.Count);
                                            randomSpell2 = spellList[index3];
                                        }

                                        Spell1 = Convert.ToInt32(randomSpell1);
                                        Spell2 = Convert.ToInt32(randomSpell2);
                                    }

                                    await connection.SelectSpells(Spell1, Spell2);

                                    await
                                        connection.SelectChampion(
                                            availableChampsArray.First(champ => champ.Owned || champ.FreeToPlay)
                                                .ChampionId);
                                    await connection.ChampionSelectCompleted();
                                }
                            }
                        }
                        break;

                    case "POST_CHAMP_SELECT":
                         firstTimeInLobby = false;
                        if ( firstTimeInPostChampSelect)
                        {
                             firstTimeInPostChampSelect = false;
                             updateStatus("(Post Champ Select)",  Accountname);
                        }
                        break;

                    case "IN_QUEUE":
                         updateStatus("Trong hàng chờ",  Accountname);
                        break;

                    case "TERMINATED":
                         updateStatus("Quay lại hàng chờ",  Accountname);
                         firstTimeInPostChampSelect = true;
                         firstTimeInQueuePop = true;
                        break;

                    case "JOINING_CHAMP_SELECT":
                        if ( firstTimeInQueuePop && game.StatusOfParticipants.Contains("1"))
                        {
                             updateStatus("Bấm nút đồng ý vào chọn tướng",  Accountname);
                             firstTimeInQueuePop = false;
                             firstTimeInLobby = true;
                            await  connection.AcceptPoppedGame(true);
                        }
                        break;

                    default:
                         updateStatus("[Mặc định]" + game.GameStateString,  Accountname);
                        break;
                }
            }
        }

        void exeProcess_Exited(object sender, EventArgs e)
        {
            updateStatus("Khởi động lại game LMHT", Accountname);
            Thread.Sleep(1000);
            if ( loginPacket.ReconnectInfo != null &&  loginPacket.ReconnectInfo.Game != null)
            {
                 connection_OnMessageReceived(sender, loginPacket.ReconnectInfo.PlayerCredentials);
            }
            else
                 connection_OnMessageReceived(sender, new EndOfGameStats());
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private void updateStatus(string status, string accname)
        {
            MainWindow.Instance.Print(string.Concat(new object[] { "[", accname, "]: ", status }));
        }

        private async void RegisterNotifications()
        {
            object obj1 = await  connection.Subscribe("bc",  connection.AccountID());
            object obj2 = await  connection.Subscribe("cn",  connection.AccountID());
            object obj3 = await  connection.Subscribe("gn",  connection.AccountID());
        }

        private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {
            if (positionInLine <= 0)
                return;
             updateStatus("STT đăng nhập: " + (object)positionInLine, Accountname);
        }

        private void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread((async () =>
            {
                updateStatus("Đang kết nối...", Accountname);
                 RegisterNotifications();
                 loginPacket = await  connection.GetLoginDataPacketForUser();
                if (loginPacket.AllSummonerData == null)
                {
                    Random rnd = new Random();
                    String summonerName = Accountname;
                    if (summonerName.Length > 16)
                        summonerName = summonerName.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                    LoLLauncher.RiotObjects.Platform.Summoner.AllSummonerData sumData = await connection.CreateDefaultSummoner(summonerName);
                    loginPacket.AllSummonerData = sumData;
                    updateStatus("Tự động generate tên cho summoner " + summonerName, Accountname);
                }
                sumLevel = loginPacket.AllSummonerData.SummonerLevel.Level;
                string sumName = loginPacket.AllSummonerData.Summoner.Name;
                double sumId = loginPacket.AllSummonerData.Summoner.SumId;
                rpBalance = loginPacket.RpBalance;
                if (sumLevel >= MainWindow.Instance.MaxLevel)
                {
                    connection.Disconnect();
                    updateStatus("Summoner: " + sumName + " đã max level.", Accountname);
                    updateStatus("Đã ngắt kết nối.", Accountname);
                    //Program.lognNewAccount();
                    //return;
                }
                else
                {
                    if (sumLevel < 3.0 && queueType == QueueTypes.NORMAL_5x5)
                    {
                         updateStatus("Cần phải đạt level 3 trước khi tham gia vào chế độ NORMAL_5X5.", Accountname);
                         updateStatus("Về chế độ Co-Op vs AI (Beginner) cho tới khi đạt level 3.", Accountname);
                        queueType = QueueTypes.BEGINNER_BOT;
                        actualQueueType = QueueTypes.NORMAL_5x5;
                    }
                    else if (sumLevel < 6.0 && queueType == QueueTypes.ARAM)
                    {
                         updateStatus("Cần phải đạt level 6 trước khi tham gia vào chế độ ARAM.", Accountname);
                         updateStatus("Về chế độ Co-Op vs AI (Beginner) cho tới khi đạt level 6", Accountname);
                        queueType = QueueTypes.BEGINNER_BOT;
                        actualQueueType = QueueTypes.ARAM;
                    }
                    else if (sumLevel < 7.0 && queueType == QueueTypes.NORMAL_3x3)
                    {
                         updateStatus("Cần phải đạt level 7 trước khi tham gia vào chế độ NORMAL_3x3", Accountname);
                         updateStatus("Về chế độ Co-Op vs AI (Beginner) cho tới khi đạt level 7", Accountname);
                        queueType = QueueTypes.BEGINNER_BOT;
                        actualQueueType = QueueTypes.NORMAL_3x3;
                    }
                }

                if ((( rpBalance == 400.0) && MainWindow.Instance.BuyBoost) && ( sumLevel < 5.0))
                    {
                         updateStatus("Mua XP Boost",  Accountname);
                        try
                        {
                            new Task(new Action( buyBoost)).Start();
                        }
                        catch (Exception exception)
                        {
                             updateStatus("Không thể mua RP Boost.\n" + exception,  Accountname);
                        }
                    }

                updateStatus("Đăng nhập với tên " + loginPacket.AllSummonerData.Summoner.Name + " @ level " + loginPacket.AllSummonerData.SummonerLevel.Level, Accountname);
                if ( CurrentSummonerName != null)
                {
                    MainWindow.Instance.updateLabelName( CurrentSummonerName,  loginPacket.AllSummonerData.Summoner.Name);
                }
                if ( CurrentIp != null)
                {
                     ipBalance =  loginPacket.IpBalance;
                    MainWindow.Instance.updateIpBalance( CurrentIp,  ipBalance);
                }
                if ( CurrentIpGain != null)
                {
                     ipGain = 0.0;
                    MainWindow.Instance.updateIpGain( CurrentIpGain,  ipGain);
                }
                if ( CurrentLevel != null)
                {
                    MainWindow.Instance.updateLabelLevel( CurrentLevel,  loginPacket.AllSummonerData.SummonerLevel.Level.ToString());
                }
                if ( CurrentSummonerIcon != null)
                {
                    MainWindow.Instance.updateSummonerIcon( CurrentSummonerIcon,  loginPacket.AllSummonerData.Summoner.ProfileIconId);
                }
                availableChampsArray = await connection.GetAvailableChampions();
                PlayerDTO player = await connection.CreatePlayer();
                if ( loginPacket.ReconnectInfo != null &&  loginPacket.ReconnectInfo.Game != null)
                {
                     connection_OnMessageReceived(sender,  loginPacket.ReconnectInfo.PlayerCredentials);
                }
                else
                     connection_OnMessageReceived(sender, new EndOfGameStats());
            })).Start();
        }

        private void connection_OnError(object sender, Error error)
        {
            if (error.Message.Contains("is not owned by summoner"))
            {
                return;
            }
            else if (error.Message.Contains("Your summoner level is too low to select the spell"))
            {
                var random = new Random();
                var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                int index = random.Next(spellList.Count);
                int index2 = random.Next(spellList.Count);

                int randomSpell1 = spellList[index];
                int randomSpell2 = spellList[index2];

                if (randomSpell1 == randomSpell2)
                {
                    int index3 = random.Next(spellList.Count);
                    randomSpell2 = spellList[index3];
                }

                int Spell1 = Convert.ToInt32(randomSpell1);
                int Spell2 = Convert.ToInt32(randomSpell2);
                return;
            }
             updateStatus("error received:\n" + error.Message, Accountname);
        }

        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            MainWindow.Instance.connectedAccs--;
            updateStatus("Đã ngắt kết nối", Accountname);
            MainWindow.Instance.StartAgain();
        }

        private void connection_OnConnect(object sender, EventArgs e)
        {
            MainWindow.Instance.connectedAccs++;
        }
        private void HandleProxyResponse(LcdsServiceProxyResponse Response)
        {
            updateStatus(Response.MethodName, Accountname);
        }

        public void levelUp()
        {
            updateStatus("Level Up: " + sumLevel, Accountname);
            rpBalance = loginPacket.RpBalance;
            if (sumLevel >= MainWindow.Instance.MaxLevel)
            {
                connection.Disconnect();
                //bool connectStatus = await connection.IsConnected();
                if (!connection.IsConnected())
                {
                }
            }
            if (rpBalance == 400.0 && MainWindow.Instance.BuyBoost)
            {
                updateStatus("Buying XP Boost", Accountname);
                try
                {
                    Task t = new Task(buyBoost);
                    t.Start();
                }
                catch (Exception exception)
                {
                    updateStatus("Không thể mua XP Boost.\n" + exception, Accountname);
                }
            }
        }
        async void buyBoost()
        {
            try
            {
                if (region == "EUW")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + "1.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + "1.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else if (region == "EUNE")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.Substring(0, 3).ToLower() + "1.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.Substring(0, 3).ToLower() + "1.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else if (region == "NA")
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + "2.lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + "2.lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
                else
                {
                    string url = await connection.GetStoreUrl();
                    HttpClient httpClient = new HttpClient();
                    Console.WriteLine(url);
                    await httpClient.GetStringAsync(url);
                    string storeURL = "https://store." + region.ToLower() + ".lol.riotgames.com/store/tabs/view/boosts/1";
                    await httpClient.GetStringAsync(storeURL);
                    string purchaseURL = "https://store." + region.ToLower() + ".lol.riotgames.com/store/purchase/item";
                    List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                    storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                    storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                    storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                    storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                    storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                    storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                    HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                    await httpClient.PostAsync(purchaseURL, httpContent);
                    updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                    httpClient.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
