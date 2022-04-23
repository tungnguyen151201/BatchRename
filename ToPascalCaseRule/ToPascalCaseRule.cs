using IRenameLib;
using System;
using System.Collections.Generic;

namespace ToPascalCaseRule
{
    public class ToPascalCaseRule : IRenameRule
    {
        public List<string> Punctuations { get; set; }
        public ToPascalCaseRule(List<string> punctuations)
        {
            Punctuations = punctuations;
        }
        public string Rename(string original)
        {
            int index = original.LastIndexOf('.');
            string filename = original.Substring(0, index);
            string extension = original.Substring(index, original.Length - index);

            foreach (var punctuation in Punctuations)
            {
                string result = "";
                string[] words = filename.Split(punctuation);
                if (words.Length > 1)
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] != string.Empty)
                        {
                            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                            if (i == words.Length - 1) result += words[i];
                            else result += words[i] + punctuation;
                        }
                    }
                    filename = result;
                }
            }
            return $"{filename}{extension}";
        }
    }
}
