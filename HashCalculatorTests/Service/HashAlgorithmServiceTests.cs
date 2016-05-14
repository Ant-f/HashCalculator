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

using HashCalculator.Service;
using NUnit.Framework;
using System.Security.Cryptography;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashAlgorithmServiceTests
    {
        [TestCase(HashAlgorithmService.HashAlgorithmMd5, typeof(MD5))]
        [TestCase(HashAlgorithmService.HashAlgorithmSha1, typeof(SHA1))]
        [TestCase(HashAlgorithmService.HashAlgorithmSha256, typeof(SHA256))]
        [TestCase(HashAlgorithmService.HashAlgorithmSha512, typeof(SHA512))]
        public void ReturnedHashAlgorithmCorrespondsToParameterName<T>(
            string algorithmName,
            T expectedAlgorithmType)
        {
            var service = new HashAlgorithmService();

            var algorithm = service.GetAlgorithm(algorithmName);

            Assert.IsTrue(algorithm.GetType() is T);
        }
    }
}
