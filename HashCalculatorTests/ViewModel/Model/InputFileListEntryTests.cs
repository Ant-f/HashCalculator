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

using HashCalculator.ViewModel.Model;
using NUnit.Framework;
using System.ComponentModel;

namespace HashCalculatorTests.ViewModel.Model
{
    [TestFixture]
    public class InputFileListEntryTests
    {
        [Test]
        public void SettingFilePathToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            const string path = "path";

            var propertyChangedRaised = false;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                propertyChangedRaised = true;
            };
            
            var entry = new InputFileListEntry(path);
            entry.PropertyChanged += eventHandler;

            // Act

            entry.FilePath = path;

            entry.PropertyChanged -= eventHandler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingFilePathToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            string raisedPropertyName = null;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
            };

            var entry = new InputFileListEntry("Path");
            entry.PropertyChanged += eventHandler;

            // Act

            entry.FilePath = "NewPath";

            entry.PropertyChanged -= eventHandler;

            // Assert

            Assert.AreEqual(nameof(entry.FilePath), raisedPropertyName);
        }

        [Test]
        public void SettingHashCodeMatchToSameValueDoesNotRaisePropertyChanged()
        {
            // Arrange

            var propertyChangedRaised = false;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                propertyChangedRaised = true;
            };

            var entry = new InputFileListEntry("Path")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };
            entry.PropertyChanged += eventHandler;

            // Act

            entry.HashCodeMatch = HashCodeMatchCriteria.None;

            entry.PropertyChanged -= eventHandler;

            // Assert

            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingHashCodeMatchToDifferentValueRaisesPropertyChanged()
        {
            // Arrange

            string raisedPropertyName = null;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
            };

            var entry = new InputFileListEntry("Path")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };
            entry.PropertyChanged += eventHandler;

            // Act

            entry.HashCodeMatch = HashCodeMatchCriteria.FileNameMatch;

            entry.PropertyChanged -= eventHandler;

            // Assert

            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }

        [Test]
        public void SettingFilePathSetsHashMetadataFilePath()
        {
            // Arrange

            const string newPath = "newPath";

            var input = new InputFileListEntry(string.Empty);

            // Act

            input.FilePath = newPath;

            // Assert

            Assert.AreEqual(newPath, input.HashMetadata.FilePath);
        }
    }
}
