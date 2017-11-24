﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using TranslatorApk.Logic.OrganisationItems;
using TranslatorApk.Logic.Utils;
using TranslatorApk.Logic.ViewModels;
using UsefulFunctionsLib;

using Res = TranslatorApk.Resources.Localizations.Resources;

namespace TranslatorApk.Windows
{
    /// <summary>
    /// Interaction logic for AddLanguageWindow.xaml
    /// </summary>
    public partial class AddLanguageWindow
    {
        /// <summary>
        /// Список исходных языков
        /// </summary>
        public ObservableCollection<string> SourceLanguages { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Список целевых языков
        /// </summary>
        public ObservableCollection<NewLanguageViewModel> TargetLanguages { get; } = new ObservableCollection<NewLanguageViewModel>();

        private readonly List<string> _folderLangs = GlobalVariables.SettingsFoldersOfLanguages;
        private readonly List<string> _folderLocalizedLangs = GlobalVariables.SettingsNamesOfFolderLanguages;

        private readonly string _folder;

        /// <summary>
        /// Создаёт окно добавления новых языков
        /// </summary>
        /// <param name="folderOfProject">Папка проекта</param>
        public AddLanguageWindow(string folderOfProject)
        {
            InitializeComponent();

            if (folderOfProject == null)
                return;

            _folder = Path.Combine(folderOfProject, "res");

            var values = Directory.GetDirectories(_folder, "values*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);

            foreach (string value in values)
            {
                int index = _folderLangs.IndexOf(value);

                if (index > -1)
                {
                    SourceLanguages.Add(_folderLocalizedLangs[index]);
                }
            }

            foreach (string lang in _folderLocalizedLangs)
            {
                if (SourceLanguages.All(src => src != lang))
                {
                    string name = _folderLangs[_folderLocalizedLangs.IndexOf(lang)];

                    TargetLanguages.Add(
                        new NewLanguageViewModel
                        {
                            LanguageIcon = ImageUtils.GetFlagImage(name),
                            Title = lang
                        }
                    );
                }
            }
        }

        private void AddLangClick(object sender, RoutedEventArgs e)
        {
            if (TargetBox.SelectedValue == null)
            {
                MessBox.ShowDial(Res.LanguageIsNotSelected, Res.ErrorLower);
                return;
            }

            string target = TargetBox.SelectedValue.As<NewLanguageViewModel>().Title;

            var sourcedir = Path.Combine(_folder, "values");
            var targetdir = Path.Combine(_folder, _folderLangs[_folderLocalizedLangs.IndexOf(target)]);

            if (Directory.Exists(targetdir))
                Directory.Delete(targetdir, true);

            Directory.CreateDirectory(targetdir);

            var filesToCopy = new[] {"strings.xml", "arrays.xml"};

            foreach (var file in filesToCopy)
            {
                string src = Path.Combine(sourcedir, file);

                if (File.Exists(src))
                    File.Copy(src, Path.Combine(targetdir, file), true);
            }

            SourceLanguages.Add(target);
            TargetLanguages.RemoveAt(TargetLanguages.FindIndex(it => it.Title == target));

            MessBox.ShowDial(Res.AllDone);
        }
    }
}
