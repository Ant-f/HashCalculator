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
using HashCalculatorTests.TestingInfrastructure;
using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace HashCalculatorTests.Service
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class AboutWindowServiceTests
    {
        [Test]
        public void CreateAboutWindowReturnsNewAboutToolWindow()
        {
            // Arrange

            WpfApplication.Initialize();
            var service = new AboutWindowService();

            // Act

            var about = service.CreateAboutWindow(null);

            // Assert

            Assert.That(about.WindowStyle, Is.EqualTo(WindowStyle.ToolWindow));
        }

        [Test]
        public void StartupLocationIsCenterOwner()
        {
            // Arrange

            WpfApplication.Initialize();
            var service = new AboutWindowService();

            // Act

            var about = service.CreateAboutWindow(null);

            // Assert

            Assert.That(
                about.WindowStartupLocation,
                Is.EqualTo(WindowStartupLocation.CenterOwner));
        }

        [Test, Explicit]
        public void WindowParameterIsSetAsWindowOwner()
        {
            // Arrange

            WpfApplication.Initialize();
            var parent = new Window();

            // A parent window must have been shown prior to setting as a
            // window owner

            parent.Show();

            var service = new AboutWindowService();

            // Act

            var about = service.CreateAboutWindow(parent);

            // Assert

            Assert.That(about.Owner, Is.EqualTo(parent));
        }

        [Test]
        public void Gpl3ContainsLicenseHeader()
        {
            var service = new AboutWindowService();

            Assert.IsFalse(string.IsNullOrWhiteSpace(service.Gpl3));
            Assert.IsTrue(service.Gpl3.Contains("GNU GENERAL PUBLIC LICENSE"));
            Assert.IsTrue(service.Gpl3.Contains("Version 3"));
        }

        [Test]
        public void Apache2ContainsLicenseHeader()
        {
            var service = new AboutWindowService();

            Assert.IsFalse(string.IsNullOrWhiteSpace(service.Apache2));
            Assert.IsTrue(service.Apache2.Contains("Apache License"));
            Assert.IsTrue(service.Apache2.Contains("Version 2.0"));
        }
    }
}
