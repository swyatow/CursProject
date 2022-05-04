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
using Charts;

namespace TextAnalysis.ResultPages
{
    /// <summary>
    /// Логика взаимодействия для CircleDiagramPage.xaml
    /// </summary>
    public partial class CircleDiagramPage : Page
    {
        Chart chart = new PieChart();
        Dictionary<char, int> symbols = new Dictionary<char, int>();

        public CircleDiagramPage(Dictionary<char, int> symbols, int symbolsCount)
        {
            InitializeComponent();
            this.symbols = symbols;
            //Добавляем диаграмму на грид
            PaintingSurface.Children.Add(chart.ChartBg);
        }

        private void PaintingSurface_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Принудительно обновляем размеры контейнера для диаграммы
            PaintingSurface.UpdateLayout();
            PaintingSurface.ToolTip = "Нажмите, чтобы перерисовать график, после изменения размера окна";

            chart.ChartBg.Children.Clear();
            chart.PathList.Clear();

            int counter = 0;
            foreach (var symbol in symbols)
            {
                chart.AddValue(symbol.Value, $"{symbol.Key} - {symbol.Value}");
                counter++;
                if (counter == 10) break;
            }
        }
    }
}
