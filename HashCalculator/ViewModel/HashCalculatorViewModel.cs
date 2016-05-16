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
using System.Threading;

namespace HashCalculator.ViewModel
{
    public class HashCalculatorViewModel : PropertyChangedNotifier, IHashCalculatorViewModel
    {
        private readonly IExportPathPrompter _exportPathPrompter;
        private readonly IFileExistenceChecker _fileExistenceChecker;
        private readonly IFileHashCodeMatchChecker _fileHashCodeMatchChecker;
        private readonly IHashCodeBatchCalculationService _hashCodeBatchCalculationService;
        private readonly IHashCodeExporter _hashCodeExporter;
        private readonly IPropertyChangedSubscriber _propertyChangedSubscriber;

        private static readonly char[] NewLineSeparators = { '\r', '\n' };
        private static readonly char[] HashSeparators = { '*' };

        private Thread hashCalcThread;

        private bool hashCalculationIsRunning = false;
        private double _normalizedFileCalculationProgress = 0;
        private int _fileCalculationProgressPercentage;
        private string _knownFileHashCodesText = string.Empty;
        private bool _matchFullFilePath = false;

        public bool HashCalculationIsRunning
        {
            get
            {
                return hashCalculationIsRunning;
            }

            set
            {
                if (hashCalculationIsRunning != value)
                {
                    hashCalculationIsRunning = value;
                    OnPropertyChanged();

                    ExportHashListCommand.EvaluateCanExecutePredicate(null);
                }
            }
        }

