﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TranslatorApk.Logic.Interfaces.SettingsPages;
using TranslatorApk.Logic.Utils;
using TranslatorApk.Logic.ViewModels.SettingsPages;
using TranslatorApk.Logic.ViewModels.TreeViewModels;

namespace TranslatorApk.Windows
{
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            ViewModel = new SettingsViewModel();
        }

        internal SettingsViewModel ViewModel
        {
            get => DataContext as SettingsViewModel;
            private set => DataContext = value;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ViewModel.CurrentPage = e.NewValue.As<SettingsTreeViewNodeModel>();
        }

        private void Frame_OnNavigated(object sender, NavigationEventArgs e)
        {
            ISettingsPageViewModel currentModel = ViewModel.CurrentPage.PageViewModel;

            currentModel.RefreshData();

            NavigationFrame.Content.As<Page>().DataContext = currentModel;
        }

        private void SettingsWindow_OnClosed(object sender, EventArgs e)
        {
            ViewModel.Dispose();
        }
    }
}
