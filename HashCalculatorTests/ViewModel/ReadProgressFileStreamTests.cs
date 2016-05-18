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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HashCalculatorTests.ViewModel
{
    [TestFixture]
    public class ReadProgressFileStreamTests
    {
        [Test, Explicit]
        public void ReadFileProgressIsBetween0And1()
        {
            // Arrange

            var progressValues = new List<double>();

            var eventHandler = new EventHandler<ReadProgressEventArgs>((s, e) =>
            {
                progressValues.Add(e.Progress);
            });

            var runningDir = AppDomain.CurrentDomain.BaseDirectory;
            var testFilePath = $"{runningDir}\\TestingData\\LoremIpsum.txt";

            // Act

            using (var stream = new ReadProgressFileStream(testFilePath))
            {
                stream.ProgressUpdate += eventHandler;

                using (var reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();
                }

                stream.ProgressUpdate -= eventHandler;
            }

            // Assert

            Assert.IsNotEmpty(progressValues);

            var minValue = progressValues.Min();
            Assert.IsTrue(minValue >= 0);

            var maxValue = progressValues.Max();
            Assert.IsTrue(maxValue <= 1);
        }

        [Test, Explicit]
        public void ReadFileProgressValuesAreEvenlyDistributed()
        {
            // Arrange

            var progressValues = new List<double>();

            var eventHandler = new EventHandler<ReadProgressEventArgs>((s, e) =>
            {
                progressValues.Add(e.Progress);
            });

            var runningDir = AppDomain.CurrentDomain.BaseDirectory;
            var testFilePath = $"{runningDir}\\TestingData\\LoremIpsum.txt";

            // Act

            using (var stream = new ReadProgressFileStream(testFilePath))
            {
                stream.ProgressUpdate += eventHandler;

                using (var reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();
                }

                stream.ProgressUpdate -= eventHandler;
            }

            // Assert

            Assert.IsNotEmpty(progressValues);

            Assert.IsTrue(progressValues[0] < 0.1);
            Assert.IsTrue(progressValues[progressValues.Count-1] > 0.9);

            var average = progressValues.Average();
            Assert.IsTrue(average > 0.49);
            Assert.IsTrue(average < 0.51);
        }
    }
}
