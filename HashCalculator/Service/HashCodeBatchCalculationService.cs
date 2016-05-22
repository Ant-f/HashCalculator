﻿// HashCalculator
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

using System.Collections.Generic;
using HashCalculator.Interface;
using HashCalculator.ViewModel.Model;
using System.Security.Cryptography;
using HashCalculator.ViewModel;

namespace HashCalculator.Service
{
    public class HashCodeBatchCalculationService : PropertyChangedNotifier, IHashCodeBatchCalculationService
    {
        private readonly IHashCodeCalculationService _hashCodeCalculationService;

        private bool _calculationIsRunning = false;
        private string _listProgress;

        public bool CalculationIsRunning
        {
            get
            {
                return _calculationIsRunning;
            }

            private set
            {
                if (_calculationIsRunning != value)
                {
                    _calculationIsRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ListProgress
        {
            get
            {
                return _listProgress;
            }

            private set
            {
                if (_listProgress != value)
                {
                    _listProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public HashCodeBatchCalculationService(IHashCodeCalculationService hashCodeCalculationService)
        {
            _hashCodeCalculationService = hashCodeCalculationService;
        }

        public void CalculateHashCodes(string algorithmName, IList<InputFileListEntry> collection)
        {
            CalculationIsRunning = true;

            using (var algorithm = HashAlgorithm.Create(algorithmName))
            {
                if (algorithm != null)
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        ListProgress = $"{i + 1}/{collection.Count}";

                        var listEntry = collection[i];
                        var hashCode = _hashCodeCalculationService.CalculateHashCodes(algorithm, listEntry.FilePath);
                        listEntry.CalculatedFileHash = hashCode;
                    }
                }
                ListProgress = string.Empty;
            }

            CalculationIsRunning = false;
        }
    }
}
