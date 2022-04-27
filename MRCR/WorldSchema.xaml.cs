using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MRCR
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class WorldSchemaChoice : Window
    {
        private string? _selected = null;
        public WorldSchemaChoice()
        {
            InitializeComponent();

            List<WorldSchema> worldSchemas = new List<WorldSchema>();
            try{
                string[] worlds = Directory.GetFiles(@"Worlds\");
                foreach (string world in worlds)
                {
                    string name = Path.GetFileNameWithoutExtension(world);
                    WorldSchema ws = new WorldSchema() { Name = name };
                    worldSchemas.Add(ws);
                }
            }
            catch(IOException E)
            {
                Console.WriteLine("[WARNING] Worlds folder not found");
                Directory.CreateDirectory("Worlds");
            }

            LbWorldsList.ItemsSource = worldSchemas;
        }

        private void LbWorldsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LbWorldsList.SelectedIndex != -1)
            {
                BtDelete.IsEnabled = true;
                BtEdit.IsEnabled = true;
                BtOpen.IsEnabled = true;
            }
            else
            {
                BtDelete.IsEnabled = false;
                BtEdit.IsEnabled = false;
                BtOpen.IsEnabled = false;
            }
        }

        private void BtOpen_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtNew_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtEdit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtDelete_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class WorldSchema
    {
        public string Name { get; set; }
    }
}
