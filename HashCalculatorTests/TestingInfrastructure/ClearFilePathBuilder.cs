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
using HashCalculator.ViewModel.Command;
using Moq;

namespace HashCalculatorTests.TestingInfrastructure
{
    internal class ClearFilePathBuilder
    {
        /// <summary>
        /// The HashCodeBatchCalculationService instance to use in the created
        /// command. A mock instance will be used if no value is specified.
        /// </summary>
        public HashCodeBatchCalculationService HashCodeBatchCalculationService { get; set; }

        /// <summary>
        /// A mock IHashCodeBatchCalculationService that can be used to
        /// specify desired behaviour. This will only be used if no value
        /// is specified for the HashCodeBatchCalculationService property.
        /// </summary>
        public Mock<IHashCodeBatchCalculationService> HashCodeBatchCalculationServiceMock { get; }
            = new Mock<IHashCodeBatchCalculationService>();

        /// <summary>
        /// Create a new ClearFilePath, configured with the properties available in
        /// this ClearFilePathBuilder instance
        /// </summary>
        /// <returns>
        /// A new ClearFilePath, configured with the properties available in this
        /// ClearFilePathBuilder instance
        /// </returns>
        public ClearFilePath CreateClearFilePath()
        {
            var command = new ClearFilePath(
                new TestingDispatcherService(),
                HashCodeBatchCalculationService ?? HashCodeBatchCalculationServiceMock.Object);

            return command;
        }
    }
}
