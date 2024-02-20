using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ExportViewer.GUI.Interfaces;

namespace ExportViewer.GUI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<IMenuItem> MenuItems { get; }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem , value))
                {
                    IsMenuOpen = false;
                }
            }
        }

        private bool isMenuOpen;
        public bool IsMenuOpen
        {
            get => isMenuOpen;
            set => SetProperty(ref isMenuOpen , value);
        }

        public MainViewModel ()
        {
            MenuItems = new ObservableCollection<IMenuItem>()
            {
                new UtilitiesViewModel(),
                new ChatsViewModel()
            };
            SelectedItem = MenuItems[0];
        }
    }
}
