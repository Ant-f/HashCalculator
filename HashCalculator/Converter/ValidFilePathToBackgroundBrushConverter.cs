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

using HashCalculator.View;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HashCalculator.Converter
{
    /// <summary>
    /// Converts a bool that represents whether a file path is valid, to a brush
    /// that visually conveys this. Used to highlight invalid file paths.
    /// </summary>
    public class ValidFilePathToBackgroundBrushConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is bool)
            {
                var boolValue = (bool)value;
                if (boolValue)
                {
                    return AppBrushes.DefaultBrush;
                }
                else
                {
                    return AppBrushes.ErrorBrush;
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
            var brushValue = value as Brush;
            if (brushValue != null)
            {
                if (brushValue.Equals(AppBrushes.DefaultBrush))
                {
                    return true;
                }
                else if (brushValue.Equals(AppBrushes.ErrorBrush))
                {
                    return false;
                }
            }

            throw new ArgumentException();
        }
    }
}