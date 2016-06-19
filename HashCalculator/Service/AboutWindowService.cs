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

using System.IO;
using System.Reflection;
using System.Windows;
using HashCalculator.Interface;
using HashCalculator.View;

namespace HashCalculator.Service
{
    public class AboutWindowService : IAboutWindowService
    {
        public string Apache2 { get; }
        public string Gpl3 { get; }

        public AboutWindowService()
        {
            const string apache2ResourceName =
                "HashCalculator.License.Ninject.ApacheLicenseVersion2.0.txt";

            const string gpl3ResourceName = "HashCalculator.License.LICENSE";

            Apache2 = GetLicenseText(apache2ResourceName);
            Gpl3 = GetLicenseText(gpl3ResourceName);
        }

        /// <summary>
        /// Retrieve text from an embedded resource
        /// </summary>
        /// <param name="resourceName">Name of the resource to read</param>
        /// <returns>The entire contents of the named resource</returns>
        private string GetLicenseText(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string licenseText;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    licenseText = streamReader.ReadToEnd();
                }
            }

            return licenseText;
        }

        /// <summary>
        /// Create a new window that displays information about the appplication
        /// </summary>
        /// <param name="owner">
        /// A <see cref="Window"/> that will be set as the owner of the
        /// returned window
        /// </param>
        /// <returns>
        /// A new window that displays information about this appplication
        /// </returns>
        public About CreateAboutWindow(Window owner)
        {
            var about = new About
            {
                Owner = owner
            };

            return about;
        }

        public void ShowModal(About aboutWindow)
        {
            aboutWindow.ShowDialog();
        }
    }
}
