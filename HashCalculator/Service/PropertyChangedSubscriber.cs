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

using System;
using System.Collections.Generic;
using HashCalculator.Interface;
using System.ComponentModel;
using System.Linq;

namespace HashCalculator.Service
{
    /// <summary>
    /// Provides methods to un/subscribe to
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/>
    /// </summary>
    public class PropertyChangedSubscriber : IPropertyChangedSubscriber
    {
        private readonly HashSet<INotifyPropertyChanged> _subscriptions
            = new HashSet<INotifyPropertyChanged>();

        /// <summary>
        /// A <see cref="PropertyChangedEventHandler"/> to use when subscribing
        /// to <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// </summary>
        public PropertyChangedEventHandler EventHandler { get; set; }

        /// <summary>
        /// Subscribe to <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// of <see cref="obj"/> if not already subscribed
        /// </summary>
        /// <param name="obj">
        /// The <see cref="INotifyPropertyChanged"/> for which
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> will be
        /// subscribed to
        /// </param>
        /// <returns>true if subscribed, false otherwise</returns>
        public bool Subscribe(INotifyPropertyChanged obj)
        {
            VerifyEventHandler();

            var added = false;
            if (!_subscriptions.Contains(obj))
            {
                obj.PropertyChanged += EventHandler;
                _subscriptions.Add(obj);
                added = true;
            }

            return added;
        }

        /// <summary>
        /// Unsubscribe from <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// of <see cref="obj"/> if already subscribed
        /// </summary>
        /// <param name="obj">
        /// The <see cref="INotifyPropertyChanged"/> for which
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> will be
        /// unsubscribed from
        /// </param>
        /// <returns>true if unsubscribed, false otherwise</returns>
        public bool Unsubscribe(INotifyPropertyChanged obj)
        {
            VerifyEventHandler();

            var removed = false;
            if (_subscriptions.Contains(obj))
            {
                obj.PropertyChanged -= EventHandler;
                _subscriptions.Remove(obj);
                removed = true;
            }

            return removed;
        }

        /// <summary>
        /// Unsubscribe from <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// for all previously subscriptions
        /// </summary>
        /// <returns>
        /// An array containing the objects where
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> was unsubscribed
        /// </returns>
        public INotifyPropertyChanged[] UnsubscribeAll()
        {
            var subscriptionsArray = _subscriptions.ToArray();

            foreach (var item in subscriptionsArray)
            {
                Unsubscribe(item);
            }

            return subscriptionsArray;
        }

        private void VerifyEventHandler()
        {
            if (EventHandler == null)
            {
                throw new InvalidOperationException("EventHandler is null");
            }
        }
    }
}
