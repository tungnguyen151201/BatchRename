using IRenameLib;
using System;

namespace ToLowerCaseRule
{
    public class ToLowerCaseRule : IRenameRule
    {
        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            string extension = original.Substring(index, original.Length - index);
            return filename.ToLower() + extension;
        }
    }
}
