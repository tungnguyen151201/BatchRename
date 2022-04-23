using IRenameLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace BatchRename
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        BindingList<GridData> list = new BindingList<GridData>();
        List<IRenameRule> plugins = new List<IRenameRule>();
        BindingList<Rule> rulesList = new BindingList<Rule>();
        BindingList<Rule> rulesComboBoxSource = new BindingList<Rule>();
        List<GridData> fileRename = new List<GridData>();
        List<string> _fileRename = new List<string>();
        List<string> addCounterRuleParameters = new List<string>();
        BindingList<Preset> presets = new BindingList<Preset>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            var infos = new DirectoryInfo(folder).GetFiles("*.dll");

            foreach (var fi in infos)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(fi.FullName));
                var types = assembly.GetTypes();               

                foreach(var type in types)
                {
                    if (type.IsClass && typeof(IRenameRule).IsAssignableFrom(type))
                    {
                        rulesComboBoxSource.Add(new Rule { Name = type.Name, Type = type });
                    }
                }
            }

            //var presetFiles = new DirectoryInfo(folder + "/Preset").GetFiles("*.txt");
            foreach (var file in presetFiles)
            {
                presets.Add(new Preset { Name = file.Name , Path = file.FullName});                
            }

            rulesComboBox.ItemsSource = rulesComboBoxSource;
            rulesListView.ItemsSource = rulesList;
            presetsCombobox.ItemsSource = presets;
        }
        private void UpdateData()
        {
            applyComboBox.Items.Clear();
            if (applyComboBox.Items.Count == 0 && list.Count > 0)
            {
                applyComboBox.Items.Add("All");
            }
            FilesGrid.ItemsSource = list;           
            foreach (var item in list)
            {
                applyComboBox.Items.Add(item.FileName);
            }
            applyComboBox.SelectedIndex = 0;
            UpdateNewFileName();
        }
        private bool isExisted(GridData row)
        {
            foreach (var item in list)
            {
                if (item.Path == row.Path) return true;
            }
            return false;
        }
        private void AddNewFile(string fileName, string path)
        {
            GridData gridData = new GridData()
            {
                FileName = fileName,
                Path = path,
            };
            if (!isExisted(gridData))
            {
                list.Add(gridData);
                UpdateData();
            }
            else
            {
                MessageBox.Show("This file is already existed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    AddNewFile(Path.GetFileName(filename), filename);
                }
            }
        }

        private void btnRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (FilesGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a file to remove", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                list.RemoveAt(FilesGrid.SelectedIndex);
                UpdateData();
            }
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            UpdateData();
        }
        private void AddNewRule()
        {
            var selection = (Rule)rulesComboBox.SelectedItem;
            if (selection == null) return;
            switch (selection.Name)
            {
                case "ReplaceRule":
                    var window = new ChangeReplaceRuleParameters();

                    if (window.ShowDialog() == true)
                    {                   
                        plugins.Add(Activator.CreateInstance(selection.Type, new List<string>() { window.Needles }, window.Replacer) as IRenameRule);
                        rulesList.Add(new Rule 
                        { 
                            Name = "Replace \"" + window.Needles + "\" => \"" + window.Replacer + "\"", 
                            Type = selection.Type 
                        });
                    }
                    break;
                case "AddPrefixRule":
                case "AddSuffixRule":
                    var window2 = new ChangeAddPrefixRuleParameters();

                    if (window2.ShowDialog() == true)
                    {
                        plugins.Add(Activator.CreateInstance(selection.Type, window2.Prefix) as IRenameRule);
                        rulesList.Add(new Rule 
                        {
                            Name = "Add" + (selection.Name == "AddPrefixRule" ? "Prefix \"" : "Suffix \"") + window2.Prefix + "\"",
                            Type = selection.Type 
                        });
                    }
                    break;
                case "ToLowerCaseRule":
                    plugins.Add(Activator.CreateInstance(selection.Type) as IRenameRule);
                    rulesList.Add(new Rule
                    {
                        Name = "ToLowerCase",
                        Type = selection.Type
                    });
                    break;
                case "ToPascalCaseRule":
                    plugins.Add(Activator.CreateInstance(selection.Type, new List<string> { ".", ",", " ", "-", "_" }) as IRenameRule);
                    rulesList.Add(new Rule
                    {
                        Name = "ToPascalCase",
                        Type = selection.Type
                    });
                    break;
                case "ChangeExtensionRule":
                    var window3 = new ChangeAddPrefixRuleParameters();

                    if (window3.ShowDialog() == true)
                    {
                        plugins.Add(Activator.CreateInstance(selection.Type, window3.Prefix) as IRenameRule);
                        rulesList.Add(new Rule
                        {
                            Name = "ChangeExtension => \"" + window3.Prefix + "\"",
                            Type = selection.Type
                        });
                    }
                    break;
                case "AddCounterRule":
                    var window4 = new ChangeAddCounterRuleParameters();

                    if (window4.ShowDialog() == true)
                    {
                        addCounterRuleParameters.Clear();
                        addCounterRuleParameters.Add(window4.StartValue);
                        addCounterRuleParameters.Add(window4.Steps);
                        addCounterRuleParameters.Add(window4.NumberDigits);
                        plugins.Add(Activator.CreateInstance(selection.Type, new List<string>(), window4.StartValue, window4.Steps, window4.NumberDigits) 
                            as IRenameRule);
                        rulesList.Add(new Rule
                        {
                            Name = $"AddCounter (Start Value: {window4.StartValue}, Steps: {window4.Steps}, Number of digits: {window4.NumberDigits})",
                            Type = selection.Type
                        });
                    }
                    break;
                case "RemoveSpaceFromBeginningAndEndingRule":
                    plugins.Add(Activator.CreateInstance(selection.Type) as IRenameRule);
                    rulesList.Add(new Rule
                    {
                        Name = "RemoveSpaceFromBeginningAndEnding",
                        Type = selection.Type
                    });
                    break;
            }
            UpdateNewFileName();
        }
        private void addRule_Click(object sender, RoutedEventArgs e)
        {
            AddNewRule();
        }
        private void ruleComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AddNewRule();
        }
        private void UpdateNewFileName()
        {
            if (FilesGrid.Items.Count == 0) return; 
            if (fileRename.Count != list.Count)
            {
                foreach(GridData file in list)
                {
                    file.NewFileName = file.FileName;
                }    
            }
            foreach(GridData file in fileRename)
            {
                string temp = file.FileName;
                if (plugins.Count == 0)
                {
                    file.NewFileName = file.FileName;                    
                }
                for (int i = 0; i < plugins.Count; i++)
                {
                    Type type = plugins[i].GetType();
                    if (type.Name == "AddCounterRule")
                    {
                        plugins[i] = Activator.CreateInstance(type, _fileRename, addCounterRuleParameters[0], addCounterRuleParameters[1], addCounterRuleParameters[2])
                            as IRenameRule;
                        file.NewFileName = plugins[i].Rename(temp, file.FileName);
                    }
                    else file.NewFileName = plugins[i].Rename(temp);
                    temp = file.NewFileName;
                }
            }                
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            var selection = (Rule)rulesListView.SelectedItem;  
            switch (selection.Type.Name)
            {
                case "ReplaceRule":
                    var window = new ChangeReplaceRuleParameters();

                    if (window.ShowDialog() == true)
                    {
                        plugins.RemoveAt(rulesListView.SelectedIndex);
                        rulesList.RemoveAt(rulesListView.SelectedIndex);
                        plugins.Add(Activator.CreateInstance(selection.Type, new List<string>() { window.Needles }, window.Replacer) as IRenameRule);
                        rulesList.Add(new Rule { Name = "Replace \"" + window.Needles + "\" => \"" + window.Replacer + "\"", Type = selection.Type });
                    }
                    break;
                case "AddPrefixRule":
                case "AddSuffixRule":
                    var window2 = new ChangeAddPrefixRuleParameters();

                    if (window2.ShowDialog() == true)
                    {
                        plugins.RemoveAt(rulesListView.SelectedIndex);
                        rulesList.RemoveAt(rulesListView.SelectedIndex);
                        plugins.Add(Activator.CreateInstance(selection.Type, window2.Prefix) as IRenameRule);
                        rulesList.Add(new Rule
                        {
                            Name = "Add" + (selection.Name == "AddPrefixRule" ? "Prefix \"" : "Suffix \"") + window2.Prefix + "\"",
                            Type = selection.Type
                        });
                    }
                    break;
                case "ChangeExtensionRule":
                    var window3 = new ChangeAddPrefixRuleParameters();

                    if (window3.ShowDialog() == true)
                    {
                        plugins.RemoveAt(rulesListView.SelectedIndex);
                        rulesList.RemoveAt(rulesListView.SelectedIndex);
                        plugins.Add(Activator.CreateInstance(selection.Type, window3.Prefix) as IRenameRule);
                        rulesList.Add(new Rule
                        {
                            Name = "ChangeExtension => \"" + window3.Prefix + "\"",
                            Type = selection.Type
                        });
                    }
                    break;
                case "AddCounterRule":
                    var window4 = new ChangeAddCounterRuleParameters();

                    if (window4.ShowDialog() == true)
                    {
                        plugins.RemoveAt(rulesListView.SelectedIndex);
                        rulesList.RemoveAt(rulesListView.SelectedIndex);
                        addCounterRuleParameters.Clear();
                        addCounterRuleParameters.Add(window4.StartValue);
                        addCounterRuleParameters.Add(window4.Steps);
                        addCounterRuleParameters.Add(window4.NumberDigits);
                        plugins.Add(Activator.CreateInstance(selection.Type, new List<string>(), window4.StartValue, window4.Steps, window4.NumberDigits)
                            as IRenameRule);
                        rulesList.Add(new Rule
                        {
                            Name = $"AddCounter (Start Value: {window4.StartValue}, Steps: {window4.Steps}, Number of digits: {window4.NumberDigits})",
                            Type = selection.Type
                        });
                    }
                    break;
            }
            UpdateNewFileName();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            plugins.Clear();
            rulesList.Clear();            
            UpdateNewFileName();
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            plugins.RemoveAt(rulesListView.SelectedIndex);
            rulesList.RemoveAt(rulesListView.SelectedIndex);
            UpdateNewFileName();
        }

        private void applyComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {         
            if (applyComboBox.SelectedIndex == 0 || applyComboBox.SelectedIndex == -1)
            {
                fileRename.Clear();
                foreach (var item in list)
                {
                    fileRename.Add(item);
                }    
            }
            else
            {
                fileRename.Clear();
                int index = applyComboBox.SelectedIndex - 1;
                fileRename.Add(list[index]);
            }
            _fileRename.Clear();
            foreach (var file in fileRename)
            {
                _fileRename.Add(file.FileName);
            }           
            UpdateNewFileName();
        }
        private void startBatch_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < fileRename.Count; i++)
            {
                if (fileRename[i].FileName != fileRename[i].NewFileName)
                {
                    try
                    {
                        string newPath = fileRename[i].Path.Replace(fileRename[i].FileName, "") + fileRename[i].NewFileName;
                        File.Move(fileRename[i].Path, newPath);
                        fileRename[i].FileName = fileRename[i].NewFileName;
                        fileRename[i].Path = newPath;
                        UpdateData();
                    }
                    catch (Exception ex)
                    {
                        fileRename[i].Error = ex.Message;
                        UpdateData();
                    }
                }    
            }    
        }
        private Type GetTypeOfRule(string name)
        {
            foreach(Rule rule in rulesComboBoxSource)
            {
                if (rule.Name == name)
                    return rule.Type;
            }
            return null;
        }
        private void presetsCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            plugins.Clear();
            rulesList.Clear();
            Preset preset = (Preset)presetsCombobox.SelectedItem;
            string[] lines = File.ReadAllLines(preset.Path);
            RuleParserFactory factory = new RuleParserFactory();
            foreach (string line in lines)
            {
                int firstSpaceIndex = line.IndexOf(" ");
                string firstWord = string.Empty;
                if (firstSpaceIndex > 0)
                {
                    firstWord = line.Substring(0, firstSpaceIndex);
                }
                else
                {
                    firstWord = line;
                }                
                IRenameRuleParser parser = factory.Create(firstWord);
                IRenameRule rule = parser.Parse(line, GetTypeOfRule(firstWord + "Rule"));
                plugins.Add(rule);
                rulesList.Add(new Rule { Name = line, Type = GetTypeOfRule(firstWord + "Rule") });
                if (firstWord == "AddCounter")
                {
                    addCounterRuleParameters.Clear();
                    foreach(var param in parser.GetParameters())
                    {
                        addCounterRuleParameters.Add(param);
                    }                  
                }
            }
            UpdateNewFileName();
        }
        private bool presetExisted(Preset pre)
        {
            foreach(var preset in presets)
            {
                if (pre.Name == preset.Name)
                    return true;
            }
            return false;
        }
        private void savePreset_Click(object sender, RoutedEventArgs e)
        {
            if (rulesList.Count == 0)
                return;
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = Path.GetDirectoryName(exePath);
            string message;
            MessageBoxButton msgButton;
            if (presetsCombobox.SelectedIndex == -1)
            {
                message = "Do you want to save a new preset?";
                msgButton = MessageBoxButton.OKCancel;
            }
            else
            {
                message = "Do you want to save a new preset or current preset?";
                msgButton = MessageBoxButton.YesNoCancel;
            }           
            MessageBoxResult result = MessageBox.Show(message, "Save Preset", msgButton, MessageBoxImage.Question);
            if (result == MessageBoxResult.Cancel)
                return;
            else if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                var savePresetWindow = new SavePreset();
                if (savePresetWindow.ShowDialog() == true)
                {
                    using (TextWriter tw = new StreamWriter(folder + "/Preset/" + savePresetWindow.PresetName + ".txt"))
                    {
                        foreach (var rule in rulesList)
                            tw.WriteLine(rule.Name);
                    }                   
                    Preset newPreset = new Preset
                    {
                        Name = savePresetWindow.PresetName + ".txt",
                        Path = folder + "/Preset/" + savePresetWindow.PresetName + ".txt"
                    };
                    if (!presetExisted(newPreset))
                    {
                        presets.Add(newPreset);
                        presetsCombobox.SelectedItem = newPreset;
                    }                    
                }
            }
            else
            {
                Preset preset = (Preset)presetsCombobox.SelectedItem;
                using (TextWriter tw = new StreamWriter(preset.Path))
                {
                    foreach (var rule in rulesList)
                        tw.WriteLine(rule.Name);
                }
            }    
        }

        private void FilesGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    AddNewFile(Path.GetFileName(file), file);
                }
            }
        }
    }
}
