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
using HashCalculatorTests.TestingInfrastructure;

namespace HashCalculatorTests.ViewModel
{
    [TestFixture]
    public class ReadProgressFileStreamTests
    {
        [Test, Explicit, Category(Constants.FileSystemTestCategory)]
        public void NormalizedReadFileProgressIsBetween0And1()
        {
            // Arrange

            var progressValues = new List<double>();

            var eventHandler = new EventHandler<ReadProgressEventArgs>((sender, args) =>
            {
                progressValues.Add(args.NormalizedProgress);
            });

            // Act

            using (var stream = new ReadProgressFileStream(TestingDataService.TestingDataFilePath))
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
            Assert.That(minValue, Is.GreaterThanOrEqualTo(0));

            var maxValue = progressValues.Max();
            Assert.That(maxValue, Is.LessThanOrEqualTo(1));
        }

        [Test, Explicit, Category(Constants.FileSystemTestCategory)]
        public void NormalizedReadFileProgressValuesAreEvenlyDistributed()
        {
            // Arrange

            var progressValues = new List<double>();

            var eventHandler = new EventHandler<ReadProgressEventArgs>((sender, args) =>
            {
                progressValues.Add(args.NormalizedProgress);
            });

            // Act

            using (var stream = new ReadProgressFileStream(TestingDataService.TestingDataFilePath))
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

            var collectionItemDeltaList = new List<double>();

            // Discount the final element: the delta is likely to be smaller due to
            // calculation rounding and a fixed upper limit
            var upperLimit = progressValues.Count - 1;

            for (int i = 1; i < upperLimit; i++)
            {
                var collectionItemDelta = progressValues[i] - progressValues[i - 1];
                collectionItemDeltaList.Add(collectionItemDelta);
            }

            var deltaAverage = collectionItemDeltaList.Average();

            for (int i = 0; i < collectionItemDeltaList.Count; i++)
            {
                var itemAverageDelta = Math.Abs(collectionItemDeltaList[i] - deltaAverage);
                Assert.That(itemAverageDelta, Is.LessThanOrEqualTo(0.01));
            }
        }

        [Test, Explicit, Category(Constants.FileSystemTestCategory)]
        public void PercentageReadFileProgressValuesCorrespondToNormalizedProgress()
        {
            // Arrange

            var normalizedValues = new List<double>();
            var percentageValues = new List<int>();

            var eventHandler = new EventHandler<ReadProgressEventArgs>((sender, args) =>
            {
                normalizedValues.Add(args.NormalizedProgress);
                percentageValues.Add(args.PercentageProgress);
            });

            // Act

            using (var stream = new ReadProgressFileStream(TestingDataService.TestingDataFilePath))
            {
                stream.ProgressUpdate += eventHandler;

                using (var reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();
                }

                stream.ProgressUpdate -= eventHandler;
            }

            // Assert

            Assert.IsNotEmpty(normalizedValues);
            Assert.IsNotEmpty(percentageValues);

            Assert.AreEqual(normalizedValues.Count, percentageValues.Count);

            for (int i = 0; i < normalizedValues.Count; i++)
            {
                var expectedValue = (int) (normalizedValues[i]*100);
                Assert.AreEqual(expectedValue, percentageValues[i]);
            }
        }
    }
}
