﻿#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

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

using ImageFilters.Library.Imager;

namespace ImageFilters.UI.Classes.ScriptActions; 

public class TargetToSourceCommand : IScriptAction {
  #region Implementation of IScriptAction
  public bool ChangesSourceImage => true;
  public bool ChangesTargetImage => true;
  public bool ProvidesNewGdiSource => false;

  public bool Execute() {
    SourceImage = TargetImage;
    TargetImage = null;
    return true;
  }

  public Bitmap GdiSource => null;

  public cImage SourceImage { get; set; }

  public cImage TargetImage { get; set; }
  #endregion
}