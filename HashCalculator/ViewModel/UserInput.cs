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
using HashCalculator.Model;
using HashCalculator.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace HashCalculator.ViewModel
{
    public class UserInput : IUserInput, IInputFilesAppender
    {
        private static readonly char[] NewLineSeparators = { '\r', '\n' };
        private static readonly char[] HashSeparators = { '*' };

        private readonly IFileExistenceChecker _fileExistenceChecker;
        private readonly IFileHashCodeMatchChecker _fileHashCodeMatchChecker;
        private readonly IPropertyChangedSubscriber _propertyChangedSubscriber;

        private string _knownFileHashCodesText = string.Empty;
        private bool _matchFullFilePath = false;

        /// <summary>
        /// Each file in this collection will have its hash code calculated
        /// </summary>
        public ObservableCollection<InputFileListEntry> InputFileList { get; }

        /// <summary>
        /// A list representing known hash codes of files. Calculated hash codes
        /// will be compared against entries in this list when determining
        /// whether hash codes match known values.
        /// </summary>
        public List<FileHashMetadata> KnownFileHashList { get; } = new List<FileHashMetadata>();

        /// <summary>
        /// New-line delimited string representing known hash codes of files.
        /// <see cref="KnownFileHashList"/> is built from this string.
        /// </summary>
        public string KnownFileHashCodesText
        {
            get
            {
                return _knownFileHashCodesText;
            }

            set
            {
                if (_knownFileHashCodesText != value)
                {
                    _knownFileHashCodesText = value;
                    RepopulateKnownFileHashList();
                    EvaluateInputFileListHashCodeMatchCriteria();
                }
            }
        }

        /// <summary>
        /// Specifies whether to use a file's full path, or its name and
        /// extension only when determining a file's identity
        /// </summary>
        public bool MatchFullFilePath
        {
            get
            {
                return _matchFullFilePath;
            }

            set
            {
                if (_matchFullFilePath != value)
                {
                    _matchFullFilePath = value;
                    RepopulateKnownFileHashList();
                    EvaluateInputFileListHashCodeMatchCriteria();
                }
            }
        }

        public UserInput(
            IFileHashCodeMatchChecker fileHashCodeMatchChecker,
            IFileExistenceChecker fileExistenceChecker,
            IPropertyChangedSubscriber propertyChangedSubscriber)
        {
            _fileExistenceChecker = fileExistenceChecker;
            _fileHashCodeMatchChecker = fileHashCodeMatchChecker;

            _propertyChangedSubscriber = propertyChangedSubscriber;
            _propertyChangedSubscriber.EventHandler = InputFileListEntryPropertyChanged;

            InputFileList = new ObservableCollection<InputFileListEntry>();
            InputFileList.CollectionChanged += InputFileListCollectionChanged;
        }

        /// <summary>
        /// <see cref="PropertyChangedEventHandler"/> that is invoked every
        /// time that a property value in <see cref="InputFileListEntry"/> is
        /// changed, as identified by
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> being raised.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="InputFileListEntry"/> that raised the
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// </param>
        /// <param name="e">
        /// <see cref="PropertyChangedEventArgs"/> associated with the event
        /// </param>
        private void InputFileListEntryPropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            var entry = sender as InputFileListEntry;
            if (entry == null)
            {
                return;
            }

            var emptyPath = string.IsNullOrWhiteSpace(entry.FilePath);
            if (emptyPath)
            {
                InputFileList.Remove(entry);
            }
            else
            {
                entry.FileExistsAtFilePath = _fileExistenceChecker.Exists(entry.FilePath);
                entry.HashCodeMatch = CheckFileHashMatch(entry.HashMetadata);
            }
        }

        /// <summary>
        /// <see cref="NotifyCollectionChangedEventHandler"/> that is invoked
        /// when items are added to/removed from <see cref="InputFileList"/>
        /// </summary>
        /// <param name="sender">
        /// The collection that raised the event, i.e.
        /// <see cref="InputFileList"/>
        /// </param>
        /// <param name="e">
        /// <see cref="NotifyCollectionChangedEventArgs"/> associated with the event
        /// </param>
        private void InputFileListCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<InputFileListEntry>().ToArray();
            var oldItems = e.OldItems?.Cast<InputFileListEntry>().ToArray();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    EvaluateFileExistence(newItems);
                    SubscribeToPropertyChanged(newItems);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    EvaluateFileExistence(newItems);
                    SubscribeToPropertyChanged(newItems);
                    UnsubscribeToPropertyChanged(oldItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UnsubscribeToPropertyChanged(oldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _propertyChangedSubscriber.UnsubscribeAll();
                    break;
            }
        }

        /// <summary>
        /// Subscribe to <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// events raised when the properties of the passed in items update, and
        /// handle them with <see cref="InputFileListEntryPropertyChanged"/>
        /// </summary>
        /// <param name="items">
        /// Items to which <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// events will be subscribed to
        /// </param>
        private void SubscribeToPropertyChanged(IEnumerable<INotifyPropertyChanged> items)
        {
            foreach (var entry in items)
            {
                _propertyChangedSubscriber.Subscribe(entry);
            }
        }

        /// <summary>
        /// Unsubscribe from <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// events raised by the specified items.
        /// </summary>
        /// <param name="items">
        /// Items from which to unsubscribed from
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> events
        /// </param>
        private void UnsubscribeToPropertyChanged(IEnumerable<INotifyPropertyChanged> items)
        {
            foreach (var entry in items)
            {
                _propertyChangedSubscriber.Unsubscribe(entry);
            }
        }

        /// <summary>
        /// Evaluates whether a file exists at the path specified in each
        /// provided <see cref="InputFileListEntry"/> and sets
        /// <see cref="InputFileListEntry.FileExistsAtFilePath"/> accordingly
        /// </summary>
        /// <param name="listEntries">
        /// A colleciton of <see cref="InputFileListEntry"/> for which to evaluate
        /// and set <see cref="InputFileListEntry.FileExistsAtFilePath"/>
        /// </param>
        private void EvaluateFileExistence(IEnumerable<InputFileListEntry> listEntries)
        {
            foreach (var listEntry in listEntries)
            {
                listEntry.FileExistsAtFilePath = _fileExistenceChecker.Exists(listEntry.FilePath);
            }
        }

        /// <summary>
        /// Clear and repopulate <see cref="KnownFileHashList"/>
        /// </summary>
        private void RepopulateKnownFileHashList()
        {
            var knownFileHashSums = KnownFileHashCodesText.Split(NewLineSeparators, StringSplitOptions.RemoveEmptyEntries);

            KnownFileHashList.Clear();

            foreach (string entry in knownFileHashSums)
            {
                var tokens = entry.Split(HashSeparators, StringSplitOptions.RemoveEmptyEntries);
                var hash = tokens[0].Trim();
                var fileName = tokens.Length > 1 ? tokens[1].Trim() : string.Empty;
                var metadata = new FileHashMetadata
                {
                    FilePath = fileName,
                    FileHashCode = hash
                };
                KnownFileHashList.Add(metadata);
            }
        }

        /// <summary>
        /// Set <see cref="InputFileListEntry.HashCodeMatch"/> for each
        /// <see cref="InputFileListEntry"/> in <see cref="InputFileList"/> to
        /// accurately represent whether a match exists for either file paths
        /// and/or hash codes
        /// </summary>
        private void EvaluateInputFileListHashCodeMatchCriteria()
        {
            foreach (var listEntry in InputFileList)
            {
                listEntry.HashCodeMatch = CheckFileHashMatch(listEntry.HashMetadata);
            }
        }

        /// <summary>
        /// Evaluate whether a match exists for either file path and/or hash
        /// code between the provided <see cref="FileHashMetadata"/>, and data
        /// within <see cref="KnownFileHashList"/> 
        /// </summary>
        /// <param name="metadata">
        /// The item to evaluate file path and hash code against data in
        /// <see cref="KnownFileHashList"/>
        /// </param>
        /// <returns>
        /// A <see cref="HashCodeMatchCriteria"/> representing a match for file
        /// path and/or hash code
        /// </returns>
        private HashCodeMatchCriteria CheckFileHashMatch(FileHashMetadata metadata)
        {
            var matchCriteria = _fileHashCodeMatchChecker.FindMatchCriteria(
                metadata,
                KnownFileHashList,
                MatchFullFilePath);

            return matchCriteria;
        }

        /// <summary>
        /// Add to <see cref="InputFileList"/>. An <see cref="InputFileListEntry"/>
        /// will be added for each valid file path. Valid directory paths will be
        /// recursively traversed for files.
        /// </summary>
        /// <param name="filePaths">Array of paths of files to add</param>
        public void AddFilesToInputList(string[] filePaths)
        {
            foreach (var path in filePaths)
            {
                if (Directory.Exists(path))
                {
                    AddFilesToInputList(Directory.GetFileSystemEntries(path));
                }
                else
                {
                    AddFileToInputList(path);
                }
            }
        }

        /// <summary>
        /// Add an <see cref="InputFileListEntry"/> to <see cref="InputFileList"/>
        /// by specifying the path to the file to be added. Does nothing if the
        /// path to the file is invalid, or if an <see cref="InputFileListEntry"/>
        /// already exists with the specified path.
        /// </summary>
        /// <param name="path">Path to the file to be added</param>
        public void AddFileToInputList(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            var exists = InputFileList
                .Select(entry => entry.FilePath)
                .Contains(path);

            if (!exists)
            {
                InputFileList.Add(new InputFileListEntry(path));
            }
        }

        /// <summary>
        /// Remove the specified <see cref="InputFileListEntry"/> from
        /// <see cref="InputFileList"/>
        /// </summary>
        /// <param name="entry">The <see cref="InputFileListEntry"/> to remove
        /// from <see cref="InputFileList"/></param>
        public void RemoveInputListEntry(InputFileListEntry entry)
        {
            InputFileList.Remove(entry);
        }
    }
}
