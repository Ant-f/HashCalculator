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
using System;
using System.Linq;

namespace HashCalculator.ViewModel.Command
{
    public class ExportHashList : IExportHashList
    {
        private readonly IExportPathPrompter _exportPathPrompter;
        private readonly IHashCodeExporter _hashCodeExporter;
        private readonly IUserInput _userInput;

        public event EventHandler CanExecuteChanged;

        public ExportHashList(
            IExportPathPrompter exportPathPrompter,
            IHashCodeExporter hashCodeExporter,
            IUserInput userInput)
        {
            _exportPathPrompter = exportPathPrompter;
            _hashCodeExporter = hashCodeExporter;
            _userInput = userInput;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            Export();
        }

        private void Export()
        {
            var path = _exportPathPrompter.ShowPrompt();

            if (!string.IsNullOrWhiteSpace(path))
            {
                var inputList = _userInput.InputFileList.Select(itm => itm.HashMetadata).ToArray();
                _hashCodeExporter.Export(path, inputList, _userInput.MatchFullFilePath);
            }
        }
    }
}
