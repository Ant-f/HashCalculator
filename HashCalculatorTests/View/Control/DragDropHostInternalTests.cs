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
using HashCalculator.View.Control;
using Moq;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace HashCalculatorTests.View.Control
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class DragDropHostInternalTests
    {
        [Test]
        public void DragDropOverlayIsNotHitTestVisibleAfterInitialize()
        {
            // Arrange

            var overlay = new Border();
            var host = new DragDropHostInternal();

            // Act

            host.InitializeDragDropOverlay(overlay);

            // Assert

            Assert.IsFalse(host.DragDropOverlay.IsHitTestVisible);
        }

        [Test]
        public void DragDropOverlayIsHitTestVisibleOnDragEnter()
        {
            // Arrange

            var overlay = new Border();

            var host = new DragDropHostInternal();
            host.InitializeDragDropOverlay(overlay);

            // Act

            host.OnDragEnter();

            // Assert

            Assert.IsTrue(host.DragDropOverlay.IsHitTestVisible);
        }

        [Test]
        public void DragDropOverlayIsNotHitTestVisibleOnDragLeave()
        {
            // Arrange

            var overlay = new Border();

            var host = new DragDropHostInternal();
            host.InitializeDragDropOverlay(overlay);

            // Act

            host.OnDragLeave();

            // Assert

            Assert.IsFalse(host.DragDropOverlay.IsHitTestVisible);
        }

        [Test]
        public void DragDropEffectsIsLinkIfDataContainsFileDropList()
        {
            // Arrange

            var fileDropData = new StringCollection
            {
                "File.txt"
            };

            var host = new DragDropHostInternal();

            var dataObject = new DataObject();
            dataObject.SetFileDropList(fileDropData);

            // Act

            var effects = host.EvaluateDragDropEffects(dataObject);

            // Assert

            Assert.That(effects, Is.EqualTo(DragDropEffects.Link));
        }

        [Test]
        public void FilesDroppedOnOverlayArePassedToInputFilesAppender()
        {
            // Arrange

            string[] addedFiles = null;

            var fileDropData = new StringCollection
            {
                "File1.txt",
                "File2.txt",
                "File3.txt"
            };

            var inputFilesAppender = new Mock<IInputFilesAppender>();
            inputFilesAppender
                .Setup(i => i.AddFilesToInputList(It.IsAny<string[]>()))
                .Callback<string[]>(files => addedFiles = files);
            
            var host = new DragDropHostInternal
            {
                InputFilesAppender = inputFilesAppender.Object
            };

            var overlay = new Border();
            host.InitializeDragDropOverlay(overlay);

            var dataObject = new DataObject();
            dataObject.SetFileDropList(fileDropData);

            // Act

            host.ProcessDropData(dataObject);

            // Assert

            Assert.NotNull(addedFiles);
            CollectionAssert.AreEqual(fileDropData, addedFiles);
        }

        [Test]
        public void DragDropOverlayIsNotHitTestVisibleAfterDataDrop()
        {
            // Arrange

            string[] addedFiles = null;

            var fileDropData = new StringCollection
            {
                "File1.txt"
            };

            var inputFilesAppender = new Mock<IInputFilesAppender>();
            inputFilesAppender
                .Setup(i => i.AddFilesToInputList(It.IsAny<string[]>()))
                .Callback<string[]>(files => addedFiles = files);

            var host = new DragDropHostInternal
            {
                InputFilesAppender = inputFilesAppender.Object
            };

            var overlay = new Border();
            host.InitializeDragDropOverlay(overlay);

            var dataObject = new DataObject();
            dataObject.SetFileDropList(fileDropData);

            // Act

            host.ProcessDropData(dataObject);

            // Assert

            Assert.IsFalse(host.DragDropOverlay.IsHitTestVisible);
        }
    }
}
