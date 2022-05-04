using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using TextAnalysis.ResultPages;

namespace TextAnalysis
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public int SymbolsCount;
        public Dictionary<char, int> symbols = new Dictionary<char, int>();
        private string text;
        private string filePath;

        public ResultWindow(string text, string filePath)
        {
            InitializeComponent();
            this.text = text.ToLower();
            this.filePath = filePath;
            if(filePath != String.Empty)
            {
                this.Title = "Отчет об анализе текста из файла: " + filePath;
            }
            else
                this.Title = "Отчет об анализе текста из текстового поля";
        }

        private void ShowTextResultButton_Click(object sender, RoutedEventArgs e)
        {
            TextResult textResult = new TextResult(symbols, SymbolsCount);
            frame.Content = textResult;
        }

        private void ShowBarDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            BarDiagramPage barDiagramPage = new BarDiagramPage(symbols, SymbolsCount);
            frame.Content = barDiagramPage;
        }

        private void ShowCircleDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            CircleDiagramPage circleDiagramPage = new CircleDiagramPage(symbols, SymbolsCount);
            frame.Content = circleDiagramPage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int whiteSpaceCount= 0;
            foreach (var symbol in text)
            {
                SymbolsCount++;
                if (symbols.ContainsKey(symbol))
                {
                    symbols[symbol]++;
                }
                else
                {
                    if (char.IsWhiteSpace(symbol)) whiteSpaceCount++;
                    else symbols.Add(symbol, 1);
                }
            }
            symbols = symbols.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            AllCharsCountLabel.Content = "Общее количество: " 
                + SymbolsCount 
                + " символов. Пробельных символов: " 
                + whiteSpaceCount + $" ({Math.Round((float)whiteSpaceCount/(float)SymbolsCount*100, 3)}%)";
            CreateReport();
            TextResult textResult = new TextResult(symbols, SymbolsCount);
            frame.Content = textResult;
        }

        private void CreateReport()
        {
            DateTime today = DateTime.Now;
            string reportName = $"report_{today.Day}.{today.Month}.{today.Year}_{today.Hour}.{today.Minute}.{today.Second}";
            string reportPath = @$"Reports\{reportName}.txt";

            using (FileStream writer = File.Create(reportPath))
            {
                string reportText = $"{this.Title}\n";
                reportText += $"{AllCharsCountLabel.Content}\n";
                reportText += "-------------------------------------------------------\n";
                int counter = 1;
                foreach(var symbol in symbols)
                {
                    reportText += $"{counter}) {symbol.Key} - {symbol.Value} - {Math.Round((float)symbol.Value / (float)SymbolsCount * 100, 3)}%\n";
                    counter++;
                }

                byte[] reportTextByteArray = new UTF8Encoding(true).GetBytes(reportText);
                writer.Write(reportTextByteArray, 0,reportTextByteArray.Length);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}
