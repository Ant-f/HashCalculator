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

using System;
using System.Security.Cryptography;
using System.Text;
using HashCalculator.Interface;
using HashCalculator.ViewModel;

namespace HashCalculator.Service
{
    /// <summary>
    /// Provides methods related to calculating file hash codes
    /// </summary>
    public class HashCodeCalculationService : PropertyChangedNotifier,
        IHashCodeCalculationService
    {
        private readonly IFileOperations _fileOperations;

        private double _normalizedProgress;
        private int _percentageProgress;

        /// <summary>
        /// Progress of a running calculation, expressed as a value in the
        /// range 0 to 1
        /// </summary>
        public double NormalizedProgress
        {
            get
            {
                return _normalizedProgress;
            }

            private set
            {
                if (_normalizedProgress != value)
                {
                    _normalizedProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Progress of a running calculation, expressed as a value in the
        /// range 0 to 100
        /// </summary>
        public int PercentageProgress
        {
            get
            {
                return _percentageProgress;
            }

            private set
            {
                if (_percentageProgress != value)
                {
                    _percentageProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public HashCodeCalculationService(IFileOperations fileOperation)
        {
            _fileOperations = fileOperation;
        }

        /// <summary>
        /// Calculate the hash code of the file at the specified location
        /// </summary>
        /// <param name="algorithm">
        /// The algorithm to use when calculating the resultant hash code
        /// </param>
        /// <param name="filePath">
        /// Path to the file to calculate a hash code for
        /// </param>
        /// <returns>Hexadecimal representation of the file's hash code</returns>
        public string CalculateHashCode(HashAlgorithm algorithm, string filePath)
        {
            ReadProgressFileStream fileStream = null;
            string hashCode;

            try
            {
                using (fileStream = _fileOperations.ReadFile(filePath))
                {
                    fileStream.ProgressUpdate += FileStreamProgressUpdate;
                    var hash = algorithm.ComputeHash(fileStream);
                    hashCode = ConvertBytesToHexString(hash);
                }
            }
            catch (Exception)
            {
                hashCode = string.Empty;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.ProgressUpdate -= FileStreamProgressUpdate;
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Converts the provided byte array to its Hexadecimal representation
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <returns>Hexadecimal representation of the provided byte array</returns>
        internal string ConvertBytesToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var byteValue in bytes)
            {
                var stringValue = $"{byteValue:X2}";
                sb.Append(stringValue);
            }

            var finalString = sb.ToString();
            return finalString;
        }
        
        private void FileStreamProgressUpdate(object sender, ReadProgressEventArgs e)
        {
            NormalizedProgress = e.NormalizedProgress;
            PercentageProgress = e.PercentageProgress;
        }
    }
}
