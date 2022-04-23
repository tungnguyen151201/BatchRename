using IRenameLib;
using System;

namespace AddPrefixRule
{
    public class AddPrefixRule : IRenameRule
    {
        public string Prefix { get; set; }
        public AddPrefixRule(string prefix)
        {
            Prefix = prefix;
        }

        public string Rename(string original)
        {
            return $"{Prefix}{original}";
        }
    }
}
