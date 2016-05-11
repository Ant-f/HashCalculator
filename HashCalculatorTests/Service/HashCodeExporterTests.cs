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
using HashCalculatorTests.TestingInfrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashCodeExporterTests
    {
        private const string ExportPath = "X:\\ExportedFile.txt";

        [Test]
        public void SingleMetadataEntryIsFormattedCorrectlyWithFullFilePath()
        {
            // Arrange

            var metadataList = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "X:\\FileToGetHashCodeFrom.txt")
            };

            var builder = new HashCodeExporterBuilder();
            var exporter = builder.CreateHashCodeExporter();

            // Act

            exporter.Export(ExportPath, metadataList, false);

            // Assert

            builder.FileCreatorMock.Verify(c => c.CreateTextFile(ExportPath));

            Assert.AreEqual(
                "HashCode *X:\\FileToGetHashCodeFrom.txt" + Environment.NewLine,
                builder.StreamWriter.WrittenText);
        }

        [Test]
        public void TwoMetadataEntriesAreFormattedCorrectlyWithFullFilePath()
        {
            // Arrange

            var metadataList = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode1", "X:\\FileToGetHashCodeFrom1.txt"),
                new FileHashMetadata("HashCode2", "X:\\FileToGetHashCodeFrom2.txt")
            };

            var builder = new HashCodeExporterBuilder();
            var exporter = builder.CreateHashCodeExporter();

            // Act

            exporter.Export(ExportPath, metadataList, false);

            // Assert

            builder.FileCreatorMock.Verify(c => c.CreateTextFile(ExportPath));

            Assert.AreEqual(
                "HashCode1 *X:\\FileToGetHashCodeFrom1.txt" + Environment.NewLine +
                "HashCode2 *X:\\FileToGetHashCodeFrom2.txt" + Environment.NewLine,
                builder.StreamWriter.WrittenText);
        }

        [Test]
        public void SingleMetadataEntryIsFormattedCorrectlyWithFileNameOnly()
        {
            // Arrange

            var metadataList = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode", "X:\\FileToGetHashCodeFrom.txt")
            };

            var builder = new HashCodeExporterBuilder();
            var exporter = builder.CreateHashCodeExporter();

            // Act

            exporter.Export(ExportPath, metadataList, true);

            // Assert

            builder.FileCreatorMock.Verify(c => c.CreateTextFile(ExportPath));

            Assert.AreEqual(
                "HashCode *FileToGetHashCodeFrom.txt" + Environment.NewLine,
                builder.StreamWriter.WrittenText);
        }

        [Test]
        public void TwoMetadataEntriesAreFormattedCorrectlyWithFileNameOnly()
        {
            // Arrange

            var metadataList = new List<FileHashMetadata>
            {
                new FileHashMetadata("HashCode1", "X:\\FileToGetHashCodeFrom1.txt"),
                new FileHashMetadata("HashCode2", "X:\\FileToGetHashCodeFrom2.txt")
            };

            var builder = new HashCodeExporterBuilder();
            var exporter = builder.CreateHashCodeExporter();

            // Act

            exporter.Export(ExportPath, metadataList, true);

            // Assert

            builder.FileCreatorMock.Verify(c => c.CreateTextFile(ExportPath));

            Assert.AreEqual(
                "HashCode1 *FileToGetHashCodeFrom1.txt" + Environment.NewLine +
                "HashCode2 *FileToGetHashCodeFrom2.txt" + Environment.NewLine,
                builder.StreamWriter.WrittenText);
        }
    }
}
