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
    class ReadProgressEventArgs : EventArgs
    {
        public double Progress { get; set; }

        public ReadProgressEventArgs(double progress)
        {
            Progress = progress;
        }
    }

    class ProgressUpdateStream : FileStream
    {
        public event EventHandler<ReadProgressEventArgs> ProgressUpdate;

        private long counter = 0;
        private long updateInterval;

        public ProgressUpdateStream(string filepath)
            : base(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
            updateInterval = this.Length / 100;
        }

        public override int Read(byte[] array, int offset, int count)
        {
            CheckFireProgressUpdate(count);
            return base.Read(array, offset, count);
        }

        public override int ReadByte()
        {
            CheckFireProgressUpdate(1);
            return base.ReadByte();
        }

        private void CheckFireProgressUpdate(int byteCount)
        {
            counter += byteCount;

            if (counter > updateInterval)
            {
                counter %= updateInterval;
                FireProgressUpdate((double)this.Position / (double)this.Length);
            }
        }

        private void FireProgressUpdate(double progress)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, new ReadProgressEventArgs(progress));
        }
    }
}
