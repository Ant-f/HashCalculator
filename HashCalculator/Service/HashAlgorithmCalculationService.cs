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

using System.Security.Cryptography;
using System.Text;
using HashCalculator.Interface;
using HashCalculator.ViewModel.Model;

namespace HashCalculator.Service
{
    public class HashAlgorithmCalculationService
    {
        private readonly IFileOperations _fileOperations;

        public HashAlgorithmCalculationService(IFileOperations fileOperation)
        {
            _fileOperations = fileOperation;
        }

        public void CalculateHashCodes(HashAlgorithm algorithm, InputFileListEntry[] listEntries)
        {
            using (algorithm)
            {
                foreach (var listEntry in listEntries)
                {
                    using (var fileStream = _fileOperations.ReadFile(listEntry.FilePath))
                    {
                        var hash = algorithm.ComputeHash(fileStream);
                        var hex = ConvertBytesToHexString(hash);
                        listEntry.CalculatedFileHash = hex;
                    }
                }
            }
        }

        public string ConvertBytesToHexString(byte[] bytes)
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
    }
}
