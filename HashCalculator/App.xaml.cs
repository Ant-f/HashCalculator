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

using System.Windows;
using HashCalculator.Interface;
using HashCalculator.Service;
using HashCalculator.ViewModel;
using Ninject;

namespace HashCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IKernel IocKernel { get; } = CreateIocKernel();

        private static IKernel CreateIocKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IDispatcherService>().To<DispatcherService>().InSingletonScope();
            kernel.Bind<IExportPathPrompter>().To<ExportPathPrompter>().InSingletonScope();
            kernel.Bind<IFileOperations>().To<FileOperations>().InSingletonScope();
            kernel.Bind<IFileExistenceChecker>().To<FileExistenceChecker>().InSingletonScope();
            kernel.Bind<IFileHashCodeMatchChecker>().To<FileHashCodeMatchChecker>().InSingletonScope();
            kernel.Bind<IHashAlgorithmSelection>().To<HashAlgorithmSelection>().InSingletonScope();
            kernel.Bind<IHashCalculatorViewModel>().To<HashCalculatorViewModel>().InSingletonScope();
            kernel.Bind<IHashCodeExporter>().To<HashCodeExporter>().InSingletonScope();
            kernel.Bind<IPropertyChangedSubscriber>().To<PropertyChangedSubscriber>().InSingletonScope();
            return kernel;
        }
    }
}
