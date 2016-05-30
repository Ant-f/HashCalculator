// HashCalculator
// Tool for calculating and comparing file hash sums, e.g. sha1
// Copyright(C) 2016 Anthony Fung

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using HashCalculator.Interface;
using HashCalculator.Ioc;
using HashCalculator.ViewModel.Model;
using Ninject;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HashCalculator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUserInput _userInput;

        public MainWindow()
        {
            InitializeComponent();

            _userInput = NinjectContainer.Kernel.Get<IUserInput>();
        }

        private void PART_ContentHost_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.OriginalSource as TextBox;
            BindingExpression exp = tb.GetBindingExpression(TextBox.TextProperty);
            InputFileListEntry entry = exp.DataItem as InputFileListEntry;

            if (entry != null)
            {
                if (exp.ParentBinding.Path.Path == "FilePath")
                {
                    if (String.IsNullOrWhiteSpace(tb.Text))
                        _userInput.RemoveInputListEntry(entry);
                    else
                        exp.UpdateSource();
                }
            }
            else
            {
                // For 'DataGrid.NewItemPlaceholder'
                if (!String.IsNullOrWhiteSpace(tb.Text))
                {
                    _userInput.AddFileToInputList(tb.Text);
                    tb.Text = String.Empty;
                }
            }
        }
    }
}
