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

using HashCalculator.Model;
using HashCalculator.ViewModel.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HashCalculator.ViewModel;

namespace HashCalculator.Interface
{
    public interface IHashCalculatorViewModel
    {
        bool HashCalculationIsRunning { get; set; }
        bool MatchFullFilePath { get; set; }
        ObservableCollection<InputFileListEntry> InputFileList { get; }
        List<FileHashMetadata> KnownFileHashList { get; }
        string KnownFileHashCodesText { get; set; }

        RelayCommand CalculateHashCodesCommand { get; }
        RelayCommand ExportHashListCommand { get; }

        void BuildKnownFileHashList();
        void StartHashCalculationBatch();
        void AbortHashCalculation();
        void AddFilesToInputList(string[] files);
        void AddFileToInputList(string path);
        void RemoveInputListEntry(InputFileListEntry entry);
    }
}
