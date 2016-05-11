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
    public class PropertyChangedSubscriber : IPropertyChangedSubscriber
    {
        private readonly HashSet<INotifyPropertyChanged> _subscriptions = new HashSet<INotifyPropertyChanged>();

        public PropertyChangedEventHandler EventHandler { get; set; }

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
