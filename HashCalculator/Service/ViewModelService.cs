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
using HashCalculator.Ioc;
using Ninject;

namespace HashCalculator.Service
{
    /// <summary>
    /// Service locator for view model related objects, for use when references
    /// are required within XAML
    /// </summary>
    public class ViewModelService
    {
        private static IAboutWindowService _aboutWindowService;
        private static ICommands _commands;
        private static IHashCodeBatchCalculationService _hashCodeBatchCalculationService;
        private static IHashCodeCalculationService _hashCodeCalculationService;
        private static IHashAlgorithmSelection _hashAlgorithmSelection;
        private static IUserInput _userInput;

        /// <summary>
        /// A reference to the about-window service
        /// </summary>
        public static IAboutWindowService AboutWindowService =>
            _aboutWindowService ??
            (_aboutWindowService = NinjectContainer.Kernel.Get<IAboutWindowService>());

        /// <summary>
        /// A reference to the object containing commands
        /// </summary>
        public static ICommands Commands =>
            _commands ??
            (_commands = NinjectContainer.Kernel.Get<ICommands>());

        /// <summary>
        /// A reference to the batch calculation service
        /// </summary>
        public static IHashCodeBatchCalculationService HashCodeBatchCalculationService =>
            _hashCodeBatchCalculationService ??
            (_hashCodeBatchCalculationService = NinjectContainer.Kernel.Get<IHashCodeBatchCalculationService>());

        /// <summary>
        /// A reference to the calculation service
        /// </summary>
        public static IHashCodeCalculationService HashCodeCalculationService =>
            _hashCodeCalculationService ??
            (_hashCodeCalculationService = NinjectContainer.Kernel.Get<IHashCodeCalculationService>());

        /// <summary>
        /// A reference to the object representing the selected hash algorithm
        /// </summary>
        public static IHashAlgorithmSelection HashAlgorithmSelection =>
            _hashAlgorithmSelection ??
            (_hashAlgorithmSelection = NinjectContainer.Kernel.Get<IHashAlgorithmSelection>());

        /// <summary>
        /// A reference to the object holding data that the user has entered
        /// </summary>
        public static IUserInput UserInput =>
            _userInput ??
            (_userInput = NinjectContainer.Kernel.Get<IUserInput>());
    }
}
