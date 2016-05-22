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

using HashCalculator.ViewModel.Model;
using HashCalculatorTests.TestingInfrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HashCalculatorTests.ViewModel.Command
{
    [TestFixture]
    public class BeginCalculationTests
    {
        [Test]
        public void CalculateHashCodesCommandUsesHashCodeBatchCalculationService()
        {
            // Arrange

            var commandBuilder = new BeginCalculationBuilder();

            var userInputBuilder = new UserInputBuilder();
            commandBuilder.UserInput = userInputBuilder.CreateUserInput();

            var command = commandBuilder.CreateBeginCalculation();

            // Act

            command.Execute(null);

            // Assert

            commandBuilder.HashCodeBatchCalculationServiceMock.Verify(s => s.CalculateHashCodes(
                It.IsAny<string>(),
                It.IsAny<IList<InputFileListEntry>>()));
        }

        [Test]
        public void CanExecuteChangedIsRaisedWhenBatchCalculationServiceCalculationIsRunningIsChanged()
        {
            // Arrange
            var canExecuteChangedRaised = false;

            var eventHandler = new EventHandler((sender, args) =>
            {
                canExecuteChangedRaised = true;
            });

            var builder = new BeginCalculationBuilder();
            var command = builder.CreateBeginCalculation();
            command.CanExecuteChanged += eventHandler;

            // Act

            builder.HashCodeBatchCalculationServiceMock.Raise(s =>
                s.PropertyChanged += null,
                new PropertyChangedEventArgs(nameof(builder.HashCodeBatchCalculationService.CalculationIsRunning)));

            command.CanExecuteChanged -= eventHandler;

            // Assert

            Assert.IsTrue(canExecuteChangedRaised);
        }
    }
}
