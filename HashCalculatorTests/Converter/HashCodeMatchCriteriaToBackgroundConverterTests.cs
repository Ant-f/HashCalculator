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
using System;
using HashCalculator.ViewModel.Model;

namespace HashCalculatorTests.Converter
{
    [TestFixture]
    public class HashCodeMatchCriteriaToBackgroundConverterTests
    {
        [Test]
        public void FilePathMatchAndFileHashMatchReturnsSuccessBrush()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            var brush = converter.Convert(
                HashCodeMatchCriteria.FileNameMatch | HashCodeMatchCriteria.HashCodeMatch,
                null, null, null);

            Assert.AreEqual(brush, AppBrushes.SuccessBrush);
        }

        [Test]
        public void FilePathMatchAndFileHashMismatchReturnsErrorBrush()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            var brush = converter.Convert(
                HashCodeMatchCriteria.FileNameMatch,
                null, null, null);

            Assert.AreEqual(brush, AppBrushes.ErrorBrush);
        }

        [Test]
        public void FilePathMismatchAndFileHashMatchReturnsIndeterminateBrush()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            var brush = converter.Convert(
                HashCodeMatchCriteria.HashCodeMatch,
                null, null, null);

            Assert.AreEqual(brush, AppBrushes.IndeterminateBrush);
        }

        [Test]
        public void FilePathMismatchAndFileHashMismatchReturnsDefaultBrush()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            var brush = converter.Convert(HashCodeMatchCriteria.None,
                null, null, null);

            Assert.AreEqual(brush, AppBrushes.DefaultBrush);
        }

        [Test]
        public void ConverterConvertThrowsArgumentExceptionWhenGivenNull()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            Assert.Throws<ArgumentException>(() => converter.Convert(null, null, null, null));
        }

        [Test]
        public void ConverterConvertBackThrowsArgumentExceptionWhenGivenNull()
        {
            var converter = new HashCodeMatchCriteriaToBackgroundConverter();

            Assert.Throws<NotSupportedException>(() => converter.ConvertBack(null, null, null, null));
        }
    }
}
