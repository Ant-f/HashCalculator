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

namespace HashCalculator.ViewModel
{
    /// <summary>
    /// Class used to contain information about the relative position
    /// within a ReadProgressFileStream, represented as a value
    /// between 0 and 1
    /// </summary>
    public class ReadProgressEventArgs : EventArgs
    {
        /// <summary>
        /// The relative position within a ReadProgressFileStream,
        /// represented as a value between 0 and 1
        /// </summary>
        public double NormalizedProgress { get; private set; }

        /// <summary>
        /// The relative position within a ReadProgressFileStream,
        /// represented as a value between 0 and 100
        /// </summary>
        public int PercentageProgress { get; private set; }

        public ReadProgressEventArgs(double normalizedProgress, int percentageProgress)
        {
            NormalizedProgress = normalizedProgress;
            PercentageProgress = percentageProgress;
        }
    }
}
