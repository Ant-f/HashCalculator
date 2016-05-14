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

using HashCalculator.Interface;
using HashCalculator.Service;
using HashCalculator.ViewModel.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashCodeBatchCalculationServiceTests
    {
        [Test]
        public void HashCodeCalculationServiceIsUsedWithEachInputFileListEntry()
        {
            // Arrange

            const string fileName1 = "File1.txt";
            const string fileName2 = "File2.txt";
            const string fileName3 = "File3.txt";

            const string fileHash1 = "FileHash1";
            const string fileHash2 = "FileHash2";
            const string fileHash3 = "FileHash3";

            var calculationService = new Mock<IHashCodeCalculationService>();

            calculationService.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), fileName1))
                .Returns(fileHash1);

            calculationService.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), fileName2))
                .Returns(fileHash2);

            calculationService.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), fileName3))
                .Returns(fileHash3);

            var service = new HashCodeBatchCalculationService(calculationService.Object);

            var entry1 = new InputFileListEntry(fileName1);
            var entry2 = new InputFileListEntry(fileName2);
            var entry3 = new InputFileListEntry(fileName3);

            var collection = new[]
            {
                entry1,
                entry2,
                entry3
            };

            // Act

            using (var algorithm = SHA1.Create())
            {
                service.CalculateHashCodes(algorithm, collection);
            }

            //Assert

            Assert.AreEqual(fileHash1, entry1.CalculatedFileHash, $"Unexpected hash result for {nameof(entry1)}");
            Assert.AreEqual(fileHash2, entry2.CalculatedFileHash, $"Unexpected hash result for {nameof(entry2)}");
            Assert.AreEqual(fileHash3, entry3.CalculatedFileHash, $"Unexpected hash result for {nameof(entry3)}");
        }

        [Test]
        public void ServiceShowsProgressThroughInputListDuringBatchCalculation()
        {
            // Arrange

            var progressHistory = new List<string>();

            HashCodeBatchCalculationService batchCalculationService = null;
            var calculationService = new Mock<IHashCodeCalculationService>();

            calculationService.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), It.IsAny<string>()))
                .Callback(() => progressHistory.Add(batchCalculationService.ListProgress));

            batchCalculationService = new HashCodeBatchCalculationService(calculationService.Object);

            var collection = new[]
            {
                new InputFileListEntry("File1.txt"),
                new InputFileListEntry("File2.txt"),
                new InputFileListEntry("File3.txt")
            };

            // Act

            using (var algorithm = SHA1.Create())
            {
                batchCalculationService.CalculateHashCodes(algorithm, collection);
            }

            //Assert

            Assert.AreEqual(3, progressHistory.Count);

            Assert.AreEqual("1/3", progressHistory[0]);
            Assert.AreEqual("2/3", progressHistory[1]);
            Assert.AreEqual("3/3", progressHistory[2]);
        }

        [Test]
        public void ListProgressIsClearedAfterCalculation()
        {
            // Arrange

            var calculationService = new Mock<IHashCodeCalculationService>();
            var batchCalculationService = new HashCodeBatchCalculationService(calculationService.Object);

            var collection = new[]
            {
                new InputFileListEntry("File1.txt"),
                new InputFileListEntry("File2.txt"),
                new InputFileListEntry("File3.txt")
            };

            // Act

            using (var algorithm = SHA1.Create())
            {
                batchCalculationService.CalculateHashCodes(algorithm, collection);
            }

            //Assert

            Assert.IsTrue(string.IsNullOrWhiteSpace(batchCalculationService.ListProgress));
        }
    }
}
