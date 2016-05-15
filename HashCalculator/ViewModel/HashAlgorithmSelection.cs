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

using System.Collections.Generic;
using HashCalculator.Interface;

namespace HashCalculator.ViewModel
{
    public class HashAlgorithmSelection : PropertyChangedNotifier, IHashAlgorithmSelection
    {
        public const string MD5 = "MD5";
        public const string SHA1 = "SHA1";
        public const string SHA256 = "SHA256";
        public const string SHA512 = "SHA512";

        private string _selectedHashAlgorithm = SHA256;

        public List<string> HashAlgorithms { get; } = new List<string>
        {
            MD5,
            SHA1,
            SHA256,
            SHA512
        };

        public string SelectedHashAlgorithm
        {
            get
            {
                return _selectedHashAlgorithm;
            }

            set
            {
                if (_selectedHashAlgorithm != value)
                {
                    _selectedHashAlgorithm = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
