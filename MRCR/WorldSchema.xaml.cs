using System.Windows;

namespace MRCR
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class WorldSchemaChoice : Window
    {
        private OEDWorld _ccWorldSelect;
        private CreateNewWorld _ccCreateNewWorld;
        public WorldSchemaChoice()
        {
            InitializeComponent();
            _ccWorldSelect = new OEDWorld(this);
            _ccCreateNewWorld = new CreateNewWorld();
            ContentControl.Content = _ccWorldSelect;
            _ccWorldSelect.OEDCreateWorldEvent += new RoutedEventHandler(ContentControl_OnOEDCreateWorldEvent);
            _ccCreateNewWorld.CancelWorldCreationEvent += new RoutedEventHandler(ContentControl_OnCancelWorldCreationEvent);
        }

        private void ContentControl_OnOEDCreateWorldEvent(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = _ccCreateNewWorld;
        }
        
        private void ContentControl_OnCancelWorldCreationEvent(object sender, RoutedEventArgs e)
        {
            _ccWorldSelect.ReloadWorldList();
            ContentControl.Content = _ccWorldSelect;
        }
    }
}
