using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public MainWindow()
        {
            InitializeComponent();
        }


        static void StartPressF()
        {
            string windowTitle = "Pal"; // 替换为目标窗口的标题

            IntPtr windowHandle = IntPtr.Zero;
            var processes = Process.GetProcessesByName("Palworld-Win64-Shipping");
            if (processes.Length > 0)
            {
                windowHandle = processes[0].MainWindowHandle;
                foreach (var item in processes)
                {
                    item.Dispose();
                }
            }

            if (windowHandle == IntPtr.Zero)
            {
                MessageBox.Show("窗口未找到", "错误");
                return;
            }

            //while (true)
            //{
            // 模拟按下键盘A键
            const int VK_F = 0x46;
            //const int KEYEVENTF_KEYDOWN = 0x0;

            //Console.WriteLine("正在发送按键消息...");

            // 使用PostMessage发送按键消息
            PostMessage(windowHandle, WM_KEYDOWN, VK_F, 0);

            // 可以添加一些延迟，模拟按键的持续时间
            //Thread.Sleep(100);
            //}
            // 发送松开按键的消息
            //PostMessage(windowHandle, WM_KEYUP, VK_F, 0);

            //Console.WriteLine("按键消息已发送");

            // 如果你愿意，也可以使用SendMessage函数
            // SendMessage(windowHandle, WM_KEYDOWN, VK_A, 0);
            // Thread.Sleep(100);
            // SendMessage(windowHandle, WM_KEYUP, VK_A, 0);

            //Console.ReadLine();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartPressF();
        }
    }
}