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
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HashCalculator.View.Control
{
    /// <summary>
    /// Contains non-view logic for the <see cref="DragDropHost"/> control
    /// </summary>
    internal class DragDropHostInternal
    {
        /// <summary>
        /// Control that will accept <see cref="UIElement.Drop"/> events. This
        /// should only be hit-test-visible on drag enter, meaning that mouse
        /// interactions with the Content of a <see cref="DragDropHost"/> should
        /// be unaffected.
        /// </summary>
        public Border DragDropOverlay { get; private set; }

        /// <summary>
        /// Specifies the <see cref="IInputFilesAppender"/> that will receive
        /// data from a drag and drop operation
        /// </summary>
        public IInputFilesAppender InputFilesAppender { get; set; }

        /// <summary>
        /// Logic that should occur on <see cref="UIElement.DragEnter"/>: sets
        /// <see cref="UIElement.IsHitTestVisible"/> on <see cref="DragDropOverlay"/>
        /// to true
        /// </summary>
        public void OnDragEnter()
        {
            DragDropOverlay.IsHitTestVisible = true;
        }

        /// <summary>
        /// Logic that should occur on <see cref="UIElement.DragEnter"/>: sets
        /// <see cref="UIElement.IsHitTestVisible"/> on <see cref="DragDropOverlay"/>
        /// to false
        /// </summary>
        public void OnDragLeave()
        {
            DragDropOverlay.IsHitTestVisible = false;
        }

        /// <summary>
        /// Evaluates the <see cref="DragDropEffects"/> appropriate for the given
        /// <see cref="IDataObject"/>
        /// </summary>
        /// <param name="dataObject">
        /// An <see cref="IDataObject"/> to evaluate <see cref="DragDropEffects"/>
        /// for
        /// </param>
        /// <returns>
        /// A <see cref="DragDropEffects"/> value appropriate for the given
        /// <see cref="IDataObject"/>
        /// </returns>
        public DragDropEffects EvaluateDragDropEffects(IDataObject dataObject)
        {
            var effects = DragDropEffects.None;
            var data = dataObject as DataObject;

            if (data != null &&
                data.ContainsFileDropList())
            {
                effects = DragDropEffects.Link;
            }

            return effects;
        }

        /// <summary>
        /// Initializes the passed in <see cref="Border"/> to process data on
        /// <see cref="UIElement.Drop"/> events
        /// </summary>
        /// <param name="overlay">The <see cref="Border"/> to initialize</param>
        public void InitializeDragDropOverlay(Border overlay)
        {
            DragDropOverlay = overlay;
            DragDropOverlay.IsHitTestVisible = false;
            DragDropOverlay.Drop += (sender, args) =>
            {
                var data = args.Data as DataObject;
                ProcessDropData(data);
            };
        }

        /// <summary>
        /// Process a <see cref="DataObject"/> from <see cref="DragEventArgs.Data"/>,
        /// passing the file drop list to <see cref="InputFilesAppender"/>.
        /// Refactored to a separate method to facilitate testing, as
        /// <see cref="DragEventArgs"/> is sealed and has an internal constructor
        /// </summary>
        /// <param name="data">
        /// A <see cref="DataObject"/> containing file paths to pass to
        /// <see cref="InputFilesAppender"/>
        /// </param>
        internal void ProcessDropData(DataObject data)
        {
            if (data != null &&
                data.ContainsFileDropList())
            {
                var collection = data.GetFileDropList();
                InputFilesAppender.AddFilesToInputList(collection.Cast<string>().ToArray());
            }

            DragDropOverlay.IsHitTestVisible = false;
        }
    }
}
