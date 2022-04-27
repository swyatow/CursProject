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
        public ResultWindow(string text)
        {
            InitializeComponent();
            this.text = text.ToLower();
        }

        private void ShowTextResultButton_Click(object sender, RoutedEventArgs e)
        {
            TextResult textResult = new TextResult(symbols,SymbolsCount);
            frame.Content = textResult;
        }

        private void ShowBarDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            CircleDiagramPage circleDiagramPage = new CircleDiagramPage(symbols, SymbolsCount);
            frame.Content = circleDiagramPage;
        }

        private void ShowCircleDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            //BarDiagramPage barDiagramPage = new BarDiagramPage(symbols, SymbolsCount);
            //frame.Content = barDiagramPage;
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
            symbols.OrderBy(pair => pair.Key);
            AllCharsCountLabel.Content = "Общее количество: " + SymbolsCount + " символов. Пробельных символов: " + whiteSpaceCount + $" ({Math.Round((float)whiteSpaceCount/(float)SymbolsCount*100, 3)}%)";
            TextResult textResult = new TextResult(symbols, SymbolsCount);
            frame.Content = textResult;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}
