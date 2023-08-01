using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Motor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Init();
            
        }

        #region CMD
        private void Init()
        {

            EmbeddedApp embeddedApp = new EmbeddedApp(WinEmb, 100, 100);
            WinEmb.Child = embeddedApp;
            
        }
        #endregion

    }

    class EmbeddedApp : HwndHost, IKeyboardInputSink
    {
        private Border WinHoster;
        private double screenW, screenH;
        public EmbeddedApp(Border b, double sW, double sH)
        {
            WinHoster = b;
            screenW = sW;
            screenH = sH;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Windows\\System32\\cmd.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            Process.Start(startInfo);
            System.Threading.Thread.Sleep(1500);

            Process[] processes = Process.GetProcessesByName("WindowsTerminal");
            IntPtr hwnd = processes[0].MainWindowHandle;

            ImportLib.ShowWindow(hwnd, 3);
            ImportLib.EnableWindow(hwnd, true);
            int style = ImportLib.GetWindowLong(hwnd, ImportLib.GWL_STYLE);
            style = style & ~((int)ImportLib.WS_CAPTION) & ~((int)ImportLib.WS_BORDER);
            style |= ((int)ImportLib.WS_CHILD);
            ImportLib.SetWindowLong(hwnd, ImportLib.GWL_STYLE, style);
            ImportLib.SetParent(hwnd, hwndParent.Handle);

            return new HandleRef(this, hwnd);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Console.WriteLine("CLOSE...\n");
        }


    }


}
