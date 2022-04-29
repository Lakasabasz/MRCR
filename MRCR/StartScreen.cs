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
using MRCR.StartScreen_UC;

namespace MRCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        private PlayerModeSelect _playerModeSelect;
        private OEDWorld? _oedWorld;
        private CreateNewWorld? _createNewWorld;
        private Stack<UserControl> _lastControll;
        public StartScreen()
        {
            InitializeComponent();
            _lastControll = new Stack<UserControl>();
            
            _playerModeSelect = new PlayerModeSelect();
            WindowContent.Content = _playerModeSelect;
            _playerModeSelect.SingleplayerChoose += Singleplayer_Click;
            _playerModeSelect.MultiplayerChoose += Multiplayer_Click;
        }

        private void Singleplayer_Click(object sender, RoutedEventArgs e)
        {
            if(_oedWorld == null)
            {
                _oedWorld = new OEDWorld(this);
                _oedWorld.OEDCreateWorld += CreateWorld_Click;
                _oedWorld.OEDBack += Back_Click;
            }

            _lastControll.Push(_playerModeSelect);
            WindowContent.Content = _oedWorld;
        }

        private void CreateWorld_Click(object sender, RoutedEventArgs e)
        {
            if(_createNewWorld == null)
            {
                _createNewWorld = new CreateNewWorld();
                _createNewWorld.CancelWorldCreationEvent += Back_Click;
            }
            if(_oedWorld == null)
            {
                MessageBox.Show("Błąd stosu kontrolek. Program zostanie wyłączony", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("Illegal user control history. CreateNewWorld without OEDWorld");
            }
            _lastControll.Push(_oedWorld);
            WindowContent.Content = _createNewWorld;
        }

        private void Multiplayer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Multiplayer");
        }
        
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (_lastControll.Count > 0)
            {
                WindowContent.Content = _lastControll.Pop();
            }
        }
    }
}