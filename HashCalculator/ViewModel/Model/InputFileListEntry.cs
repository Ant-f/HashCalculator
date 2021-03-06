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
using System.ComponentModel;

namespace HashCalculator.ViewModel.Model
{
    /// <summary>
    /// Wrapper for <see cref="FileHashMetadata"/> that implements
    /// <see cref="INotifyPropertyChanged"/> to enable UI classes to respond
    /// accordingly when properties change. Included additional properties that
    /// are related to the properties of <see cref="FileHashMetadata"/> and are
    /// derived external to this class, so they can be represented in the UI
    /// </summary>
    public class InputFileListEntry : PropertyChangedNotifier
    {
        private bool _fileExistsAtFilePath = false;
        private HashCodeMatchCriteria _hashCodeMatch = HashCodeMatchCriteria.None;
        private FileHashMetadata _hashMetadata = null;

        public string FilePath
        {
            get
            {
                return HashMetadata.FilePath;
            }

            set
            {
                if (HashMetadata.FilePath != value)
                {
                    HashMetadata.FilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CalculatedFileHash
        {
            get
            {
                return HashMetadata.FileHashCode;
            }

            set
            {
                if (HashMetadata.FileHashCode != value)
                {
                    HashMetadata.FileHashCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public FileHashMetadata HashMetadata
        {
            get
            {
                return _hashMetadata;
            }

            set
            {
                if (_hashMetadata != value)
                {
                    _hashMetadata = value;
                    OnPropertyChanged();
                }
            }
        }

        public HashCodeMatchCriteria HashCodeMatch
        {
            get
            {
                return _hashCodeMatch;
            }

            set
            {
                if (_hashCodeMatch != value)
                {
                    _hashCodeMatch = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool FileExistsAtFilePath
        {
            get
            {
                return _fileExistsAtFilePath;
            }

            set
            {
                if (_fileExistsAtFilePath != value)
                {
                    _fileExistsAtFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public InputFileListEntry(string filepath)
        {
            HashMetadata = new FileHashMetadata();
            FilePath = filepath;
        }
    }
}
