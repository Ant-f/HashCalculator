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
using System.Threading;
using System.Threading.Tasks;
using HashCalculator.ViewModel;

namespace HashCalculator.Service
{
    /// <summary>
    /// Service to calculate the hash sums of multiple files in a single batch
    /// </summary>
    public class HashCodeBatchCalculationService : PropertyChangedNotifier,
        IHashCodeBatchCalculationService
    {
        private readonly IHashCodeCalculationService _hashCodeCalculationService;

        private bool _calculationIsRunning = false;
        private CancellationTokenSource _cancellationTokenSource;
        private string _listProgress;

        /// <summary>
        /// Represents whether a calculation batch is currently in progress
        /// </summary>
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

        /// <summary>
        /// Indicates the progress of the running calculation batch in terms of
        /// list items
        /// </summary>
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

        public HashCodeBatchCalculationService(
            IHashCodeCalculationService hashCodeCalculationService)
        {
            _hashCodeCalculationService = hashCodeCalculationService;
        }

        /// <summary>
        /// Asynchronously calculate hash sums for each file referenced in
        /// <see cref="collection"/> using the specified hash algorithm
        /// </summary>
        /// <param name="algorithmName">
        /// Name of the hash algorithm to use, e.g. SHA1
        /// </param>
        /// <param name="collection">
        /// List referencing files to calculate hash sums for
        /// </param>
        /// <returns>A <see cref="Task"/> that the calculation runs in</returns>
        public async Task CalculateHashCodes(
            string algorithmName,
            IList<InputFileListEntry> collection)
        {
            await Task.Run(() =>
            {
                using (_cancellationTokenSource = new CancellationTokenSource())
                {
                    var cancellationToken = _cancellationTokenSource.Token;

                    CalculationIsRunning = true;

                    using (var algorithm = HashAlgorithm.Create(algorithmName))
                    {
                        if (algorithm != null)
                        {
                            for (int i = 0; i < collection.Count && !cancellationToken.IsCancellationRequested; i++)
                            {
                                ListProgress = $"{i + 1}/{collection.Count}";

                                var listEntry = collection[i];

                                var hashCode = _hashCodeCalculationService.CalculateHashCode(
                                    algorithm,
                                    listEntry.FilePath);

                                listEntry.CalculatedFileHash = hashCode;
                            }
                        }
                        ListProgress = string.Empty;
                    }

                    CalculationIsRunning = false;
                }
            });
        }

        /// <summary>
        /// Abort the calculation batch after the hash sum calculation for the
        /// list item that is in progress completes
        /// </summary>
        public void AbortCalculation()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
