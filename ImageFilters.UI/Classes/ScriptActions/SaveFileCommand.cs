﻿#region (c)2008-2019 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2019 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System.Diagnostics.Contracts;
using ImageFilters.Library.Imager;

namespace ImageFilters.UI.Classes.ScriptActions; 

public class SaveFileCommand : IScriptAction {
  #region Implementation of IScriptAction
  public bool ChangesSourceImage => false;

  public bool ChangesTargetImage => false;
  public bool ProvidesNewGdiSource => false;

  public bool Execute() {
    var result = CLI.SaveHelper(FileName, TargetImage.ToBitmap());
    if (result == CLIExitCode.NothingToSave)
      throw new NullReferenceException("Nothing to save");
    if (result == CLIExitCode.JpegNotSupportedOnThisPlatform)
      throw new InvalidOperationException("Jpeg not supported");

    return result == CLIExitCode.OK;
  }

  public Bitmap GdiSource => null;

  public cImage SourceImage { get; set; }

  public cImage TargetImage { get; set; }
  #endregion

  public string FileName { get; }

  public SaveFileCommand(string fileName) {
    Contract.Requires(!string.IsNullOrWhiteSpace(fileName));
    FileName = fileName;
  }
}