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

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HashCalculator.Service
{
    public class HashAlgorithmService
    {
        public const string HashAlgorithmMd5 = "MD5";
        public const string HashAlgorithmSha1 = "SHA1";
        public const string HashAlgorithmSha256 = "SHA256";
        public const string HashAlgorithmSha512 = "SHA512";

        private readonly Dictionary<string, Func<HashAlgorithm>> _hashAlgorithms
            = new Dictionary<string, Func<HashAlgorithm>>
        {
                [HashAlgorithmMd5] = () => MD5.Create(),
                [HashAlgorithmSha1] = () => SHA1.Create(),
                [HashAlgorithmSha256] = () => SHA256.Create(),
                [HashAlgorithmSha512] = () => SHA512.Create()
            };

        public HashAlgorithm GetAlgorithm(string algorithmName)
        {
            HashAlgorithm algorithm = null;

            if (_hashAlgorithms.ContainsKey(algorithmName))
            {
                algorithm = _hashAlgorithms[algorithmName]();
            }

            return algorithm;
        }
    }
}
