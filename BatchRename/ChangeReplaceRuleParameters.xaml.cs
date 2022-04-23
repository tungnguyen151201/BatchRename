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
    /// Interaction logic for ChangeReplaceRuleParameters.xaml
    /// </summary>
    public partial class ChangeReplaceRuleParameters : Window
    {
        public string Needles { get; set; }
        public string Replacer { get; set; }
        public ChangeReplaceRuleParameters()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (needlesTextBox.Text == string.Empty)
            {
                MessageBox.Show("Needle could not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }    
            Needles = needlesTextBox.Text;
            Replacer = replacerTextBox.Text;
            DialogResult = true;
        }
    }
}
