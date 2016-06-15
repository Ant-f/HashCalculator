# HashCalculator
Tool for calculating and comparing file hash sums. Can calculate MD5, SHA1,
SHA256, and SHA512.

This is a re-architecture/rewrite of a small WPF program I wrote that can
calculate and compare file hash sums on Windows.

## Installation

Once built, the following files are required for correct operation

* HashCalculator.exe
* Ninject.dll

## Features

1. MD5, SHA1, SHA256, and SHA512 algorithms

2. Easy to use. Drag and Drop to add files. Optionally compares calculated hash
   sums and offers visual feedback

3. Small footprint. The files required for operation total approximately 260 KB

## Usage

* Drag files from Windows File Explorer and drop them onto the _Input Files_
  area

* Edit the file _Path_ if needed for different files. The cell background will
  change colour if the file path is invalid.

* To remove files, either delete the entire path or click on the button in the
  right-most column

* Select the hash algorithm to use in the combo box near the bottom of the
  program window

* Optionally enter hash sums that should match the calculated hash sums in the
  format

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

This program is distributable under the terms and conditions of the GNU General
Public License Version 3; see LICENSE for more information.

This program uses the third party open source software Ninject under the terms
and conditions of the Apache License Version 2.0; for more information see
./Ninject/ApacheLicenseVersion2.0.txt
