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
using HashCalculator.ViewModel;
using HashCalculator.ViewModel.Command;
using Moq;

namespace HashCalculatorTests.TestingInfrastructure
{
    internal class BeginCalculationBuilder
    {
        /// <summary>
        /// The HashCodeBatchCalculationService instance to use in the created
        /// view model. A mock instance will be used if no value is specified.
        /// </summary>
        public HashCodeBatchCalculationService HashCodeBatchCalculationService { get; set; }

        /// <summary>
        /// The UserInput instance to use in the created view model. A mock
        /// instance will be used if no value is specified.
        /// </summary>
        public UserInput UserInput { get; set; }

        /// <summary>
        /// A mock IHashCodeBatchCalculationService that can be used to
        /// specify desired behaviour. This will only be used if no value
        /// is specified for the HashCodeBatchCalculationService property.
        /// </summary>
        public Mock<IHashCodeBatchCalculationService> HashCodeBatchCalculationServiceMock { get; }
            = new Mock<IHashCodeBatchCalculationService>();

        /// <summary>
        /// A mock IUserInput that can be used to specify desired behaviour.
        /// This will only be used if no value is specified for the UserInput
        /// property.
        /// </summary>
        public Mock<IUserInput> UserInputMock { get; }
            = new Mock<IUserInput>();

        /// <summary>
        /// Create a new BeginCalculation, configured with the properties available in
        /// this BeginCalculationBuilder instance
        /// </summary>
        /// <returns>
        /// A new BeginCalculation, configured with the properties available in this
        /// BeginCalculationBuilder instance
        /// </returns>
        public BeginCalculation CreateBeginCalculation()
        {
            var command = new BeginCalculation(
                HashCodeBatchCalculationService ?? HashCodeBatchCalculationServiceMock.Object,
                UserInput ?? UserInputMock.Object);

            return command;
        }
    }
}
