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

using HashCalculator.Model;
using HashCalculator.ViewModel.Model;
using NUnit.Framework;
using System.Collections.Generic;
using HashCalculator.Service;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class FileHashCodeMatchCheckerTests
    {
        private const string UnexpectedFileNameMatchMessage = "Unexpected FileNameMatch Value";
        private const string UnexpectedHashCodeMatchMessage = "Unexpected HashCodeMatch Value";

        private static InputFileListEntry CreateInputFileListEntry(string filePath, string hashCode)
        {
            var inputFileListEntry = new InputFileListEntry(filePath)
            {
                CalculatedFileHash = hashCode
            };

            return inputFileListEntry;
        }

        [Test]
        public void FileNameMatchAndHashCodeMatchUsingFileName()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "Testfile.txt")
            };
            
            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, false);

            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMatchAndHashCodeMismatchUsingFileName()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("AnotherHashCode", "Testfile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, false);

            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMisMatchAndHashCodeMatchUsingFileName()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "AnotherFile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, false);

            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMisMatchAndHashCodeMismatchUsingFileName()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("AnotherHashCode", "AnotherFile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, false);

            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMatchAndHashCodeMatchUsingFullPath()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "X:\\Testfile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, true);

            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMatchAndHashCodeMismatchUsingFullPath()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("AnotherHashCode", "X:\\Testfile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, true);

            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMisMatchAndHashCodeMatchUsingFullPath()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "X:\\AnotherFile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, true);

            Assert.IsTrue(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }

        [Test]
        public void FileNameMisMatchAndHashCodeMismatchUsingFullPath()
        {
            var inputFileListEntry = CreateInputFileListEntry("X:\\Testfile.txt", "HashCode");

            var knownHashCodes = new List<FileHashMetadata>
            {
                new FileHashMetadata("AnotherHashCode", "X:\\AnotherFile.txt")
            };

            var checker = new FileHashCodeMatchChecker();
            var matchCriteria = checker.FindMatchCriteria(inputFileListEntry.HashMetadata, knownHashCodes, true);

            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.HashCodeMatch), UnexpectedHashCodeMatchMessage);
            Assert.IsFalse(matchCriteria.HasFlag(HashCodeMatchCriteria.FileNameMatch), UnexpectedFileNameMatchMessage);
        }
    }
}
