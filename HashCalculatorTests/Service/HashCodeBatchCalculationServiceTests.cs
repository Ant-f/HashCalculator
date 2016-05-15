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
using HashCalculator.ViewModel;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashCodeBatchCalculationServiceTests
    {
        private const string FileName1 = "File1.txt";
        private const string FileName2 = "File2.txt";
        private const string FileName3 = "File3.txt";

        private InputFileListEntry[] CreateTestingInputFileListEntryCollection()
        {
            var collection = new[]
            {
                new InputFileListEntry(FileName1),
                new InputFileListEntry(FileName2),
                new InputFileListEntry(FileName3)
            };
            return collection;
        }

        [Test]
        public void HashCodeCalculationServiceIsUsedWithEachInputFileListEntry()
        {
            // Arrange

            const string fileHash1 = "FileHash1";
            const string fileHash2 = "FileHash2";
            const string fileHash3 = "FileHash3";

            var calculationServiceMock = new Mock<IHashCodeCalculationService>();

            calculationServiceMock.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName1))
                .Returns(fileHash1);

            calculationServiceMock.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName2))
                .Returns(fileHash2);

            calculationServiceMock.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName3))
                .Returns(fileHash3);

            var service = new HashCodeBatchCalculationService(calculationServiceMock.Object);

            var collection = CreateTestingInputFileListEntryCollection();

            // Act

            service.CalculateHashCodes(HashAlgorithmSelection.SHA1, collection);

            //Assert

            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName1));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName2));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName3));

            const string unexpectedPathMessageTemplate = "Unexpected file path for collection index {0}";
            const string unexpectedHashMessageTemplate = "Unexpected hash result for collection index {0}";

            Assert.AreEqual(FileName1, collection[0].FilePath, string.Format(unexpectedPathMessageTemplate, 0));
            Assert.AreEqual(fileHash1, collection[0].CalculatedFileHash, string.Format(unexpectedHashMessageTemplate, 0));

            Assert.AreEqual(FileName2, collection[1].FilePath, string.Format(unexpectedPathMessageTemplate, 1));
            Assert.AreEqual(fileHash2, collection[1].CalculatedFileHash, string.Format(unexpectedHashMessageTemplate, 1));

            Assert.AreEqual(FileName3, collection[2].FilePath, string.Format(unexpectedPathMessageTemplate, 2));
            Assert.AreEqual(fileHash3, collection[2].CalculatedFileHash, string.Format(unexpectedHashMessageTemplate, 2));
        }

        [Test]
        public void ServiceShowsProgressThroughInputListDuringBatchCalculation()
        {
            // Arrange

            var progressHistory = new List<string>();

            HashCodeBatchCalculationService batchCalculationService = null;
            var calculationServiceMock = new Mock<IHashCodeCalculationService>();

            calculationServiceMock.Setup(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), It.IsAny<string>()))
                .Callback(() => progressHistory.Add(batchCalculationService.ListProgress));

            batchCalculationService = new HashCodeBatchCalculationService(calculationServiceMock.Object);

            var collection = CreateTestingInputFileListEntryCollection();

            // Act

            batchCalculationService.CalculateHashCodes(HashAlgorithmSelection.SHA1, collection);

            //Assert

            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName1));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName2));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName3));

            Assert.AreEqual(3, progressHistory.Count);

            Assert.AreEqual("1/3", progressHistory[0]);
            Assert.AreEqual("2/3", progressHistory[1]);
            Assert.AreEqual("3/3", progressHistory[2]);
        }

        [Test]
        public void ListProgressIsClearedAfterCalculation()
        {
            // Arrange

            var calculationServiceMock = new Mock<IHashCodeCalculationService>();

            calculationServiceMock.Setup(s => s.CalculateHashCodes(
                It.IsAny<HashAlgorithm>(),
                It.IsAny<string>()));

            var batchCalculationService = new HashCodeBatchCalculationService(calculationServiceMock.Object);

            var collection = CreateTestingInputFileListEntryCollection();

            // Act

            batchCalculationService.CalculateHashCodes(HashAlgorithmSelection.SHA1, collection);

            //Assert

            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName1));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName2));
            calculationServiceMock.Verify(s => s.CalculateHashCodes(It.IsAny<HashAlgorithm>(), FileName3));

            Assert.IsTrue(string.IsNullOrWhiteSpace(batchCalculationService.ListProgress));
        }

        [Test]
        public void InvalidAlgorithmNameDoesNotAttemptCalculation()
        {
            // Arrange

            var calculationServiceMock = new Mock<IHashCodeCalculationService>();
            var batchCalculationService = new HashCodeBatchCalculationService(calculationServiceMock.Object);
            var collection = CreateTestingInputFileListEntryCollection();

            // Act

            batchCalculationService.CalculateHashCodes("Not a hash algorithm name", collection);

            // Assert

            calculationServiceMock.Verify(s => s.CalculateHashCodes(
                It.IsAny<HashAlgorithm>(),
                It.IsAny<string>()), Times.Never);
        }
    }
}
