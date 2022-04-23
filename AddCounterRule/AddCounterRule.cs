using IRenameLib;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AddCounterRule
{
    public class AddCounterRule : IRenameRule
    {
        public List<string> Files { get; set; }
        public int StartValue { get; set; }
        public int Steps { get; set; }
        public int NumberDigits { get; set; }
        public AddCounterRule(List<string> files, string startValue, string steps, string numberDigits)
        {
            Files = files;
            StartValue = int.Parse(startValue);
            Steps = int.Parse(steps);
            NumberDigits = int.Parse(numberDigits);
        }
        private string Padding(int n)
        {
            string result = string.Empty;            
            if (n < 10)
            {
                for (int i = 0; i < NumberDigits - 1; i++)
                {
                    result += "0";
                }
                return $"{result}{n}";
            }
            int digit = 0;
            int temp = n;
            while (temp > 0)
            {
                temp /= 10;
                digit++;
            }
            if (digit >= NumberDigits) return $"{n}";
            while (digit < NumberDigits)
            {
                result += "0";
                digit++;
            }
            return $"{result}{n}";
        }
        public string Rename(string original, string oldFileName)
        {
            string result = string.Empty;
            int count = StartValue;
            for (int i = 0; i < Files.Count; i++)
            {
                if (Files[i] == oldFileName)
                {
                    int index = original.LastIndexOf('.');
                    string filename = original.Substring(0, index);
                    string extension = original.Substring(index, original.Length - index);
                    result = $"{filename}{Padding(count)}{extension}";
                }
                count += Steps;
            }
            return result;
        }

        public string Rename(string original)
        {
            throw new NotImplementedException();
        }
    }
}
