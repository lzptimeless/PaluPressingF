using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PaluPressingF.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PaluPressingF.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PaluPressingF.Controls;assembly=PaluPressingF.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:HotKeyTextBox/>
    ///
    /// </summary>
    public class PaluHotKeyTextBox : TextBox
    {
        static PaluHotKeyTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaluHotKeyTextBox), new FrameworkPropertyMetadata(typeof(PaluHotKeyTextBox)));
        }

        public PaluHotKeyTextBox()
        {
            Loaded += PaluHotKeyTextBox_Loaded;
            TextChanged += PaluHotKeyTextBox_TextChanged;
        }

        private void PaluHotKeyTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            CalculateTextProperty();
        }

        private void PaluHotKeyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderVisibility = string.IsNullOrEmpty(base.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
           DependencyProperty.Register("Placeholder", typeof(string), typeof(PaluHotKeyTextBox), new PropertyMetadata(null, null));

        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register("PlaceholderVisibility", typeof(Visibility), typeof(PaluHotKeyTextBox), new PropertyMetadata(Visibility.Visible, null));

        public ModifierKeys ModifierKeys
        {
            get { return (ModifierKeys)GetValue(ModifierKeysProperty); }
            set { SetValue(ModifierKeysProperty, value); }
        }

        public static readonly DependencyProperty ModifierKeysProperty =
            DependencyProperty.Register("ModifierKeys", typeof(ModifierKeys), typeof(PaluHotKeyTextBox), new PropertyMetadata(ModifierKeys.Control, OnModifierKeyChanged));

        private static void OnModifierKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PaluHotKeyTextBox)?.CalculateTextProperty();
        }

        public Key HotKey
        {
            get { return (Key)GetValue(HotKeyProperty); }
            set { SetValue(HotKeyProperty, value); }
        }

        public static readonly DependencyProperty HotKeyProperty =
            DependencyProperty.Register("HotKey", typeof(Key), typeof(PaluHotKeyTextBox), new PropertyMetadata(Key.F1, OnHotKeyChanged));

        private static void OnHotKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PaluHotKeyTextBox)?.CalculateTextProperty();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;

            if (e.Key == Key.System)
            {
                // Handle Alt key separately
                ModifierKeys = ModifierKeys.Alt;
                HotKey = e.SystemKey;
            }
            else
            {
                ModifierKeys = Keyboard.Modifiers;
                HotKey = e.Key;
            }
        }

        private void CalculateTextProperty()
        {
            Text = GetHotKeyString();
        }

        private string GetHotKeyString()
        {
            string hotKeyString = string.Empty;

            if (ModifierKeys.HasFlag(ModifierKeys.Control))
                hotKeyString += "Ctrl + ";
            if (ModifierKeys.HasFlag(ModifierKeys.Shift))
                hotKeyString += "Shift + ";
            if (ModifierKeys.HasFlag(ModifierKeys.Alt))
                hotKeyString += "Alt + ";

            hotKeyString += HotKey.ToString();

            return hotKeyString;
        }
    }
}
