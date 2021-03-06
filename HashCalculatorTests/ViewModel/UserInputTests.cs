﻿// HashCalculator
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
using HashCalculator.Service;
using HashCalculator.ViewModel.Model;
using HashCalculatorTests.TestingInfrastructure;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HashCalculatorTests.ViewModel
{
    [TestFixture]
    public class UserInputTests
    {
        [Test]
        public void ChangingKnownFileHashCodesTextCausesInputFileListEntryHashMatchCriteriaUpdate()
        {
            // Arrange

            var builder = new UserInputBuilder();

            builder.FileHashCodeMatchCheckerMock.Setup(c => c.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch | HashCodeMatchCriteria.HashCodeMatch);

            var userInput = builder.CreateUserInput();

            string raisedPropertyName = null;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
            };

            var entry = new InputFileListEntry("Path");
            userInput.InputFileList.Add(entry);
            entry.PropertyChanged += eventHandler;

            // Act

            userInput.KnownFileHashCodesText = "new known hash codes";

            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileHashCodeMatchCheckerMock.VerifyAll();

            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }

        [Test]
        public void ChangingFullFilePathMatchingCausesInputFileListEntryHashMatchCriteriaUpdate()
        {
            // Arrange

            var builder = new UserInputBuilder();

            builder.FileHashCodeMatchCheckerMock.Setup(c => c.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch | HashCodeMatchCriteria.HashCodeMatch);

            var userInput = builder.CreateUserInput();
            userInput.KnownFileHashCodesText = "hash codes";

            string raisedPropertyName = null;
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
            };

            var entry = new InputFileListEntry("Path");
            userInput.InputFileList.Add(entry);
            entry.PropertyChanged += eventHandler;

            // Act

            userInput.MatchFullFilePath = true;

            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileHashCodeMatchCheckerMock.VerifyAll();

            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }

        [Test]
        public void ChangingKnownFileHashCodesTextCausesKnownFileHashCodesListToUpdateWithOneHashCode()
        {
            // Arrange

            const string hashCode = "HashCode";
            const string filePath = "x:\\filename.txt";

            var builder = new UserInputBuilder();
            var userInput = builder.CreateUserInput();

            // Act

            userInput.KnownFileHashCodesText = $"{hashCode}* {filePath}";

            // Assert

            Assert.AreEqual(1, userInput.KnownFileHashList.Count);
            Assert.AreEqual(hashCode, userInput.KnownFileHashList[0].FileHashCode);
            Assert.AreEqual(filePath, userInput.KnownFileHashList[0].FilePath);
        }

        [Test]
        public void ChangingKnownFileHashCodesTextCausesKnownFileHashCodesListToUpdateWithTwoHashCode()
        {
            // Arrange

            const string hashCode1 = "HashCode1";
            const string filePath1 = "x:\\filename1.txt";
            const string hashCode2 = "HashCode2";
            const string filePath2 = "x:\\filename2.txt";

            var builder = new UserInputBuilder();
            var userInput = builder.CreateUserInput();

            // Act

            userInput.KnownFileHashCodesText = $"{hashCode1}* {filePath1}\r\n{hashCode2}* {filePath2}";

            // Assert

            Assert.AreEqual(2, userInput.KnownFileHashList.Count);

            Assert.AreEqual(hashCode1, userInput.KnownFileHashList[0].FileHashCode);
            Assert.AreEqual(filePath1, userInput.KnownFileHashList[0].FilePath);

            Assert.AreEqual(hashCode2, userInput.KnownFileHashList[1].FileHashCode);
            Assert.AreEqual(filePath2, userInput.KnownFileHashList[1].FilePath);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void AddingEntryToInputFileListSetsFileExistence(
            bool initialFileExistsState, bool desiredFinalFileExistsState)
        {
            // Arrange

            NotifyCollectionChangedAction? raisedAction = null;

            NotifyCollectionChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedAction = args.Action;
            };

            var builder = new UserInputBuilder();

            builder.FileExistenceCheckerMock.Setup(c => c.Exists(It.IsAny<string>()))
                .Returns(desiredFinalFileExistsState);

            var userInput = builder.CreateUserInput();
            userInput.InputFileList.CollectionChanged += eventHandler;

            var entry = new InputFileListEntry("path")
            {
                FileExistsAtFilePath = initialFileExistsState
            };

            //Act

            userInput.InputFileList.Add(entry);

            userInput.InputFileList.CollectionChanged -= eventHandler;

            // Assert

            Assert.NotNull(raisedAction);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raisedAction);
            Assert.AreEqual(desiredFinalFileExistsState, entry.FileExistsAtFilePath);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void ReplacingEntryToInputFileListSetsFileExistence(
            bool initialFileExistsState, bool desiredFinalFileExistsState)
        {
            // Arrange

            NotifyCollectionChangedAction? raisedAction = null;

            NotifyCollectionChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedAction = args.Action;
            };

            var builder = new UserInputBuilder();

            builder.FileExistenceCheckerMock.Setup(c => c.Exists(It.IsAny<string>()))
                .Returns(desiredFinalFileExistsState);

            var originalEntry = new InputFileListEntry("path");

            var userInput = builder.CreateUserInput();
            userInput.InputFileList.Add(originalEntry);
            userInput.InputFileList.CollectionChanged += eventHandler;

            var newEntry = new InputFileListEntry("path2")
            {
                FileExistsAtFilePath = initialFileExistsState
            };

            //Act

            userInput.InputFileList[0] = newEntry;

            userInput.InputFileList.CollectionChanged -= eventHandler;

            // Assert

            Assert.NotNull(raisedAction);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, raisedAction);
            Assert.AreEqual(desiredFinalFileExistsState, newEntry.FileExistsAtFilePath);
        }

        [Test]
        public void AddingEntryToInputFileListSubscribesToEntryPropertyChangedEvents()
        {
            INotifyPropertyChanged subscribeObject = null;

            var builder = new UserInputBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Subscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => subscribeObject = itm);

            var userInput = builder.CreateUserInput();
            var entry = new InputFileListEntry("Path");

            userInput.InputFileList.Add(entry);

            builder.PropertyChangedSubscriberMock.VerifyAll();
            Assert.AreEqual(entry, subscribeObject);
        }

        [Test]
        public void RemovingEntryFromInputFileListUnsubscribesFromEntryPropertyChangedEvents()
        {
            INotifyPropertyChanged unsubscribeObject = null;

            var builder = new UserInputBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Unsubscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => unsubscribeObject = itm);

            var userInput = builder.CreateUserInput();
            var entry = new InputFileListEntry("Path");

            userInput.InputFileList.Add(entry);
            userInput.InputFileList.Remove(entry);

            builder.PropertyChangedSubscriberMock.VerifyAll();
            Assert.AreEqual(entry, unsubscribeObject);
        }

        [Test]
        public void ReplacingEntryToInputFileListSubscribesToNewEntryPropertyChangedEventsAndUnsubscribesFromOldEntryPropertyChangedEvents()
        {
            // Arrange

            INotifyPropertyChanged subscribeObject;
            INotifyPropertyChanged unsubscribeObject;

            var builder = new UserInputBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Subscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => subscribeObject = itm);

            builder.PropertyChangedSubscriberMock.Setup(p => p.Unsubscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => unsubscribeObject = itm);

            var userInput = builder.CreateUserInput();
            var originalEntry = new InputFileListEntry("Path");

            userInput.InputFileList.Add(originalEntry);

            subscribeObject = null;
            unsubscribeObject = null;

            var newEntry = new InputFileListEntry("Path");

            // Act

            userInput.InputFileList[0] = newEntry;

            // Assert

            builder.PropertyChangedSubscriberMock.VerifyAll();

            Assert.AreEqual(originalEntry, unsubscribeObject);
            Assert.AreEqual(newEntry, subscribeObject);
        }

        [Test]
        public void ClearingInputFileListUnsubscribesFromPropertyChangedEventsOnAllEntries()
        {
            // Arrange

            var builder = new UserInputBuilder();

            var userInput = builder.CreateUserInput();
            var entry = new InputFileListEntry("Path");

            userInput.InputFileList.Add(entry);

            // Act

            userInput.InputFileList.Clear();

            // Assert

            builder.PropertyChangedSubscriberMock.Verify(p => p.UnsubscribeAll());
        }

        [Test]
        public void UpdatingEntryInInputFileListToNewFileNameSetsFileExistence()
        {
            // Arrange

            var updatedPropertyNames = new HashSet<string>();

            var entry = new InputFileListEntry("FilePath.txt");

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);
            };

            var builder = new UserInputBuilder
            {
                PropertyChangedSubscriber = new PropertyChangedSubscriber()
            };

            builder.FileExistenceCheckerMock
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            var userInput = builder.CreateUserInput();
            userInput.InputFileList.Add(entry);
            // Subscribe to PropertyChanged and set FileExistsAtFilePath to false
            // after adding entry to InputFileList: adding an entry will evaluate
            // file existence
            entry.PropertyChanged += eventHandler;
            entry.FileExistsAtFilePath = false;

            // Act

            entry.FilePath = "NewFilePath.txt";

            entry.PropertyChanged -= eventHandler;
            builder.PropertyChangedSubscriber.UnsubscribeAll();

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            Assert.IsTrue(entry.FileExistsAtFilePath);

            CollectionAssert.Contains(
                updatedPropertyNames,
                nameof(entry.FileExistsAtFilePath));

            CollectionAssert.Contains(
                updatedPropertyNames,
                nameof(entry.FilePath));
        }

        [Test]
        public void UpdatingEntryInInputFileListToNewFileNameSetsHashCodeMatch()
        {
            // Arrange

            var updatedPropertyNames = new HashSet<string>();

            var entry = new InputFileListEntry("FilePath.txt")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);
            };

            var builder = new UserInputBuilder
            {
                PropertyChangedSubscriber = new PropertyChangedSubscriber()
            };

            builder.FileHashCodeMatchCheckerMock
                .Setup(f => f.FindMatchCriteria(
                    It.IsAny<FileHashMetadata>(),
                    It.IsAny<List<FileHashMetadata>>(),
                    It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch);

            var userInput = builder.CreateUserInput();
            entry.PropertyChanged += eventHandler;
            userInput.InputFileList.Add(entry);

            // Act

            entry.FilePath = "NewFilePath.txt";

            entry.PropertyChanged -= eventHandler;
            builder.PropertyChangedSubscriber.UnsubscribeAll();

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            Assert.AreEqual(
                HashCodeMatchCriteria.FileNameMatch,
                entry.HashCodeMatch);

            CollectionAssert.Contains(
                updatedPropertyNames,
                nameof(entry.FilePath));

            CollectionAssert.Contains(
                updatedPropertyNames,
                nameof(entry.HashCodeMatch));
        }

        [Test]
        public void UpdatingEntryInInputFileListToHaveEmptyFilePathRemovesEntry()
        {
            // Arrange

            var updatedPropertyNames = new HashSet<string>();

            var entry = new InputFileListEntry("FilePath.txt")
            {
                FileExistsAtFilePath = false
            };

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);
            };

            var builder = new UserInputBuilder();

            builder.FileExistenceCheckerMock.Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            builder.PropertyChangedSubscriber = new PropertyChangedSubscriber();

            var userInput = builder.CreateUserInput();
            entry.PropertyChanged += eventHandler;
            userInput.InputFileList.Add(entry);

            // Act

            entry.FilePath = string.Empty;

            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            CollectionAssert.DoesNotContain(userInput.InputFileList, entry);
            CollectionAssert.Contains(updatedPropertyNames, nameof(entry.FileExistsAtFilePath));
            CollectionAssert.Contains(updatedPropertyNames, nameof(entry.FilePath));
        }

        [Test]
        public void UpdatingEntryInInputFileListToHaveEmptyFilePathDoesNotSetsHashCodeMatch()
        {
            // Arrange
            
            var updatedPropertyNames = new HashSet<string>();

            var entry = new InputFileListEntry("FilePath.txt");

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);
            };

            var builder = new UserInputBuilder
            {
                PropertyChangedSubscriber = new PropertyChangedSubscriber()
            };

            var userInput = builder.CreateUserInput();
            entry.PropertyChanged += eventHandler;
            userInput.InputFileList.Add(entry);

            // Act

            entry.FilePath = string.Empty;

            entry.PropertyChanged -= eventHandler;
            builder.PropertyChangedSubscriber.UnsubscribeAll();

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            builder.FileHashCodeMatchCheckerMock.Verify(f => f.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()),
                Times.Never);

            CollectionAssert.Contains(updatedPropertyNames, nameof(entry.FilePath));
        }
    }
}
