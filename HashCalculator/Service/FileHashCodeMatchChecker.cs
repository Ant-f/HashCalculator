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

using System;
using HashCalculator.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HashCalculator.Interface;
using HashCalculator.ViewModel.Model;

namespace HashCalculator.Service
{
    public class FileHashCodeMatchChecker : IFileHashCodeMatchChecker
    {
        public HashCodeMatchCriteria FindMatchCriteria(
            FileHashMetadata inputHashCodeMetadata,
            List<FileHashMetadata> knownHashCodes,
            bool fullPathFileNameMatching)
        {
            // Set 'inputFileName' as either file name only, or the full path depending on fullPathFileNameMatching

            var inputFileName = fullPathFileNameMatching ?
                inputHashCodeMetadata.FilePath : Path.GetFileName(inputHashCodeMetadata.FilePath);

            FileHashMetadata[] matchingNameMetadata;

            if (fullPathFileNameMatching)
            {
                matchingNameMetadata = knownHashCodes.Where(metadata =>
                    string.Compare(
                        inputFileName,
                        metadata.FilePath,
                        StringComparison.OrdinalIgnoreCase) == 0).ToArray();
            }
            else
            {
                matchingNameMetadata = knownHashCodes.Where(metadata =>
                    string.Compare(
                        inputFileName,
                        Path.GetFileName(metadata.FilePath),
                        StringComparison.OrdinalIgnoreCase) == 0).ToArray();
            }

            var criteria = HashCodeMatchCriteria.None;

            if (matchingNameMetadata.Length == 1)
            {
                // Exactly one file name match
                criteria |= HashCodeMatchCriteria.FileNameMatch;
            }

            if (knownHashCodes.Any(knownHashMetdata =>
                string.Compare(inputHashCodeMetadata.FileHashCode, knownHashMetdata.FileHashCode,
                    StringComparison.OrdinalIgnoreCase) == 0))
            {
                // Hash code match
                criteria |= HashCodeMatchCriteria.HashCodeMatch;
            }

            return criteria;
        }
    }
}
