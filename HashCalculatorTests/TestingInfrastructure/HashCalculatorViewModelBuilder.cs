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
    /// <summary>
    /// Builds HashCalculatorViewModel instances for use in unit tests
    /// </summary>
    internal class HashCalculatorViewModelBuilder
    {
        /// <summary>
        /// The DispatcherService instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public DispatcherService DispatcherService { get; set; }

        /// <summary>
        /// The ExportPathPrompter instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public ExportPathPrompter ExportPathPrompter { get; set; }

        /// <summary>
        /// The FileExistenceChecker instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public FileExistenceChecker FileExistenceChecker { get; set; }

        /// <summary>
        /// The FileHashCodeMatchChecker instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public FileHashCodeMatchChecker FileHashCodeMatchChecker { get; set; }

        /// <summary>
        /// The HashCodeExporter instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public HashCodeExporter HashCodeExporter { get; set; }

        /// <summary>
        /// The PropertyChangedSubscriber instance to use in the created view model. A mock instance will
        /// be used if no value is specified.
        /// </summary>
        public PropertyChangedSubscriber PropertyChangedSubscriber { get; set; }

        /// <summary>
        /// A mock IDispatcherService that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the DispatcherService property.
        /// </summary>
        public Mock<IDispatcherService> DispatcherServiceMock { get; }
            = new Mock<IDispatcherService>();

        /// <summary>
        /// A mock IExportPathPrompter that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the ExportPathPrompter property.
        /// </summary>
        public Mock<IExportPathPrompter> ExportPathPrompterMock { get; }
            = new Mock<IExportPathPrompter>();

        /// <summary>
        /// A mock IFileExistenceChecker that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the FileExistenceChecker property.
        /// </summary>
        public Mock<IFileExistenceChecker> FileExistenceCheckerMock { get; }
            = new Mock<IFileExistenceChecker>();

        /// <summary>
        /// A mock IFileHashCodeMatchChecker that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the FileHashCodeMatchChecker property.
        /// </summary>
        public Mock<IFileHashCodeMatchChecker> FileHashCodeMatchCheckerMock { get; }
            = new Mock<IFileHashCodeMatchChecker>();

        /// <summary>
        /// A mock IHashCodeExporter that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the HashCodeExporter property.
        /// </summary>
        public Mock<IHashCodeExporter> HashCodeExporterMock { get; }
            = new Mock<IHashCodeExporter>();

        /// <summary>
        /// A mock IPropertyChangedSubscriber that can be used to specify desired behaviour. This will only be
        /// used if no value is specified for the PropertyChangedSubscriber property.
        /// </summary>
        public Mock<IPropertyChangedSubscriber> PropertyChangedSubscriberMock { get; }
            = new Mock<IPropertyChangedSubscriber>();

        /// <summary>
        /// Create a new HashCalculatorViewModel, configured with the properties
        /// available in this HashCalculatorViewModelBuilder instance
        /// </summary>
        /// <returns>A new HashCalculatorViewModel, configured with the properties
        /// available in this HashCalculatorViewModelBuilder instance</returns>
        public HashCalculatorViewModel CreateViewModel()
        {
            var viewModel = new HashCalculatorViewModel(
                DispatcherService ?? DispatcherServiceMock.Object,
                ExportPathPrompter ?? ExportPathPrompterMock.Object,
                FileExistenceChecker ?? FileExistenceCheckerMock.Object,
                FileHashCodeMatchChecker ?? FileHashCodeMatchCheckerMock.Object,
                HashCodeExporter ?? HashCodeExporterMock.Object,
                PropertyChangedSubscriber ?? PropertyChangedSubscriberMock.Object);

            return viewModel;
        }
    }
}
