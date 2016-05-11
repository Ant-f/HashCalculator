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
using System.Threading;
using static HashCalculatorTests.UnitTestConstants;

namespace HashCalculatorTests.ViewModel.Model
{
    [TestFixture]
    public class InputFileListEntryTests
    {
        [Test]
        public void SettingFilePathToSameValueDoesNotRaisePropertyChanged()
        {
            var propertyChangedRaised = false;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                propertyChangedRaised = true;
                resetEvent.Set();
            };
            
            var entry = new InputFileListEntry("Path");
            entry.PropertyChanged += eventHandler;

            entry.FilePath = "Path";

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            Assert.IsTrue(timeout);
            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingFilePathToDifferentValueDoesRaisePropertyChanged()
        {
            string raisedPropertyName = null;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
                resetEvent.Set();
            };

            var entry = new InputFileListEntry("Path");
            entry.PropertyChanged += eventHandler;

            entry.FilePath = "NewPath";

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            Assert.IsFalse(timeout);
            Assert.AreEqual(nameof(entry.FilePath), raisedPropertyName);
        }

        [Test]
        public void SettingHashCodeMatchToSameValueDoesNotRaisePropertyChanged()
        {
            var propertyChangedRaised = false;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                propertyChangedRaised = true;
                resetEvent.Set();
            };

            var entry = new InputFileListEntry("Path")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };
            entry.PropertyChanged += eventHandler;

            entry.HashCodeMatch = HashCodeMatchCriteria.None;

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            Assert.IsTrue(timeout);
            Assert.IsFalse(propertyChangedRaised);
        }

        [Test]
        public void SettingHashCodeMatchToDifferentValueDoesRaisePropertyChanged()
        {
            string raisedPropertyName = null;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
                resetEvent.Set();
            };

            var entry = new InputFileListEntry("Path")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };
            entry.PropertyChanged += eventHandler;

            entry.HashCodeMatch = HashCodeMatchCriteria.FileNameMatch;

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            Assert.IsFalse(timeout);
            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }
    }
}
