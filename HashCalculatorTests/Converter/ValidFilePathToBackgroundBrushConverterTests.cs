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
using HashCalculator.View;
using NUnit.Framework;

namespace HashCalculatorTests.Converter
{
    [TestFixture]
    public class ValidFilePathToBackgroundBrushConverterTests
    {
        [Test]
        public void TrueConvertsToDefaultBrush()
        {
            var converter = new ValidFilePathToBackgroundBrushConverter();
            var result = converter.Convert(true, null, null, null);
            Assert.AreEqual(AppBrushes.DefaultBrush, result);
        }

        [Test]
        public void FalseConvertsToErrorBrush()
        {
            var converter = new ValidFilePathToBackgroundBrushConverter();
            var result = converter.Convert(false, null, null, null);
            Assert.AreEqual(AppBrushes.ErrorBrush, result);
        }

        [Test]
        public void DefaultBrushConvertsBackToTrue()
        {
            var converter = new ValidFilePathToBackgroundBrushConverter();
            var result = converter.ConvertBack(AppBrushes.DefaultBrush, null, null, null);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ErrorBrushConvertsToFalse()
        {
            var converter = new ValidFilePathToBackgroundBrushConverter();
            var result = converter.ConvertBack(AppBrushes.ErrorBrush, null, null, null);
            Assert.AreEqual(false, result);
        }
    }
}
