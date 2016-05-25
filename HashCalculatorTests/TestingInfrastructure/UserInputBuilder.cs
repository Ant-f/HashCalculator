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
using Moq;

namespace HashCalculatorTests.TestingInfrastructure
{
    internal class UserInputBuilder
    {
        /// <summary>
        /// The FileExistenceChecker instance to use in the created command. A
        /// mock instance will be used if no value is specified.
        /// </summary>
        public FileExistenceChecker FileExistenceChecker { get; set; }

        /// <summary>
        /// The FileHashCodeMatchChecker instance to use in the created command.
        /// A mock instance will be used if no value is specified.
        /// </summary>
        public FileHashCodeMatchChecker FileHashCodeMatchChecker { get; set; }

        /// <summary>
        /// The PropertyChangedSubscriber instance to use in the created command.
        /// A mock instance will be used if no value is specified.
        /// </summary>
        public PropertyChangedSubscriber PropertyChangedSubscriber { get; set; }

        /// <summary>
        /// A mock IFileExistenceChecker that can be used to specify desired
        /// behaviour. This will only be used if no value is specified for the
        /// FileExistenceChecker property.
        /// </summary>
        public Mock<IFileExistenceChecker> FileExistenceCheckerMock { get; }
            = new Mock<IFileExistenceChecker>();

        /// <summary>
        /// A mock IFileHashCodeMatchChecker that can be used to specify desired
        /// behaviour. This will only be used if no value is specified for the
        /// FileHashCodeMatchChecker property.
        /// </summary>
        public Mock<IFileHashCodeMatchChecker> FileHashCodeMatchCheckerMock { get; }
            = new Mock<IFileHashCodeMatchChecker>();

        /// <summary>
        /// A mock IPropertyChangedSubscriber that can be used to specify desired
        /// behaviour. This will only be used if no value is specified for the
        /// PropertyChangedSubscriber property.
        /// </summary>
        public Mock<IPropertyChangedSubscriber> PropertyChangedSubscriberMock { get; }
            = new Mock<IPropertyChangedSubscriber>();

        /// <summary>
        /// Create a new UserInput, configured with the properties available in
        /// this UserInputBuilder instance
        /// </summary>
        /// <returns>
        /// A new UserInput, configured with the properties available in this
        /// UserInputBuilder instance
        /// </returns>
        public UserInput CreateUserInput()
        {
            var viewModel = new UserInput(
                FileHashCodeMatchChecker ?? FileHashCodeMatchCheckerMock.Object,
                FileExistenceChecker ?? FileExistenceCheckerMock.Object,
                PropertyChangedSubscriber ?? PropertyChangedSubscriberMock.Object);

            return viewModel;
        }
    }
}
