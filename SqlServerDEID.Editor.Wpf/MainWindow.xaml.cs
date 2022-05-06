using SqlServerDEID.Common.Globals.Models;
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

namespace SqlServerDEID.Editor.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var database = Database.LoadFile(@"C:\Users\tim.cartwright.VGNET\source\repos\SqlServerDEID\SqlServerDEID\TestFiles\Transform.xml");

            CollectionViewSource itemCollectionViewSource;
            itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source = database.Tables;
        }
    }
}
