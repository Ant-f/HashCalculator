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
using System.ComponentModel;
using HashCalculatorTests.TestingInfrastructure;
using NUnit.Framework;
using System.Linq;
using System.Security.Cryptography;
using HashCalculator.Interface;
using HashCalculator.Service;
using Moq;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class HashCodeCalculationServiceTests
    {
        [Test, Explicit, NUnit.Framework.Category(Constants.FileSystemTestCategory)]
        public void HashCodeIsCalculatedUsingProvidedHashAlgorithm()
        {
            // Arrange

            var fileOperations = new FileOperations();
            var service = new HashCodeCalculationService(fileOperations);

            //Act

            string hashCode;
            using (var algorithm = SHA1.Create())
            {
                hashCode = service.CalculateHashCodes(algorithm, TestingDataService.TestingDataFilePath);
            }

            // Assert

            Assert.AreEqual("D47E327C6DFDE5FFD236B174582D4241A26A0C91", hashCode);
        }

        [TestCase(new byte[] {8}, "08")]
        [TestCase(new byte[] {10}, "0A")]
        [TestCase(new byte[] {30}, "1E")]
        [TestCase(new byte[] {8, 10, 30}, "080A1E")]
        public void ByteArrayIsCorrectlyConvertedToUpperCaseHexString(byte[] bytes, string expectedHexString)
        {
            var fileOperationsMock = new Mock<IFileOperations>();
            var service = new HashCodeCalculationService(fileOperationsMock.Object);

            var hex = service.ConvertBytesToHexString(bytes);

            Assert.AreEqual(expectedHexString, hex);
        }

        [Test, Explicit, NUnit.Framework.Category(Constants.FileSystemTestCategory)]
        public void NormalizedProgressUpdateIsProvidedDuringHashCodeCalculation()
        {
            // Arrange

            var exceptionThrown = false;
            var progressValues = new List<double>();

            var fileOperations = new FileOperations();
            var service = new HashCodeCalculationService(fileOperations);

            var eventHandler = new PropertyChangedEventHandler((sender, args) =>
            {
                if (args.PropertyName == nameof(service.NormalizedProgress))
                {
                    progressValues.Add(service.NormalizedProgress);
                }
            });
            
            service.PropertyChanged += eventHandler;

            //Act

            try
            {
                using (var algorithm = SHA1.Create())
                {
                    service.CalculateHashCodes(algorithm, TestingDataService.TestingDataFilePath);
                }
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
            finally
            {
                service.PropertyChanged -= eventHandler;
            }

            // Assert

            Assert.IsFalse(exceptionThrown);
            Assert.IsNotEmpty(progressValues);

            Assert.That(progressValues, Is.Ordered);

            var min = progressValues.Min();
            Assert.That(min, Is.GreaterThanOrEqualTo(0));

            var max = progressValues.Max();
            Assert.That(max, Is.LessThanOrEqualTo(1));
        }

        [Test, Explicit, NUnit.Framework.Category(Constants.FileSystemTestCategory)]
        public void PercentageProgressUpdateIsProvidedDuringHashCodeCalculation()
        {
            // Arrange

            var exceptionThrown = false;
            var progressValues = new List<int>();

            var fileOperations = new FileOperations();
            var service = new HashCodeCalculationService(fileOperations);

            var eventHandler = new PropertyChangedEventHandler((sender, args) =>
            {
                if (args.PropertyName == nameof(service.PercentageProgress))
                {
                    progressValues.Add(service.PercentageProgress);
                }
            });
            
            service.PropertyChanged += eventHandler;

            //Act

            try
            {
                using (var algorithm = SHA1.Create())
                {
                    service.CalculateHashCodes(algorithm, TestingDataService.TestingDataFilePath);
                }
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
            finally
            {
                service.PropertyChanged -= eventHandler;
            }

            // Assert

            Assert.IsFalse(exceptionThrown);
            Assert.IsNotEmpty(progressValues);

            Assert.That(progressValues, Is.Ordered);

            var min = progressValues.Min();
            Assert.That(min, Is.GreaterThanOrEqualTo(0));

            var max = progressValues.Max();
            Assert.That(max, Is.LessThanOrEqualTo(100));
        }
    }
}
