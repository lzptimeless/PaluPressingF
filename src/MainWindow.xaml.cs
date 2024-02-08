using PaluPressingF.HotKeys;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
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

namespace PaluPressingF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int VK_F = 0x46;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private readonly HotkeyService _hotKeyService;
        private HwndSource? _hwndSource;

        public MainWindow()
        {
            InitializeComponent();

            _hotKeyService = new HotkeyService();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;

            _hotKeyService.Initialize(handle);
            SetHotKey(HotKeyTextBox.ModifierKeys, HotKeyTextBox.HotKey);

            _hwndSource = HwndSource.FromHwnd(handle);
            if (_hwndSource != null)
            {
                _hwndSource.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            _hotKeyService.ProcessHotkeyMessage(msg, wParam, lParam);

            return IntPtr.Zero;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SetHotKey(HotKeyTextBox.ModifierKeys, HotKeyTextBox.HotKey);
        }

        private void PressingFButton_Click(object sender, RoutedEventArgs e)
        {
            PressingF();
        }

        private void PressFButton_Click(object sender, RoutedEventArgs e)
        {
            PressF();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _hotKeyService.UnregisterAll();

            base.OnClosing(e);
        }

        private void SetHotKey(ModifierKeys modifier, Key hotKey)
        {
            _hotKeyService.UnregisterAll();

            var vk = HotKeys.KeyConverter.VirtualKeyFromKey(hotKey);
            if (vk == 0)
            {
                MessageBox.Show("The HotKey is invalid!", "ERROR");
                return;
            }

            try
            {
                _hotKeyService.Register(modifier, vk, (sender, args) => PressingF());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to register the HotKey.\r\n{ex.Message}","ERROR");
                return;
            }
        }

        static void PressingF()
        {
            if (!TryFindPaluWindow(out var windowHandle))
            {
                MessageBox.Show("Cann't find the Palworld window!", "ERROR");
                return;
            }

            PostMessage(windowHandle, WM_KEYDOWN, VK_F, 0);
        }

        static async void PressF()
        {
            if (!TryFindPaluWindow(out var windowHandle))
            {
                MessageBox.Show("Cann't find the Palworld window!", "ERROR");
                return;
            }

            PostMessage(windowHandle, WM_KEYDOWN, VK_F, 0);
            await Task.Delay(100);
            PostMessage(windowHandle, WM_KEYUP, VK_F, 0);
        }

        static bool TryFindPaluWindow(out IntPtr windowHandle)
        {
            windowHandle = IntPtr.Zero;
            var processes = Process.GetProcessesByName("Palworld-Win64-Shipping");
            if (processes.Length > 0)
            {
                windowHandle = processes[0].MainWindowHandle;
                foreach (var item in processes)
                {
                    item.Dispose();
                }
            }

            return windowHandle != IntPtr.Zero;
        }
    }
}