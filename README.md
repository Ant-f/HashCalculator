# File Hash Calculator
File Hash Calculator is a small Windows utility that can calculate and compare file hash sums. The following algorithms are supported:

* MD5
* SHA1
* SHA256
* SHA512

## Requirements

File Hash Calculator requires .NET Framework 4.6.1 - this is supported on Windows
7 (SP1) and upwards.

## Installation

Pre-built binaries are available on the [Releases](https://github.com/Ant-f/HashCalculator/releases) page.

Alternatively, you can build your own binaries from source if you prefer. Once
built, the following files are required for correct operation:

* HashCalculator.exe
* Ninject.dll

## Features

1. Can generate hash sums using MD5, SHA1, SHA256, and SHA512 algorithms

2. Easy to use: simply drag and drop to add files. Known hash sums can optionally
  be added for comparison, with visual feedback

3. Small footprint: the files required for operation total approximately 260 KB

## Usage

* Drag files from Windows File Explorer and drop them onto the _Input Files_
  area

* Edit the file _Path_ if needed for different files. The cell background will
  change colour if the file path is invalid.

* Remove files by either deleting the entire path, or by clicking on the button
  in the right-most column

* Select the algorithm to use in the drop-down box near the bottom of the app window

* Optionally enter known hash sums that should match the calculated hash sums in
  the format

  HashSum *FileName/FilePath

* Click _Calculate_

* The hash sums and file names/paths will be checked for matches. To match
  files using the filename and extension only, leave _Full Path_ unselected; to
  match using the full file path, select _Full Path_

* While computing the hash sums of multiple files, it is possible to stop the
  process after the computation of the current in-progress file by clicking
  _Cancel_

* To export the calculated hash sums to a file for future reference, click
  _Export_

## License

File Hash Calculator is distributable under the terms and conditions of the GNU
General Public License Version 3. The full text of the GNU General Public License
Version 3 can be viewed [here](https://github.com/Ant-f/HashCalculator/blob/master/LICENSE).

File Hash Calculator uses the third party open source library Ninject under the
terms and conditions of the Apache License Version 2.0. The full text of the
Apache License Version 2.0 can be viewed [here](https://github.com/Ant-f/HashCalculator/blob/master/Ninject/ApacheLicenseVersion2.0.txt).
