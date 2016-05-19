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
using HashCalculatorTests.TestingInfrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using HashCalculator.Service;
using static HashCalculatorTests.UnitTestConstants;

namespace HashCalculatorTests.ViewModel
{
    [TestFixture]
    public class HashCalculatorViewModelTests
    {
        [Test]
        public void ChangingViewModelKnownFileHashCodesTextCausesInputFileListEntryHashMatchCriteriaUpdate()
        {
            // Arrange

            var builder = new HashCalculatorViewModelBuilder();

            builder.FileHashCodeMatchCheckerMock.Setup(c => c.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch | HashCodeMatchCriteria.HashCodeMatch);

            var viewModel = builder.CreateViewModel();

            string raisedPropertyName = null;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
                resetEvent.Set();
            };

            var entry = new InputFileListEntry("Path");
            viewModel.InputFileList.Add(entry);
            entry.PropertyChanged += eventHandler;

            // Act

            viewModel.KnownFileHashCodesText = "new known hash codes";

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileHashCodeMatchCheckerMock.VerifyAll();

            Assert.IsFalse(timeout);
            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }

        [Test]
        public void ChangingViewModelFullFilePathMatchingCausesInputFileListEntryHashMatchCriteriaUpdate()
        {
            // Arrange

            var builder = new HashCalculatorViewModelBuilder();

            builder.FileHashCodeMatchCheckerMock.Setup(c => c.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch | HashCodeMatchCriteria.HashCodeMatch);

            var viewModel = builder.CreateViewModel();
            viewModel.KnownFileHashCodesText = "hash codes";

            string raisedPropertyName = null;
            var resetEvent = new ManualResetEvent(false);
            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedPropertyName = args.PropertyName;
                resetEvent.Set();
            };

            var entry = new InputFileListEntry("Path");
            viewModel.InputFileList.Add(entry);
            entry.PropertyChanged += eventHandler;

            // Act

            viewModel.MatchFullFilePath = true;

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileHashCodeMatchCheckerMock.VerifyAll();

