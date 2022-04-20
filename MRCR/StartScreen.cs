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

namespace MRCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        private void Singleplayer_Click(object sender, RoutedEventArgs e)
        {
            WorldSchemaChoice wsc = new WorldSchemaChoice();
            wsc.Owner = this;
            this.Hide();
            wsc.ShowDialog();
            this.Show();
        }

        private void Multiplayer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Multiplayer");
        }
    }
}