using IRenameLib;
using System;

namespace ChangeExtensionRule
{
    public class ChangeExtensionRule : IRenameRule
    {
        public string Replacer { get; set; }
        public ChangeExtensionRule(string replacer)
        {           
            this.Replacer = replacer;
        }
        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            return $"{filename}.{Replacer}";
        }
    }
}
