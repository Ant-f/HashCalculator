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

using HashCalculatorTests.TestingInfrastructure;
using NUnit.Framework;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashCodeCalculationServiceTests
    {
        private static Stream CreateMemoryStream(string content)
        {
            var testingStreamBytes = Encoding.UTF8.GetBytes(content);
            var testingStream = new MemoryStream(testingStreamBytes);
            return testingStream;
        }

        [Test]
        public void HashCodeIsCalculatedUsingProvidedHashAlgorithm()
        {
            // Arrange

            const string fileName = "File1.txt";

            var testingStream = CreateMemoryStream("Stream");

            var builder = new HashCodeCalculationServiceBuilder();
            builder.FileOperationsMock.Setup(f => f.ReadFile(fileName)).Returns(testingStream);

            var service = builder.Build();

            //Act

            string hashCode;
            using (var algorithm = SHA1.Create())
            {
                hashCode = service.CalculateHashCodes(algorithm, fileName);
            }

            // Assert

            Assert.AreEqual("DF063869E11D7A9AA132CD4A984F7B5EB870D656", hashCode);
        }

        [TestCase(new byte[] {8}, "08")]
        [TestCase(new byte[] {10}, "0A")]
        [TestCase(new byte[] {30}, "1E")]
        [TestCase(new byte[] {8, 10, 30}, "080A1E")]
        public void ByteArrayIsCorrectlyConvertedToUpperCaseHexString(byte[] bytes, string expectedHexString)
        {
            var builder = new HashCodeCalculationServiceBuilder();
            var service = builder.Build();

            var hex = service.ConvertBytesToHexString(bytes);

            Assert.AreEqual(expectedHexString, hex);
        }
    }
}
