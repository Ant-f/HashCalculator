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
using System.IO;
using HashCalculator.ViewModel;

namespace HashCalculator.Service
{
    /// <summary>
    /// Provides methods for interacting with files
    /// </summary>
    public class FileOperations : IFileOperations
    {
        /// <summary>
        /// Create a new text file, or overwrite the file at the specified path
        /// </summary>
        /// <param name="path">Path to create a new file</param>
        /// <returns>A TextWriter object to write text with</returns>
        public TextWriter CreateTextFile(string path)
        {
            var streamWriter = File.CreateText(path);
            return streamWriter;
        }

        /// <summary>
        /// Open a file for reading
        /// </summary>
        /// <param name="path">Path to the file to open for reading</param>
        /// <returns>A stream for reading the file</returns>
        public ReadProgressFileStream ReadFile(string path)
        {
            var stream = new ReadProgressFileStream(path);
            return stream;
        }
    }
}
