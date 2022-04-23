using IRenameLib;
using System;

namespace RemoveSpaceFromBeginningAndEndingRule
{
    public class RemoveSpaceFromBeginningAndEndingRule : IRenameRule
    {
        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            string extension = original.Substring(index, original.Length - index);
            if (filename[0] == ' ')
            {
                for (int i = 0; i < filename.Length; i++)
                {
                    if (filename[i] != ' ')
                    {
                        filename = filename.Remove(0, i);
                        break;
                    }
                }
            }
            if (filename[filename.Length - 1] == ' ')
            {
                for (int i = filename.Length - 1; i >= 0; i--)
                {
                    if (filename[i] != ' ')
                    {
                        filename = filename.Remove(i + 1, filename.Length - i - 1);
                        break;
                    }
                }
            }
            return $"{filename}{extension}";
        }
    }
}
