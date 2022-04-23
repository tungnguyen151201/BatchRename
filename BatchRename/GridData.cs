using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    class GridData : INotifyPropertyChanged
    {
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
