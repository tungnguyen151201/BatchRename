using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    class Preset : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
