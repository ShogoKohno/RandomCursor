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
using RandomCursor.Services;
using RandomCursor.Config;
using System.IO;

namespace RandomCursor.GUI
{
   public partial class MainWindow : Window
    {
        private AppConfig _config;
        public MainWindow()
        {
            InitializeComponent();

            _config =
                ConfigManager.Load();

            UpdateStartupStatus();


            PrefixBox.Text =
                _config.Prefix;


            AvoidPreviousCheck.IsChecked =
                _config.AvoidPrevious;
            WriteLogCheck.IsChecked =
    _config.WriteLog;

            foreach (var scheme in
    SchemeManager.GetAll(_config))
            {
                SchemeListBox.Items.Add(
                    scheme.Name);
            }


            MaxLogSizeBox.Text =
                _config.MaxLogSizeMB.ToString();
        }
        private void SaveButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            _config.Prefix =
                PrefixBox.Text;


            _config.AvoidPrevious =
                AvoidPreviousCheck.IsChecked
                ?? false;

            _config.WriteLog =
    WriteLogCheck.IsChecked
    ?? false;


            if (int.TryParse(
                    MaxLogSizeBox.Text,
                    out int size))
            {
                _config.MaxLogSizeMB =
                    size;
            }


            ConfigManager.Save(
                _config);


            MessageBox.Show(
                "設定を保存しました。");
        }
        private void ApplyButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (SchemeListBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "スキームを選択してください。");

                return;
            }


            string name =
                SchemeListBox.SelectedItem
                .ToString()!;


            var scheme =
                SchemeManager.GetByName(
                    _config,
                    name);


            if (scheme == null)
            {
                MessageBox.Show(
                    "スキームが見つかりません。");

                return;
            }


            CursorManager.Apply(
                scheme);


            MessageBox.Show(
                $"適用しました: {scheme.Name}");
        }
        private void UpdateStartupStatus()
        {
            StartupStatusText.Text =
                StartupManager.IsEnabled()
                    ? "状態: 有効"
                    : "状態: 無効";
        }
        private void EnableStartupButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            StartupManager.Enable();


            UpdateStartupStatus();


            MessageBox.Show(
                "スタートアップを有効化しました。");
        }
        private void DisableStartupButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            StartupManager.Disable();


            UpdateStartupStatus();


            MessageBox.Show(
                "スタートアップを無効化しました。");
        }
    }
}