        public double NormalizedFileCalculationProgress
        {
            get
            {
                return _normalizedFileCalculationProgress;
            }

            private set
            {
                if (_normalizedFileCalculationProgress != value)
                {
                    _normalizedFileCalculationProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public int FileCalculationProgressPercentage
        {
            get
            {
                return _fileCalculationProgressPercentage;
            }

            private set
            {
                if (_fileCalculationProgressPercentage != value)
                {
                    _fileCalculationProgressPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public RelayCommand CalculateHashCodesCommand { get; }
        public RelayCommand ExportHashListCommand { get; }

        public HashCalculatorViewModel(
            IDispatcherService dispatcherService,
            IExportPathPrompter exportPathPrompter,
            IFileExistenceChecker fileExistenceChecker,
            IFileHashCodeMatchChecker fileHashCodeMatchChecker,
            IHashCodeBatchCalculationService hashCodeBatchCalculationService,
            IHashCodeExporter hashCodeExporter,
            IPropertyChangedSubscriber propertyChangedSubscriber)
        {
            _exportPathPrompter = exportPathPrompter;
            _fileExistenceChecker = fileExistenceChecker;
            _fileHashCodeMatchChecker = fileHashCodeMatchChecker;
            _hashCodeBatchCalculationService = hashCodeBatchCalculationService;
            _hashCodeExporter = hashCodeExporter;
            _propertyChangedSubscriber = propertyChangedSubscriber;
            _propertyChangedSubscriber.EventHandler = InputFileListEntryPropertyChanged;

            InputFileList = new ObservableCollection<InputFileListEntry>();
            InputFileList.CollectionChanged += InputFileListCollectionChanged;

            CalculateHashCodesCommand = new RelayCommand(
                dispatcherService,
                param => CalculateFileHash(),
                param => !HashCalculationIsRunning);

            ExportHashListCommand = new RelayCommand(
                dispatcherService,
                ExportHashList,
                param => !HashCalculationIsRunning);
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

        private void InputFileListEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entry = sender as InputFileListEntry;
            if (entry != null)
            {
                entry.HashCodeMatch = CheckFileHashMatch(entry.HashMetadata);
            }
        }

        private void EvaluateFileExistence(IList listEntries)
        {
            foreach (var listEntry in listEntries.Cast<InputFileListEntry>())
            {
                listEntry.FileExistsAtFilePath = _fileExistenceChecker.Exists(listEntry.FilePath);
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

        public void StartHashCalculationBatch()
        {
            hashCalcThread = new Thread(CalculateFileHash);
            hashCalcThread.IsBackground = true;
            HashCalculationIsRunning = true;
            hashCalcThread.Start();
        }

        public void AbortHashCalculation()
        {
            hashCalcThread.Abort();
        }

        private void CalculateFileHash()
        {
            foreach (InputFileListEntry entry in InputFileList)
                entry.CalculatedFileHash = String.Empty;

            _hashCodeBatchCalculationService.CalculateHashCodes("sha512", InputFileList);

            //HashAlgorithm algorithm = null;
            //ProgressUpdateStream stream = null;
            //bool subscribed = false;
            //try
            //{
            //    /**
            //        * http://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx
            //        * 
            //        * Windows 7                    6.1
            //        * Windows Server 2008 R2       6.1
            //        * Windows Server 2008          6.0
            //        * Windows Vista                6.0
            //        * Windows Server 2003 R2       5.2
            //        * Windows Server 2003          5.2
            //        * Windows XP 64-Bit Edition    5.2
            //        * Windows XP                   5.1
            //        * Windows 2000                 5.0 */

            //    int osVersion = Environment.OSVersion.Version.Major;

            //    if (SelectedHashAlgorithm == HashAlgorithmMd5)
            //        algorithm = osVersion > 5 ? (HashAlgorithm)new MD5Cng() : (HashAlgorithm)new MD5CryptoServiceProvider();

            //    else if (SelectedHashAlgorithm == HashAlgorithmSha1)
            //        algorithm = osVersion > 5 ? (HashAlgorithm)new SHA1Cng() : (HashAlgorithm)new SHA1CryptoServiceProvider();

            //    else if (SelectedHashAlgorithm == HashAlgorithmSha256)
            //        algorithm = osVersion > 5 ? (HashAlgorithm)new SHA256Cng() : (HashAlgorithm)new SHA256Managed();

            //    else if (SelectedHashAlgorithm == HashAlgorithmSha512)
            //        algorithm = osVersion > 5 ? (HashAlgorithm)new SHA512Cng() : (HashAlgorithm)new SHA512Managed();

            //    Debug.Assert(stream == null);

            //    SetCurrentFileCalculationProgress(0);

            //    for (int i = 0; i < InputFileList.Count; i++)
            //    {
            //        var entry = InputFileList[i];
            //        if (File.Exists(entry.FilePath))
            //        {
            //            stream = new ProgressUpdateStream(entry.FilePath);
            //            FileListProgress = $"{i + 1}/{InputFileList.Count}";

            //            subscribed = true;
            //            stream.ProgressUpdate += stream_ProgressUpdate;

            //            entry.CalculatedFileHash = PrintByteArray(algorithm.ComputeHash(stream));
            //            stream.Close();

            //            stream.ProgressUpdate -= stream_ProgressUpdate;
            //            subscribed = false;
            //        }
            //    }
            //}
            //catch (ArgumentException)
            //{
            //}
            //catch (FileNotFoundException)
            //{
            //}
            //catch (ThreadAbortException)
            //{
            //}
            //finally
            //{
            //    if (algorithm != null)
            //        algorithm.Dispose();

            //    if (stream != null)
            //    {
            //        stream.Close();
            //        if (subscribed)
            //            stream.ProgressUpdate -= stream_ProgressUpdate;
            //    }

            //    HashCalculationIsRunning = false;
            //}
        }

        void stream_ProgressUpdate(object sender, ReadProgressEventArgs e)
        {
            SetCurrentFileCalculationProgress(e.Progress);
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

        private void ExportHashList(object parameter)
        {
            var path = _exportPathPrompter.ShowPrompt();

            if (!string.IsNullOrWhiteSpace(path))
            {
                var inputList = InputFileList.Select(itm => itm.HashMetadata).ToArray();
                _hashCodeExporter.Export(path, inputList, MatchFullFilePath);
            }
        }

        internal void SetCurrentFileCalculationProgress(double normalizedProgress)
        {
            NormalizedFileCalculationProgress = normalizedProgress;
            FileCalculationProgressPercentage = (int)(normalizedProgress * 100);
        }
    }
}
