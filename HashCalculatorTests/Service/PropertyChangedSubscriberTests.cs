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

using HashCalculator.Service;
using HashCalculator.ViewModel.Model;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;

namespace HashCalculatorTests.Service
{
    [TestFixture]
    public class PropertyChangedSubscriberTests
    {
        [Test]
        public void SubscribingWithNullEventHandlerThrowsInvalidOperationException()
        {
            var subscriber = new PropertyChangedSubscriber();
            var entry = new InputFileListEntry("path");

            Assert.Throws<InvalidOperationException>(() =>subscriber.Subscribe(entry));
        }

        [Test]
        public void UnsubscribingWithNullEventHandlerThrowsInvalidOperationException()
        {
            var subscriber = new PropertyChangedSubscriber();
            var entry = new InputFileListEntry("path");

            Assert.Throws<InvalidOperationException>(() => subscriber.Subscribe(entry));
        }

        [Test]
        public void EventHandlerIsCalledWhenEventIsRaisedAfterSubscribing()
        {
            // Arrange

            var handlerInvoked = false;
            PropertyChangedEventHandler eventHandler = (sender, e) => { handlerInvoked = true; };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry = new InputFileListEntry("path");
            subscriber.Subscribe(entry);

            // Act

            entry.FilePath = "NewPath";
            subscriber.Unsubscribe(entry);

            // Assert

            Assert.IsTrue(handlerInvoked);
        }

        [Test]
        public void SubscribeReturnsTrueIfNotAlreadySubscribed()
        {
            // Arrange

            PropertyChangedEventHandler eventHandler = (sender, e) => { };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry = new InputFileListEntry("path");

            // Act

            var success = subscriber.Subscribe(entry);
            subscriber.Unsubscribe(entry);

            // Arrange

            Assert.IsTrue(success);
        }

        [Test]
        public void SubscribeReturnsFalseWhenAlreadySubscribed()
        {
            // Arrange

            PropertyChangedEventHandler eventHandler = (sender, e) => { };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry = new InputFileListEntry("path");
            subscriber.Subscribe(entry);

            // Act

            var success = subscriber.Subscribe(entry);
            subscriber.Unsubscribe(entry);

            // Arrange

            Assert.IsFalse(success);
        }

        [Test]
        public void UnsubscribeReturnsTrueIfSubscribed()
        {
            // Arrange

            PropertyChangedEventHandler eventHandler = (sender, e) => { };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry = new InputFileListEntry("path");
            subscriber.Subscribe(entry);

            // Act

            var success = subscriber.Unsubscribe(entry);

            // Arrange

            Assert.IsTrue(success);
        }

        [Test]
        public void UnsubscribeReturnsFalseWhenNotSubscribed()
        {
            // Arrange

            PropertyChangedEventHandler eventHandler = (sender, e) => { };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry = new InputFileListEntry("path");

            // Act

            var success = subscriber.Unsubscribe(entry);

            // Arrange

            Assert.IsFalse(success);
        }

        [Test]
        public void UnsubscribeAllRemovesAllSubscriptions()
        {
            // Arrange

            PropertyChangedEventHandler eventHandler = (sender, e) => { };

            var subscriber = new PropertyChangedSubscriber
            {
                EventHandler = eventHandler
            };

            var entry1 = new InputFileListEntry("path1");
            var entry2 = new InputFileListEntry("path2");

            subscriber.Subscribe(entry1);
            subscriber.Subscribe(entry2);

            // Act

            var entries = subscriber.UnsubscribeAll();

            // Arrange

            Assert.AreEqual(2, entries.Length);
            Assert.That(entries, Contains.Item(entry1));
            Assert.That(entries, Contains.Item(entry2));
        }
    }
}
