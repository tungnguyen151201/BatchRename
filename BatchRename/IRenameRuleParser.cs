using IRenameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    interface IRenameRuleParser
    {
        IRenameRule Parse(string line, Type type);
        virtual List<string> GetParameters() { return null; }
    }
    //Replace "a" => "b"
    class ReplaceRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "Replace";      
        public IRenameRule Parse(string line, Type type)
        {
            string[] tokens = line.Split(new string[] { "Replace " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { " => " }, StringSplitOptions.None);
            
            List<string> needles = new List<string>();
            needles.Add(parts[0].Substring(1, parts[0].Length - 2));

            string replacement = parts[1].Substring(1, parts[1].Length - 2);

            IRenameRule rule = Activator.CreateInstance(type, needles, replacement) as IRenameRule;
            return rule;
        }
    }
    //AddPrefix "1"
    class AddPrefixRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "AddPrefix";
        public IRenameRule Parse(string line, Type type)
        {
            string[] tokens = line.Split(new string[] { "AddPrefix " }, StringSplitOptions.None);

            string prefix = tokens[1].Replace("\"", "");
            IRenameRule rule = Activator.CreateInstance(type, prefix) as IRenameRule;

            return rule;
        }
    }
    //AddSuffix "1"
    class AddSuffixRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "AddSuffix";
        public IRenameRule Parse(string line, Type type)
        {
            string[] tokens = line.Split(new string[] { "AddSuffix " }, StringSplitOptions.None);

            string suffix = tokens[1].Replace("\"", "");
            IRenameRule rule = Activator.CreateInstance(type, suffix) as IRenameRule;

            return rule;
        }
    }
    //AddCounter (Start Value: 1, Steps: 2, Numbers of digits: 3)
    class AddCounterRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "AddCounter";
        public static string StartValue { get; set; }
        public static string Steps { get; set; }
        public static string NumberDigits { get; set; }
        public IRenameRule Parse(string line, Type type)
        {
            string[] tokens = line.Split(new string[] { "AddCounter " }, StringSplitOptions.None);
            //Remove "(", ")"
            tokens[1] = tokens[1].Replace("(", "");
            tokens[1] = tokens[1].Replace(")", "");

            string[] parts = tokens[1].Split(", ");
            string[] values0 = parts[0].Split(": ");
            string[] values1 = parts[1].Split(": ");
            string[] values2 = parts[2].Split(": ");

            StartValue = values0[1];
            Steps = values1[1];
            NumberDigits = values2[1];

            IRenameRule rule = Activator.CreateInstance(type, new List<string>(), StartValue, Steps, NumberDigits) as IRenameRule;

            return rule;
        }
        public List<string> GetParameters()
        {
            return new List<string> { StartValue, Steps, NumberDigits };
        }
    }
    //ChangeExtension => "123"
    class ChangeExtensionRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "ChangeExtension";
        public IRenameRule Parse(string line, Type type)
        {
            string[] tokens = line.Split(new string[] { "ChangeExtension => " }, StringSplitOptions.None);
            string extension = tokens[1].Replace("\"", "");
            
            IRenameRule rule = Activator.CreateInstance(type, extension) as IRenameRule;

            return rule;
        }
    }
    //ToLowerCase
    class ToLowerCaseRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "ToLowerCase";

        public IRenameRule Parse(string line, Type type)
        {
            IRenameRule rule = Activator.CreateInstance(type) as IRenameRule;

            return rule;
        }
    }
    //ToPascalCase
    class ToPascalCaseRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "ToPascalCase";

        public IRenameRule Parse(string line, Type type)
        {
            IRenameRule rule = Activator.CreateInstance(type, new List<string> { ".", ",", " ", "-", "_" }) as IRenameRule;

            return rule;
        }
    }
    //RemoveSpaceFromBeginningAndEnding
    class RemoveSpaceFromBeginningAndEndingRuleParser : IRenameRuleParser
    {
        public static string MagicWord = "RemoveSpaceFromBeginningAndEnding";

        public IRenameRule Parse(string line, Type type)
        {
            IRenameRule rule = Activator.CreateInstance(type) as IRenameRule;

            return rule;
        }
    }
    class RuleParserFactory
    {
        private Dictionary<string, IRenameRuleParser> _prototypes = null;
        public RuleParserFactory()
        {
            _prototypes = new Dictionary<string, IRenameRuleParser>() 
            {
                {ReplaceRuleParser.MagicWord, new ReplaceRuleParser()},
                {AddPrefixRuleParser.MagicWord, new AddPrefixRuleParser()},
                {AddSuffixRuleParser.MagicWord, new AddSuffixRuleParser()},
                {AddCounterRuleParser.MagicWord, new AddCounterRuleParser()},
                {ChangeExtensionRuleParser.MagicWord, new ChangeExtensionRuleParser()},
                {ToLowerCaseRuleParser.MagicWord, new ToLowerCaseRuleParser()},
                {ToPascalCaseRuleParser.MagicWord, new ToPascalCaseRuleParser()},
                {RemoveSpaceFromBeginningAndEndingRuleParser.MagicWord, new RemoveSpaceFromBeginningAndEndingRuleParser()}
            };
        }
        public IRenameRuleParser Create(string choice)
        {
            IRenameRuleParser parser = _prototypes[choice];
            return parser;
        }
    }
}
