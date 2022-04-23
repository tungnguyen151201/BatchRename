using IRenameLib;
using System;
using System.Collections.Generic;

namespace ReplaceRule
{
    public class ReplaceRule : IRenameRule
    {
        public List<string> Needles { get; set; }
        public string Replacer { get; set; }

        public ReplaceRule(List<string> needles, string replacer)
        {
            this.Needles = needles;
            this.Replacer = replacer;
        }

        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            string extension = original.Substring(index, original.Length - index);

            foreach (var needle in Needles)
            {
                filename = filename.Replace(needle, Replacer);               
            }

            return $"{filename}{extension}";
        }
    }
}
