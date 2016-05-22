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

using HashCalculator.Model;
using HashCalculatorTests.TestingInfrastructure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HashCalculatorTests.ViewModel.Command
{
    [TestFixture]
    public class ExportHashListTests
    {
        [Test]
        public void ExportCommandCallsHashCodeExporter()
        {
            // Arrange

            const string exportPath = "ExportPath";

            var commandBuilder = new ExportHashListBuilder();

            commandBuilder.ExportPathPrompterMock.Setup(e => e.ShowPrompt())
                .Returns(exportPath);

            var userInputBuilder = new UserInputBuilder();
            commandBuilder.UserInput = userInputBuilder.CreateUserInput();

            var command = commandBuilder.CreatExportHashList();

            // Act

            command.Execute(null);

            //Assert

            commandBuilder.ExportPathPrompterMock.VerifyAll();

            commandBuilder.HashCodeExporterMock.Verify(e => e.Export(
                exportPath,
                It.IsAny<IList<FileHashMetadata>>(),
                It.IsAny<bool>()));
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

            var builder = new ExportHashListBuilder();
            var command = builder.CreatExportHashList();
            command.CanExecuteChanged += eventHandler;

            // Act

            builder.HashCodeBatchCalculationServiceMock.Raise(s =>
                s.PropertyChanged += null,
                new PropertyChangedEventArgs(nameof(builder.HashCodeBatchCalculationService.CalculationIsRunning)));

            command.CanExecuteChanged -= eventHandler;

            // Assert

            Assert.IsTrue(canExecuteChangedRaised);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void CanExecuteReturnsAppropriateValueForCalculationIsRunningValue(
            bool calculationIsRunning,
            bool expectedCanExecuteValue)
        {
            var builder = new ExportHashListBuilder();
            builder.HashCodeBatchCalculationServiceMock
                .Setup(s => s.CalculationIsRunning)
                .Returns(calculationIsRunning);

            var command = builder.CreatExportHashList();

            var canExecute = command.CanExecute(null);
            Assert.AreEqual(expectedCanExecuteValue, canExecute);
        }
    }
}
