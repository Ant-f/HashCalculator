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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HashCalculator.Interface;
using HashCalculator.ViewModel.Command;

namespace HashCalculator.ViewModel
{
    /// <summary>
    /// View model class for UI bindings. Groups together the commands used
    /// within the application.
    /// </summary>
    public class Commands : ICommands
    {
        public ICommand AbortCalculation { get; }
        public ICommand BeginCalculation { get; }
        public ICommand ClearFilePath { get; }
        public ICommand ExportHashList { get; }
        public ICommand ShowAbout { get; }

        public Commands(IList<ICommand> commandCollection)
        {
            AbortCalculation = commandCollection.OfType<AbortCalculation>().Single();
            BeginCalculation = commandCollection.OfType<BeginCalculation>().Single();
            ClearFilePath = commandCollection.OfType<ClearFilePath>().Single();
            ExportHashList = commandCollection.OfType<ExportHashList>().Single();
            ShowAbout = commandCollection.OfType<ShowAbout>().Single();
        }
    }
}
