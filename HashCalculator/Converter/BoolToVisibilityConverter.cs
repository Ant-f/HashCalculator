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
using System.Windows;
using System.Windows.Data;

namespace HashCalculator.Converter
{
    /// <summary>
    /// Converts true to <see cref="Visibility.Visible"/>, and false to
    /// <see cref="Visibility.Collapsed"/>
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
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
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
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
            if (value is Visibility)
            {
                var visibility = (Visibility) value;
                if (visibility == Visibility.Visible)
                {
                    return true;
                }
                else if (visibility == Visibility.Collapsed)
                {
                    return false;
                }
            }

            throw new ArgumentException();
        }
    }
}