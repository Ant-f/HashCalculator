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
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Windows;
using HashCalculator.View;
using HashCalculatorTests.TestingInfrastructure;

namespace HashCalculatorTests.ViewModel.Command
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ShowAboutTests
    {
        [Test]
        public void CanExecuteReturnsTrue()
        {
            // Arrange

            var command = new ShowAbout(null);

            // Act

            var canExecute = command.CanExecute(new object());

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ExecuteCallsCreateAboutWindowWithArgumentWindow()
        {
            // Arrange

            var aboutWindowServiceMock = new Mock<IAboutWindowService>();
            var command = new ShowAbout(aboutWindowServiceMock.Object);
            var owner = new Window();

            // Act

            command.Execute(owner);

            // Assert
            aboutWindowServiceMock.Verify(a => a.CreateAboutWindow(owner));
        }

        [Test]
        public void ExecuteCallsShowModal()
        {
            // Arrange

            WpfApplication.Initialize();
            var aboutWindowServiceMock = new Mock<IAboutWindowService>();
            var aboutWindow = new About();

            aboutWindowServiceMock
                .Setup(a => a.CreateAboutWindow(It.IsAny<Window>()))
                .Returns(aboutWindow);

            var command = new ShowAbout(aboutWindowServiceMock.Object);
            var owner = new Window();

            // Act

            command.Execute(owner);

            // Assert
            aboutWindowServiceMock.Verify(a => a.ShowModal(aboutWindow));
        }
    }
}
