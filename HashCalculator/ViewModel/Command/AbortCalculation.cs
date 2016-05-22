﻿// HashCalculator
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
    public class AbortCalculation : ICommand
    {
        private readonly IHashCodeBatchCalculationService _hashCodeBatchCalculationService;

        public event EventHandler CanExecuteChanged;

        public AbortCalculation(IHashCodeBatchCalculationService hashCodeBatchCalculationService)
        {
            _hashCodeBatchCalculationService = hashCodeBatchCalculationService;
            _hashCodeBatchCalculationService.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_hashCodeBatchCalculationService.CalculationIsRunning))
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
            AbortHashCalculation();
        }

        private void AbortHashCalculation()
        {
            //_hashCodeBatchCalculationService.Abort();
        }
    }
}
