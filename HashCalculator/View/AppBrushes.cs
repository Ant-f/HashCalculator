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

using System.Windows.Media;

namespace HashCalculator.View
{
    /// <summary>
    /// Holds references to <see cref="Brush"/> instances used within the
    /// input-files data grid
    /// </summary>
    internal class AppBrushes
    {
        /// <summary>
        /// The default <see cref="Brush"/> for rows
        /// </summary>
        public static Brush DefaultBrush { get; }
            = new SolidColorBrush(Colors.White);

        /// <summary>
        /// Used to indicate hash sum mismatches and invalid file paths
        /// </summary>
        public static Brush ErrorBrush { get; }
            = new SolidColorBrush(Colors.Pink);

        /// <summary>
        /// Used to indicate an ambiguity when matching file hash sums
        /// </summary>
        public static Brush IndeterminateBrush { get; }
            = new SolidColorBrush(Colors.Yellow);

        /// <summary>
        /// Used to indicate that an exact match for file path and hash sum
        /// exists
        /// </summary>
        public static Brush SuccessBrush { get; }
            = new SolidColorBrush(Colors.LightGreen);
    }
}
