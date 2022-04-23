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
    public partial class ChangeAddCounterRuleParameters : Window
    {
        public string StartValue { get; set; }
        public string Steps { get; set; }
        public string NumberDigits { get; set; }
        public ChangeAddCounterRuleParameters()
        {
            InitializeComponent();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (startValueTextBox.Text == string.Empty || stepsTextBox.Text == string.Empty || numberDigitsTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill in the information enough", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } 
            if (!isNumber(startValueTextBox.Text) || !isNumber(stepsTextBox.Text) || !isNumber(numberDigitsTextBox.Text))
            {
                MessageBox.Show("The input must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }    
            StartValue = startValueTextBox.Text;
            Steps = stepsTextBox.Text;
            NumberDigits = numberDigitsTextBox.Text;
            DialogResult = true;
        }
        private bool isNumber(string s)
        {
            foreach(char c in s)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