            Assert.IsFalse(timeout);
            Assert.AreEqual(nameof(entry.HashCodeMatch), raisedPropertyName);
        }

        [Test]
        public void ChangingViewModelKnownFileHashCodesTextCausesKnownFileHashCodesListToUpdateWithOneHashCode()
        {
            // Arrange

            const string hashCode = "HashCode";
            const string filePath = "x:\\filename.txt";

            var builder = new HashCalculatorViewModelBuilder();
            var viewModel = builder.CreateViewModel();

            // Act

            viewModel.KnownFileHashCodesText = $"{hashCode}* {filePath}";

            // Assert

            Assert.AreEqual(1, viewModel.KnownFileHashList.Count);
            Assert.AreEqual(hashCode, viewModel.KnownFileHashList[0].FileHashCode);
            Assert.AreEqual(filePath, viewModel.KnownFileHashList[0].FilePath);
        }

        [Test]
        public void ChangingViewModelKnownFileHashCodesTextCausesKnownFileHashCodesListToUpdateWithTwoHashCode()
        {
            // Arrange

            const string hashCode1 = "HashCode1";
            const string filePath1 = "x:\\filename1.txt";
            const string hashCode2 = "HashCode2";
            const string filePath2 = "x:\\filename2.txt";

            var builder = new HashCalculatorViewModelBuilder();
            var viewModel = builder.CreateViewModel();

            // Act

            viewModel.KnownFileHashCodesText = $"{hashCode1}* {filePath1}\r\n{hashCode2}* {filePath2}";

            // Assert

            Assert.AreEqual(2, viewModel.KnownFileHashList.Count);

            Assert.AreEqual(hashCode1, viewModel.KnownFileHashList[0].FileHashCode);
            Assert.AreEqual(filePath1, viewModel.KnownFileHashList[0].FilePath);

            Assert.AreEqual(hashCode2, viewModel.KnownFileHashList[1].FileHashCode);
            Assert.AreEqual(filePath2, viewModel.KnownFileHashList[1].FilePath);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void AddingEntryToInputFileListSetsFileExistence(
            bool initialFileExistsState, bool desiredFinalFileExistsState)
        {
            // Arrange

            NotifyCollectionChangedAction? raisedAction = null;
            var resetEvent = new ManualResetEvent(false);

            NotifyCollectionChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedAction = args.Action;
                resetEvent.Set();
            };

            var builder = new HashCalculatorViewModelBuilder();

            builder.FileExistenceCheckerMock.Setup(c => c.Exists(It.IsAny<string>()))
                .Returns(desiredFinalFileExistsState);

            var viewModel = builder.CreateViewModel();
            viewModel.InputFileList.CollectionChanged += eventHandler;

            var entry = new InputFileListEntry("path")
            {
                FileExistsAtFilePath = initialFileExistsState
            };

            //Act

            viewModel.InputFileList.Add(entry);

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            viewModel.InputFileList.CollectionChanged -= eventHandler;

            // Assert

            Assert.IsFalse(timeout);
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
            var resetEvent = new ManualResetEvent(false);

            NotifyCollectionChangedEventHandler eventHandler = (sender, args) =>
            {
                raisedAction = args.Action;
                resetEvent.Set();
            };

            var builder = new HashCalculatorViewModelBuilder();

            builder.FileExistenceCheckerMock.Setup(c => c.Exists(It.IsAny<string>()))
                .Returns(desiredFinalFileExistsState);

            var originalEntry = new InputFileListEntry("path");

            var viewModel = builder.CreateViewModel();
            viewModel.InputFileList.Add(originalEntry);
            viewModel.InputFileList.CollectionChanged += eventHandler;

            var newEntry = new InputFileListEntry("path2")
            {
                FileExistsAtFilePath = initialFileExistsState
            };

            //Act

            viewModel.InputFileList[0] = newEntry;

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            viewModel.InputFileList.CollectionChanged -= eventHandler;

            // Assert

            Assert.IsFalse(timeout);
            Assert.NotNull(raisedAction);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, raisedAction);
            Assert.AreEqual(desiredFinalFileExistsState, newEntry.FileExistsAtFilePath);
        }

        [Test]
        public void AddingEntryToInputFileListSubscribesToEntryPropertyChangedEvents()
        {
            INotifyPropertyChanged subscribeObject = null;

            var builder = new HashCalculatorViewModelBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Subscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => subscribeObject = itm);

            var viewModel = builder.CreateViewModel();
            var entry = new InputFileListEntry("Path");

            viewModel.InputFileList.Add(entry);

            builder.PropertyChangedSubscriberMock.VerifyAll();
            Assert.AreEqual(entry, subscribeObject);
        }

        [Test]
        public void RemovingEntryToInputFileListUnsubscribesFromEntryPropertyChangedEvents()
        {
            INotifyPropertyChanged unsubscribeObject = null;

            var builder = new HashCalculatorViewModelBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Unsubscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => unsubscribeObject = itm);

            var viewModel = builder.CreateViewModel();
            var entry = new InputFileListEntry("Path");

            viewModel.InputFileList.Add(entry);
            viewModel.InputFileList.Remove(entry);

            builder.PropertyChangedSubscriberMock.VerifyAll();
            Assert.AreEqual(entry, unsubscribeObject);
        }

        [Test]
        public void ReplacingEntryToInputFileListSubscribesToNewEntryPropertyChangedEventsAndUnsubscribesFromOldEntryPropertyChangedEvents()
        {
            // Arrange

            INotifyPropertyChanged subscribeObject;
            INotifyPropertyChanged unsubscribeObject;

            var builder = new HashCalculatorViewModelBuilder();

            builder.PropertyChangedSubscriberMock.Setup(p => p.Subscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => subscribeObject = itm);

            builder.PropertyChangedSubscriberMock.Setup(p => p.Unsubscribe(
                It.IsAny<INotifyPropertyChanged>()))
                .Callback<INotifyPropertyChanged>(itm => unsubscribeObject = itm);

            var viewModel = builder.CreateViewModel();
            var originalEntry = new InputFileListEntry("Path");

            viewModel.InputFileList.Add(originalEntry);

            subscribeObject = null;
            unsubscribeObject = null;

            var newEntry = new InputFileListEntry("Path");

            // Act

            viewModel.InputFileList[0] = newEntry;

            // Assert

            builder.PropertyChangedSubscriberMock.VerifyAll();

            Assert.AreEqual(originalEntry, unsubscribeObject);
            Assert.AreEqual(newEntry, subscribeObject);
        }

        [Test]
        public void ClearingInputFileListUnsubscribesFromPropertyChangedEventsOnAllEntries()
        {
            // Arrange

            var builder = new HashCalculatorViewModelBuilder();

            var viewModel = builder.CreateViewModel();
            var entry = new InputFileListEntry("Path");

            viewModel.InputFileList.Add(entry);

            // Act

            viewModel.InputFileList.Clear();

            // Assert

            builder.PropertyChangedSubscriberMock.Verify(p => p.UnsubscribeAll());
        }

        [Test]
        public void UpdatingEntryInInputFileListSetsFileExistence()
        {
            // Arrange

            var updatedPropertyNames = new HashSet<string>();
            var resetEvent = new ManualResetEvent(false);

            var entry = new InputFileListEntry("FilePath.txt")
            {
                FileExistsAtFilePath = false
            };

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);

                if (updatedPropertyNames.Contains(nameof(entry.FilePath)) &&
                    updatedPropertyNames.Contains(nameof(entry.FileExistsAtFilePath)))
                {
                    resetEvent.Set();
                }
            };

            var builder = new HashCalculatorViewModelBuilder();

            builder.FileExistenceCheckerMock.Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            var viewModel = builder.CreateViewModel();
            entry.PropertyChanged += eventHandler;
            viewModel.InputFileList.Add(entry);

            // Act

            entry.FilePath = "NewFilePath.txt";

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            Assert.IsFalse(timeout);
            Assert.IsTrue(entry.FileExistsAtFilePath);
        }

        [Test]
        public void UpdatingEntryInInputFileListSetsHashCodeMatch()
        {
            // Arrange
            
            var updatedPropertyNames = new HashSet<string>();
            var resetEvent = new ManualResetEvent(false);

            var entry = new InputFileListEntry("FilePath.txt")
            {
                HashCodeMatch = HashCodeMatchCriteria.None
            };

            PropertyChangedEventHandler eventHandler = (sender, args) =>
            {
                updatedPropertyNames.Add(args.PropertyName);

                if (updatedPropertyNames.Contains(nameof(entry.FilePath)) &&
                    updatedPropertyNames.Contains(nameof(entry.HashCodeMatch)))
                {
                    resetEvent.Set();
                }
            };

            var builder = new HashCalculatorViewModelBuilder
            {
                PropertyChangedSubscriber = new PropertyChangedSubscriber()
            };

            builder.FileHashCodeMatchCheckerMock.Setup(f => f.FindMatchCriteria(
                It.IsAny<FileHashMetadata>(),
                It.IsAny<List<FileHashMetadata>>(),
                It.IsAny<bool>()))
                .Returns(HashCodeMatchCriteria.FileNameMatch);

            var viewModel = builder.CreateViewModel();
            entry.PropertyChanged += eventHandler;
            viewModel.InputFileList.Add(entry);

            // Act

            entry.FilePath = "NewFilePath.txt";

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            entry.PropertyChanged -= eventHandler;
            builder.PropertyChangedSubscriber.UnsubscribeAll();

            // Assert

            builder.FileExistenceCheckerMock.VerifyAll();

            Assert.IsFalse(timeout);
            Assert.AreEqual(HashCodeMatchCriteria.FileNameMatch, entry.HashCodeMatch);
        }

        [Test]
        public void SettingHashCalculationIsRunningRaisesExportHashListCommandCanExecuteChanged()
        {
            // Arrange
            
            var resetEvent = new ManualResetEvent(false);

            var canExecuteRaised = false;
            var eventHandler = new EventHandler((sender, args) =>
            {
                canExecuteRaised = true;
                resetEvent.Set();
            });

            var builder = new HashCalculatorViewModelBuilder();

            builder.DispatcherServiceMock.Setup(h => h.BeginInvoke(It.IsAny<Delegate>()))
                .Callback<Delegate>(method => method.DynamicInvoke());

            var viewModel = builder.CreateViewModel();
            viewModel.HashCalculationIsRunning = false;
            viewModel.ExportHashListCommand.EvaluateCanExecutePredicate(null);
            viewModel.ExportHashListCommand.CanExecuteChanged += eventHandler;

            // Act

            viewModel.HashCalculationIsRunning = true;

            var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            viewModel.ExportHashListCommand.CanExecuteChanged -= eventHandler;

            // Assert

            builder.DispatcherServiceMock.VerifyAll();

            Assert.IsFalse(timeout);
            Assert.IsTrue(canExecuteRaised);
        }

        [Test]
        public void ExportCommandCallsHashCodeExporter()
        {
            // Arrange

            const string exportPath = "ExportPath";

            var builder = new HashCalculatorViewModelBuilder();

            builder.ExportPathPrompterMock.Setup(e => e.ShowPrompt())
                .Returns(exportPath);

            var viewModel = builder.CreateViewModel();

            // Act

            viewModel.ExportHashListCommand.Execute(null);

            //Assert

            builder.ExportPathPrompterMock.VerifyAll();
            builder.HashCodeExporterMock.Verify(e => e.Export(
                exportPath,
                It.IsAny<IList<FileHashMetadata>>(),
                It.IsAny<bool>()));
        }

        [Test]
        public void CalculateHashCodesCommandUsesHashCodeBatchCalculationService()
        {
            // Arrange

            var builder = new HashCalculatorViewModelBuilder();
            var viewModel = builder.CreateViewModel();

            // Act

            viewModel.CalculateHashCodesCommand.Execute(null);

            // Assert

            builder.HashCodeBatchCalculationServiceMock.Verify(s => s.CalculateHashCodes(
                It.IsAny<string>(),
                It.IsAny<IList<InputFileListEntry>>()));
        }
    }
}
