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
using System.Windows;
using System.Windows.Controls;

namespace HashCalculator.View.Control
{
    /// <summary>
    /// Content control that acts as a drag and drop host to a single child
    /// element, negating the need for code behind in the UI for drag and drop
    /// event handlers.
    /// </summary>
    [TemplatePart(Name = DragDropOverlayPartName, Type = typeof(Border))]
    public class DragDropHost : ContentControl
    {
        private const string DragDropOverlayPartName = "PART_DragDropOverlay";

        private readonly DragDropHostInternal _dragDropHostInternal;

        /// <summary>
        /// Identifies the <see cref="DropReceiver"/> dependency property
        /// </summary>
        public static readonly DependencyProperty DropReceiverProperty
            = DependencyProperty.Register(
                "DropReceiver",
                typeof(IInputFilesAppender),
                typeof(DragDropHost),
                new PropertyMetadata((d, e) =>
                {
                    var host = (DragDropHost) d;
                    var appender = (IInputFilesAppender) e.NewValue;
                    host._dragDropHostInternal.InputFilesAppender = appender;
                }));

        /// <summary>
        /// Specifies the <see cref="IInputFilesAppender"/> that will receive
        /// data from a drag and drop operation
        /// </summary>
        public IInputFilesAppender DropReceiver
        {
            get
            {
                return (IInputFilesAppender) GetValue(DropReceiverProperty);
            }
            set
            {
                SetValue(DropReceiverProperty, value);
            }
        }

        public DragDropHost()
        {
            _dragDropHostInternal = new DragDropHostInternal();
            DefaultStyleKey = typeof(DragDropHost);
            AllowDrop = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            _dragDropHostInternal.OnDragEnter();
            base.OnDragEnter(e);
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            _dragDropHostInternal.OnDragLeave();
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effects = _dragDropHostInternal.EvaluateDragDropEffects(e.Data);

            // Required so Effects set above are not overwritten
            e.Handled = true;

            base.OnDragOver(e);
        }

        public override void OnApplyTemplate()
        {
            var overlay = GetTemplateChild(DragDropOverlayPartName) as Border;
            _dragDropHostInternal.InitializeDragDropOverlay(overlay);
            base.OnApplyTemplate();
        }
    }
}
