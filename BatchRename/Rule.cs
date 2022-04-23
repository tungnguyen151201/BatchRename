using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    class Rule : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
