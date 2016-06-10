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

using Microsoft.Win32;
using System.Windows;
using HashCalculator.Interface;

namespace HashCalculator.Service
{
    /// <summary>
    /// Used when exporting data to prompt the user for a destination to export
    /// data to
    /// </summary>
    public class ExportPathPrompter : IExportPathPrompter
    {
        /// <summary>
        /// Show a dialog box to select a file path
        /// </summary>
        /// <returns>The selected file path, or null if the user cancels the dialog</returns>
        public string ShowPrompt()
        {
            string path = null;

            var dialog = new SaveFileDialog
            {
                Filter =
                    "Text Files|*.txt" +
                    "|All Files|*.*"
            };

            var result = dialog.ShowDialog(Application.Current.MainWindow);

            if (result.Value)
            {
                path = dialog.FileName;
            }

            return path;
        }
    }
}
