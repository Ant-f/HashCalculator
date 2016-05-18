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

using HashCalculator.Service;
using NUnit.Framework;
using System;
using HashCalculator.ViewModel;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class FileOperationsTests
    {
        [Test, Explicit]
        public void ReadFileReturnsReadProgressFileStream()
        {
            var fileOperations = new FileOperations();

            var runningDir = AppDomain.CurrentDomain.BaseDirectory;
            var testFilePath = $"{runningDir}\\TestingData\\LoremIpsum.txt";

            Type fileStreamType = null;
            var exceptionThrown = false;

            try
            {
                using (var stream = fileOperations.ReadFile(testFilePath))
                {
                    fileStreamType = stream.GetType();
                }
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsFalse(exceptionThrown, "Exception should not have been thrown");
            Assert.AreEqual(typeof(ReadProgressFileStream), fileStreamType);
        }
    }
}
