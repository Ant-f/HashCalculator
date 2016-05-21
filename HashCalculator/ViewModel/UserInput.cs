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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace HashCalculator.ViewModel
{
    public class UserInput : IUserInput
    {
        private static readonly char[] NewLineSeparators = { '\r', '\n' };
        private static readonly char[] HashSeparators = { '*' };

        private readonly IFileExistenceChecker _fileExistenceChecker;
        private readonly IFileHashCodeMatchChecker _fileHashCodeMatchChecker;
        private readonly IPropertyChangedSubscriber _propertyChangedSubscriber;

        private string _knownFileHashCodesText = string.Empty;
        private bool _matchFullFilePath = false;

        public ObservableCollection<InputFileListEntry> InputFileList { get; }

        public List<FileHashMetadata> KnownFileHashList { get; } = new List<FileHashMetadata>();

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
                    BuildKnownFileHashList();
                    EvaluateInputFileListHashCodeMatchCriteria();
                }
            }
        }

        /// <summary>
        /// Specifies whether to use a file's full path, or its name and extension only
        /// when determining a file's identity
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
                    BuildKnownFileHashList();
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

        private void InputFileListEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entry = sender as InputFileListEntry;
            if (entry != null)
            {
                entry.HashCodeMatch = CheckFileHashMatch(entry.HashMetadata);
            }
        }

        private void InputFileListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    EvaluateFileExistence(e.NewItems);
                    SubscribeToPropertyChanged(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    EvaluateFileExistence(e.NewItems);
                    SubscribeToPropertyChanged(e.NewItems);
                    UnsubscribeToPropertyChanged(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    UnsubscribeToPropertyChanged(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _propertyChangedSubscriber.UnsubscribeAll();
                    break;
            }
        }

        private void SubscribeToPropertyChanged(IEnumerable items)
        {
            foreach (var entry in items.Cast<INotifyPropertyChanged>())
            {
                _propertyChangedSubscriber.Subscribe(entry);
            }
        }

        private void UnsubscribeToPropertyChanged(IEnumerable items)
        {
            foreach (var entry in items.Cast<INotifyPropertyChanged>())
            {
                _propertyChangedSubscriber.Unsubscribe(entry);
            }
        }

        private void EvaluateFileExistence(IList listEntries)
        {
            foreach (var listEntry in listEntries.Cast<InputFileListEntry>())
            {
                listEntry.FileExistsAtFilePath = _fileExistenceChecker.Exists(listEntry.FilePath);
            }
        }

        public void BuildKnownFileHashList()
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

        private void EvaluateInputFileListHashCodeMatchCriteria()
        {
            foreach (var listEntry in InputFileList)
            {
                listEntry.HashCodeMatch = CheckFileHashMatch(listEntry.HashMetadata);
            }
        }

        private HashCodeMatchCriteria CheckFileHashMatch(FileHashMetadata metadata)
        {
            var matchCriteria = _fileHashCodeMatchChecker.FindMatchCriteria(
                metadata,
                KnownFileHashList,
                MatchFullFilePath);

            return matchCriteria;
        }

        public void AddFilesToInputList(string[] files)
        {
            foreach (string str in files)
            {
                if (Directory.Exists(str))
                    AddFilesToInputList(Directory.GetFileSystemEntries(str));
                else
                    AddFileToInputList(str);
            }
        }

        public void AddFileToInputList(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            if (InputFileList
                .Select(entry => entry.FilePath)
                .SingleOrDefault(existingPath => path == existingPath) == null)
            {
                InputFileList.Add(new InputFileListEntry(path));
            }
        }

        public void RemoveInputListEntry(InputFileListEntry entry)
        {
            InputFileList.Remove(entry);
        }
    }
}
