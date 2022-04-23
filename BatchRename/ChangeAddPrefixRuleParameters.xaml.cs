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
    /// Interaction logic for ChangeAddPrefixRuleParameters.xaml
    /// </summary>
    public partial class ChangeAddPrefixRuleParameters : Window
    {
        public string Prefix { get; set; }
        public ChangeAddPrefixRuleParameters()
        {
            InitializeComponent();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (prefixTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill in the information enough", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Prefix = prefixTextBox.Text;
            DialogResult = true;
        }
    }
}
