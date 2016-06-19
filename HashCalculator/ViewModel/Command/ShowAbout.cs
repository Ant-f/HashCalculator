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
using System.Windows;
using System.Windows.Input;

namespace HashCalculator.ViewModel.Command
{
    /// <summary>
    /// Display a window with information about the application
    /// </summary>
    public class ShowAbout : ICommand
    {
        private readonly IAboutWindowService _aboutWindowService;

        public event EventHandler CanExecuteChanged;

        public ShowAbout(IAboutWindowService aboutWindowService)
        {
            _aboutWindowService = aboutWindowService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var owner = (Window) parameter;
            var about = _aboutWindowService.CreateAboutWindow(owner);
            _aboutWindowService.ShowModal(about);
        }
    }
}
