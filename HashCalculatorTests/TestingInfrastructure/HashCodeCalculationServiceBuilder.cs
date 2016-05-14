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
using HashCalculator.Service;
using Moq;

namespace HashCalculatorTests.TestingInfrastructure
{
    /// <summary>
    /// Builds HashCodeCalculationServiceBuilder instances for use in unit tests
    /// </summary>
    internal class HashCodeCalculationServiceBuilder
    {
        /// <summary>
        /// A mock IFileOperations that can be used to specify desired behaviour
        /// </summary>
        public Mock<IFileOperations> FileOperationsMock { get; }
            = new Mock<IFileOperations>();

        public HashCodeCalculationService Build()
        {
            var service = new HashCodeCalculationService(FileOperationsMock.Object);
            return service;
        }
    }
}
