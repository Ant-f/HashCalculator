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

using HashCalculator.Converter;
using NUnit.Framework;
using System.Windows;

namespace HashCalculatorTests.Converter
{
    [TestFixture]
    public class BoolToVisibilityConverterTests
    {
        [Test]
        public void TrueConvertsToVisible()
        {
            var converter = new BoolToVisibilityConverter();

            var result = converter.Convert(true, null, null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [Test]
        public void FalseConvertsToCollapsed()
        {
            var converter = new BoolToVisibilityConverter();

            var result = converter.Convert(false, null, null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [Test]
        public void VisibleConvertsBackToTrue()
        {
            var converter = new BoolToVisibilityConverter();

            var result = converter.ConvertBack(Visibility.Visible, null, null, null);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void FalseConvertsBackToCollapsed()
        {
            var converter = new BoolToVisibilityConverter();

            var result = converter.ConvertBack(Visibility.Collapsed, null, null, null);

            Assert.AreEqual(false, result);
        }
    }
}
