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
using HashCalculator.ViewModel.Command;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using HashCalculatorTests.TestingInfrastructure;

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

            var exportPathPrompterMock = new Mock<IExportPathPrompter>();

            exportPathPrompterMock.Setup(e => e.ShowPrompt())
                .Returns(exportPath);

            var hashCodeExporterMock = new Mock<IHashCodeExporter>();

            var builder = new UserInputBuilder();
            var userInput = builder.CreateUserInput();

            var command = new ExportHashList(
                exportPathPrompterMock.Object,
                hashCodeExporterMock.Object,
                userInput);

            // Act

            command.Execute(null);

            //Assert

            exportPathPrompterMock.VerifyAll();

            hashCodeExporterMock.Verify(e => e.Export(
                exportPath,
                It.IsAny<IList<FileHashMetadata>>(),
                It.IsAny<bool>()));
        }

        [Test, Ignore("Disabled during refactoring")]
        public void SettingHashCalculationIsRunningRaisesExportHashListCommandCanExecuteChanged()
        {
            //// Arrange

            //var resetEvent = new ManualResetEvent(false);

            //var canExecuteRaised = false;
            //var eventHandler = new EventHandler((sender, args) =>
            //{
            //    canExecuteRaised = true;
            //    resetEvent.Set();
            //});

            //var builder = new HashCalculatorViewModelBuilder();

            //builder.DispatcherServiceMock.Setup(h => h.BeginInvoke(It.IsAny<Delegate>()))
            //    .Callback<Delegate>(method => method.DynamicInvoke());

            //var viewModel = builder.CreateViewModel();
            //viewModel.HashCalculationIsRunning = false;
            //viewModel.ExportHashListCommand.EvaluateCanExecutePredicate(null);
            //viewModel.ExportHashListCommand.CanExecuteChanged += eventHandler;

            //// Act

            //viewModel.HashCalculationIsRunning = true;

            //var timeout = !resetEvent.WaitOne(EventHandlerTimeout);
            //viewModel.ExportHashListCommand.CanExecuteChanged -= eventHandler;

            //// Assert

            //builder.DispatcherServiceMock.VerifyAll();

            //Assert.IsFalse(timeout);
            //Assert.IsTrue(canExecuteRaised);
        }
    }
}
