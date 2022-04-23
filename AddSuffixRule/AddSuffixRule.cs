using IRenameLib;
using System;

namespace AddPrefixRule
{
    public class AddSuffixRule : IRenameRule
    {
        public string Suffix { get; set; }
        public AddSuffixRule(string suffix)
        {
            Suffix = suffix;
        }

        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            string extension = original.Substring(index, original.Length - index);
            return $"{filename}{Suffix}{extension}";
        }
    }
}
