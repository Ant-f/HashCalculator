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
using HashCalculator.ViewModel.Model;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HashCalculator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _dropTargetHasMouseEnterHandler = false;
        private IHashCalculatorViewModel ViewModel => (IHashCalculatorViewModel) DataContext;

        public MainWindow()
        {
            InitializeComponent();
            AttachDropTargetMouseEnterHandler();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            ViewModel.AbortHashCalculation();
        }

        private void DropTargetBorderDrop(object sender, DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data.ContainsFileDropList())
            {
                StringCollection collection = data.GetFileDropList();
                ViewModel.AddFilesToInputList(collection.OfType<string>().ToArray());
            }

            AttachDropTargetMouseEnterHandler();
            DropTargetBorder.IsHitTestVisible = false;
        }

        private void DropTargetBorderPreviewDragOver(object sender, DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data.ContainsFileDropList())
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void DropTargetBorderMouseEnter(object sender, MouseEventArgs e)
        {
            DropTargetBorder.IsHitTestVisible = false;
        }

        private void inputFileDataGridMouseLeave(object sender, MouseEventArgs e)
        {
            DropTargetBorder.IsHitTestVisible = true;
        }

        private void DropTargetBorderDragEnter(object sender, DragEventArgs e)
        {
            RemoveDropTargetMouseEnterHandler();
        }

        private void DropTargetBorderDragLeave(object sender, DragEventArgs e)
        {
            AttachDropTargetMouseEnterHandler();
        }
        
        private void AttachDropTargetMouseEnterHandler()
        {
            if (!_dropTargetHasMouseEnterHandler)
            {
                _dropTargetHasMouseEnterHandler = true;
                DropTargetBorder.MouseEnter += DropTargetBorderMouseEnter;
            }
        }

        private void RemoveDropTargetMouseEnterHandler()
        {
            if (_dropTargetHasMouseEnterHandler)
            {
                _dropTargetHasMouseEnterHandler = false;
                DropTargetBorder.MouseEnter -= DropTargetBorderMouseEnter;
            }
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
                        ViewModel.RemoveInputListEntry(entry);
                    else
                        exp.UpdateSource();
                }
            }
            else
            {
                // For 'DataGrid.NewItemPlaceholder'
                if (!String.IsNullOrWhiteSpace(tb.Text))
                {
                    ViewModel.AddFileToInputList(tb.Text);
                    tb.Text = String.Empty;
                }
            }
        }
    }
}
