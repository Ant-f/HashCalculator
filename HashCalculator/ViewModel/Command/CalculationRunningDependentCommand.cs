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
using System;
using System.Windows.Input;

namespace HashCalculator.ViewModel.Command
{
    /// <summary>
    /// Base class for classes implementing ICommand that should raise
    /// CanExecuteChanged when the value of
    /// IHashCodeBatchCalculationService.CalculationIsRunning changes
    /// </summary>
    public abstract class CalculationRunningDependentCommand : ICommand
    {
        private readonly IDispatcherService _dispatcherService;

        protected readonly IHashCodeBatchCalculationService HashCodeBatchCalculationService;

        public event EventHandler CanExecuteChanged;

        protected CalculationRunningDependentCommand(
            IDispatcherService dispatcherService,
            IHashCodeBatchCalculationService hashCodeBatchCalculationService)
        {
            _dispatcherService = dispatcherService;

            HashCodeBatchCalculationService = hashCodeBatchCalculationService;
            HashCodeBatchCalculationService.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(HashCodeBatchCalculationService.CalculationIsRunning))
                {
                    _dispatcherService.BeginInvoke(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
                }
            };
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
