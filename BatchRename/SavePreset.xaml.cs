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

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for SavePreset.xaml
    /// </summary>
    public partial class SavePreset : Window
    {
        public string PresetName { get; set; }
        public SavePreset()
        {
            InitializeComponent();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (presetNameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill in the information enough", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PresetName = presetNameTextBox.Text;
            DialogResult = true;
        }
    }
}
