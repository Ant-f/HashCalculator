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
using Ninject.Modules;

namespace HashCalculator.Ioc
{
    /// <summary>
    /// Ninject module that creates bindings for the services used within the
    /// application
    /// </summary>
    internal class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAboutWindowService>().To<AboutWindowService>().InSingletonScope();
            Kernel.Bind<IDispatcherService>().To<DispatcherService>().InSingletonScope();
            Kernel.Bind<IExportPathPrompter>().To<ExportPathPrompter>().InSingletonScope();
            Kernel.Bind<IFileExistenceChecker>().To<FileExistenceChecker>().InSingletonScope();
            Kernel.Bind<IFileHashCodeMatchChecker>().To<FileHashCodeMatchChecker>().InSingletonScope();
            Kernel.Bind<IFileOperations>().To<FileOperations>().InSingletonScope();
            Kernel.Bind<IHashCodeBatchCalculationService>().To<HashCodeBatchCalculationService>().InSingletonScope();
            Kernel.Bind<IHashCodeCalculationService>().To<HashCodeCalculationService>().InSingletonScope();
            Kernel.Bind<IHashCodeExporter>().To<HashCodeExporter>().InSingletonScope();
            Kernel.Bind<IPropertyChangedSubscriber>().To<PropertyChangedSubscriber>().InSingletonScope();
        }
    }
}
