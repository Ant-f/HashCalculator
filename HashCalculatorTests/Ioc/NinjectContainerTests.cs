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

using HashCalculator.Ioc;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace HashCalculatorTests.Ioc
{
    [TestFixture]
    public class NinjectContainerTests
    {
        [Test]
        public void InterfaceBindingResolution()
        {
            var typesToResolve = new List<Type>
            {
                typeof(ICommand)
            };

            var assembly = Assembly.GetAssembly(typeof(NinjectContainer));
            var assemblyInterfaces = assembly.GetTypes().Where(t => t.IsInterface);

            typesToResolve.AddRange(assemblyInterfaces);

            foreach (var interfaceType in typesToResolve)
            {
                var objects = NinjectContainer.Kernel.GetAll(interfaceType);
                Assert.IsNotEmpty(objects, $"Unable to resolve instance(s) of {interfaceType.Name}");
            }
        }
    }
}
