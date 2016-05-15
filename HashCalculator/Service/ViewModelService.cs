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
using Ninject;

namespace HashCalculator.Service
{
    /// <summary>
    /// Service locator for view model and related objects, for use when
    /// references are required within XAML
    /// </summary>
    public class ViewModelService
    {
        private static IHashCalculatorViewModel _viewModel;
        private static IHashAlgorithmSelection _hashAlgorithmSelection;

        /// <summary>
        /// A reference to the view model
        /// </summary>
        public static IHashCalculatorViewModel ViewModel =>
            _viewModel ??
            (_viewModel = App.IocKernel.Get<IHashCalculatorViewModel>());

        /// <summary>
        /// A reference to the object describing the hash algorithm selected
        /// in the view model
        /// </summary>
        public static IHashAlgorithmSelection HashAlgorithmSelection =>
            _hashAlgorithmSelection ??
            (_hashAlgorithmSelection = App.IocKernel.Get<IHashAlgorithmSelection>());
    }
}
