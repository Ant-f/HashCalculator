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
using System.Windows.Data;
using HashCalculator.ViewModel.Model;

namespace HashCalculator.Converter
{
    public class HashCodeMatchCriteriaToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is HashCodeMatchCriteria)
            {
                var criteria = (HashCodeMatchCriteria) value;
                if (criteria.HasFlag(HashCodeMatchCriteria.FileNameMatch))
                {
                    if (criteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch))
                    {
                        // File name match, hash code match
                        return AppBrushes.SuccessBrush;
                    }
                    else
                    {
                        // File name match, hash code mismatch
                        return AppBrushes.ErrorBrush;
                    }
                }
                else
                {
                    if (criteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch))
                    {
                        // File name mismatch, hash code match
                        return AppBrushes.IndeterminateBrush;
                    }
                    else
                    {
                        // File name mismatch, hash code mismatch
                        return AppBrushes.DefaultBrush;
                    }
                }
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}