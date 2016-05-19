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
using System.IO;

namespace HashCalculator.ViewModel
{
    /// <summary>
    /// Subclass of System.IO.FileStream. Raises events during read operations
    /// to indicate current relative position of the stream, represented as a
    /// value between 0 and 1
    /// </summary>
    public class ReadProgressFileStream : FileStream
    {
        public event EventHandler<ReadProgressEventArgs> ProgressUpdate;

        private int _percentage;

        public ReadProgressFileStream(string filepath)
            : base(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
        }

        public override int Read(byte[] array, int offset, int count)
        {
            UpdateReadPercentage();
            return base.Read(array, offset, count);
        }

        public override int ReadByte()
        {
            UpdateReadPercentage();
            return base.ReadByte();
        }

        private void UpdateReadPercentage()
        {
            var normalizedPercentage = Position/(double) Length;
            var intPercentage = (int) (normalizedPercentage*100);

            if (_percentage != intPercentage)
            {
                var eventArgs = new ReadProgressEventArgs(normalizedPercentage, intPercentage);
                ProgressUpdate?.Invoke(this, eventArgs);
            }

            _percentage = intPercentage;
        }
    }
}
