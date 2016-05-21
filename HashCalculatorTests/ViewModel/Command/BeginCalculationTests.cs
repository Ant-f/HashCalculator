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
using HashCalculator.ViewModel.Command;
using HashCalculator.ViewModel.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using HashCalculatorTests.TestingInfrastructure;

namespace HashCalculatorTests.ViewModel.Command
{
    [TestFixture]
    public class BeginCalculationTests
    {
        [Test]
        public void CalculateHashCodesCommandUsesHashCodeBatchCalculationService()
        {
            // Arrange
            
            var hashCodeBatchCalculationServiceMock = new Mock<IHashCodeBatchCalculationService>();

            var builder = new UserInputBuilder();
            var userInput = builder.CreateUserInput();

            var command = new BeginCalculation(
                hashCodeBatchCalculationServiceMock.Object,
                userInput);

            // Act

            command.Execute(null);

            // Assert

            hashCodeBatchCalculationServiceMock.Verify(s => s.CalculateHashCodes(
                It.IsAny<string>(),
                It.IsAny<IList<InputFileListEntry>>()));
        }
    }
}
