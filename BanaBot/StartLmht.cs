using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using MahApps.Metro.Native;

//using System.Windows.Forms;

namespace BanaBot
{
    internal class StartLmht
    {
        private static int counter = 5;
        private static int garenaHeight;
        private static int garenaWidth;
        public const int MOUSEEVENTF_LEFTDOWN = 2;
        public const int MOUSEEVENTF_LEFTUP = 4;
        private static int screenWidth;
        private const int SW_HIDE = 0;
        private static  RECT lpRect = new RECT();

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindow(string ClassName, string WindowName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(2, xpos, ypos, 0, 0);
            mouse_event(4, xpos, ypos, 0, 0);
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        public static string GetCommandLine_lol()
        {
            string str = "";
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName.Equals("lol"))
                    {
                        StringBuilder builder = new StringBuilder();
                        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + process.Id))
                        {
                            foreach (ManagementBaseObject obj2 in searcher.Get())
                            {
                                builder.Append(obj2["CommandLine"]);
                            }
                        }
                        str = builder.ToString().Remove(0, 1);
                    }
                }
                catch (Exception)
                {
                }
            }
            return str;
        }

        public static string RunAndGetToken()
        {
            Process process;
            string str;
            Process[] processesByName = Process.GetProcessesByName("lol");
            if (processesByName.Length > 0)
            {
                process = processesByName[0];
                if (process != null)
                {
                    str = GetCommandLine_lol();
                    if (str != "")
                    {
                        process.Kill();
                        return str;
                    }
                }
            }
            else
            {
                OpenLol(MainWindow.Instance.GarenaProcess.MainWindowHandle);
                CheckProcess("LolClient");
                if (!File.Exists(MainWindow.Instance.LmhtDirectory))
                {
                    MainWindow.Instance.DirectoryTextBox.Text = MainWindow.Instance.GetLmhtPath("LolClient");
                }
                Process[] processArray2 = Process.GetProcessesByName("lol");
                if (processArray2.Length > 0)
                {
                    process = processArray2[0];
                    if (process != null)
                    {
                        str = GetCommandLine_lol();
                        if (str != "")
                        {
                            process.Kill();
                            return str;
                        }
                    }
                
                }
            }
            return "";
        }
        public static string RunAndGetTokenAgain()
        {
            Process process;
            string str;
            Process[] processesByName = Process.GetProcessesByName("lol");
            if (processesByName.Length > 0)
            {
                process = processesByName[0];
                if (process != null)
                {
                    str = GetCommandLine_lol();
                    if (str != "")
                    {
                        process.Kill();
                        return str;
                    }
                }
            }
            else
            {
                OpenLolAgain(MainWindow.Instance.GarenaProcess.MainWindowHandle);
                CheckProcess("LolClient");
                Process[] processArray2 = Process.GetProcessesByName("lol");
                if (processArray2.Length > 0)
                {
                    process = processArray2[0];
                    if (process != null)
                    {
                        str = GetCommandLine_lol();
                        if (str != "")
                        {
                            process.Kill();
                            return str;
                        }
                    }

                }
            }
            return "";
        }

        public static bool IsOpenProc(String ProcName)
        {
            Process[] processes = Process.GetProcessesByName(ProcName);
            return processes.Length > 0;
        }


        public static void CheckProcess(String ProcessN)
        {
            int Lol = 0;
            while (!IsOpenProc(ProcessN) && Lol < 5000)
            {
                Lol += 1;
            }
        }
        public static void KillBug()
        {
            if (IsOpenProc("BsSndRpt"))
            {
                foreach (Process proc in Process.GetProcessesByName("BsSndRpt"))
                {
                    proc.Kill();
                }
            }
        }

        public static void Kill_LMHT()
        {
            if (IsOpenProc("LolClient"))
            {
                foreach (Process process in Process.GetProcessesByName("LolClient"))
                {
                    process.Kill();
                }
            }
        }

        public static void OpenLol(IntPtr hWnd)
        {
            GetWindowRect(hWnd, ref lpRect);
            garenaWidth = lpRect.Right - lpRect.Left;
            garenaHeight = lpRect.Bottom - lpRect.Top;
            screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            MoveWindow(hWnd, screenWidth - 285, 0, 285, 682, true);
            BringWindowToTop(hWnd);
            Thread.Sleep(1000);
            GetWindowRect(hWnd, ref lpRect);
            garenaWidth = lpRect.Right - lpRect.Left;
            garenaHeight = lpRect.Bottom - lpRect.Top;
            screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            LeftMouseClick(lpRect.Left + 25,lpRect.Bottom - 380);
            Thread.Sleep(500);
            LeftMouseClick(lpRect.Left - 100, lpRect.Bottom - 80);
        }
        public static void OpenLolAgain(IntPtr hWnd)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            DirectoryInfo info2 = new DirectoryInfo(MainWindow.Instance.LmhtDirectory);
            DirectoryInfo parent = info2.Parent.Parent.Parent;
            startInfo.FileName = parent.FullName + @"\Lien Minh Huyen Thoai.exe";
            Process.Start(startInfo);
            Thread.Sleep(500);
            LeftMouseClick(lpRect.Left - 100, lpRect.Bottom - 80);
        }


        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
