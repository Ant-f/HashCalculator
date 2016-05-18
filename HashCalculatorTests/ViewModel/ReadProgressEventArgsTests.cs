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

using HashCalculator.ViewModel;
using NUnit.Framework;

namespace HashCalculatorTests.ViewModel
{
    [TestFixture]
    public class ReadProgressEventArgsTests
    {
        [TestCase(0.1)]
        [TestCase(0.7)]
        [TestCase(2.9)]
        [TestCase(8.6)]
        [TestCase(9.0)]
        public void ValuePassAsConstructorArgumentSetsProgressProperty(double value)
        {
            var eventArgs = new ReadProgressEventArgs(value);

            Assert.AreEqual(value, eventArgs.Progress);
        }
    }
}
