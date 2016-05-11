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
using HashCalculator.Model;
using System.Collections.Generic;
using System.IO;

namespace HashCalculator.Service
{
    public class HashCodeExporter : IHashCodeExporter
    {
        private readonly IFileCreator _fileCreator;

        /// <summary>
        /// Provides methods to write FileHashMetadata objects to a file
        /// </summary>
        /// <param name="fileCreator">An IFileCreator, used when creating files on the filesystem</param>
        public HashCodeExporter(IFileCreator fileCreator)
        {
            _fileCreator = fileCreator;
        }

        /// <summary>
        /// Write the data provided to a text file
        /// </summary>
        /// <param name="exportPath">Path to write exported file to</param>
        /// <param name="metadataList">List of metadata to write to file</param>
        /// <param name="fileNameOnly">true to write only the file names of files in
        /// metadataList, false to write full paths</param>
        public void Export(string exportPath, IList<FileHashMetadata> metadataList, bool fileNameOnly)
        {
            using (var streamWriter = _fileCreator.CreateTextFile(exportPath))
            {
                foreach (var listEntry in metadataList)
                {
                    var outputFilePath = fileNameOnly
                        ? Path.GetFileName(listEntry.FilePath)
                        : listEntry.FilePath;

                    streamWriter.WriteLine($"{listEntry.FileHashCode} *{outputFilePath}");
                }
            }
        }
    }
}
