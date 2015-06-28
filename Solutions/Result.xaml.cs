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

namespace Solutions
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window
    {
        public Result()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var gridView = new GridView();
            ListView.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Вершина",
                DisplayMemberBinding = new Binding("Id")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Конкретная реализация",
                DisplayMemberBinding = new Binding("Name")
            });


            ListView.Items.Add(new { Id = "Электротележка", Name = "СЭГЗ ЕТ2012" });
            ListView.Items.Add(new { Id = "Манипуляторы", Name = "KUKA KR 360-2" });
            ListView.Items.Add(new { Id = "Рельсовые тележки", Name = "\"Крандеталь\" Тележка рельсовая передаточная Г/П 3-5Т" });
        }
    }
}
