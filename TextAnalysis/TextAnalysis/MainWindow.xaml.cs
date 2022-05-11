using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace TextAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string TextForAnalysis = String.Empty;
        private string filePath = String.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearTextAreaButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextForAnalysis == String.Empty)
            {
                InputTextBox.Text = String.Empty;
            }
        }

        private void CancelFileButton_Click(object sender, RoutedEventArgs e)
        {
            if(TextForAnalysis == String.Empty)
            {
                MessageBox.Show("Вы еще не прикрепили файл",
                "Предупреждение!",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }
            if(MessageBox.Show("Вы действительно хотите открепить файл?",
                "Подтверждение", 
                MessageBoxButton.OKCancel, 
                MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                ClearInfo();
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Присутствует ли текст или файл
            if (InputTextBox.Text != String.Empty)
            {
                if (TextForAnalysis == String.Empty)
                {
                    TextForAnalysis = InputTextBox.Text;
                }
            }
            else
            {
                MessageBox.Show("Вы должны сначала ввести текст или прикрепить файл!",
                "Внимание!",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }
            ResultWindow resultWindow = new ResultWindow(TextForAnalysis, filePath);
            resultWindow.Show();
            this.Hide();
            ClearInfo();
        }

        private void ClearInfo()
        {
            InputTextBox.Text = String.Empty;
            TextForAnalysis = String.Empty;
            InputTextBox.IsReadOnly = false;
            filePath = String.Empty;
        }

        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "Document";
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Текстовые документы (.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader reader = new(openFileDialog.FileName))
                {
                    TextForAnalysis = reader.ReadToEnd();
                }
                filePath = openFileDialog.FileName;
                InputTextBox.Text = "Вы прикрепили текстовый файл!\n Путь к файлу:\n" + openFileDialog.FileName;
                InputTextBox.IsReadOnly = true;
            }
        }
    }
}
