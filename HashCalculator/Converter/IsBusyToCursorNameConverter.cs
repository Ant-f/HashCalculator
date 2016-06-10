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
using System.Globalization;
using System.Windows.Data;

namespace HashCalculator.Converter
{
    /// <summary>
    /// Converts a bool that represents whether the program is in a busy state,
    /// to the name of an appropriate cursor. Used when setting the cursor according
    /// to whether hash codes are being calculated.
    /// </summary>
    public class IsBusyToCursorNameConverter : IValueConverter
    {
        public const string ArrowCursorName = "Arrow";
        public const string WaitCursorName = "Wait";

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is bool)
            {
                var boolValue = (bool) value;
                if (boolValue)
                {
                    return WaitCursorName;
                }
                else
                {
                    return ArrowCursorName;
                }
            }

            throw new ArgumentException();
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                switch (stringValue)
                {
                    case ArrowCursorName:
                        return false;

                    case WaitCursorName:
                        return true;
                }
            }

            throw new ArgumentException();
        }
    }
}