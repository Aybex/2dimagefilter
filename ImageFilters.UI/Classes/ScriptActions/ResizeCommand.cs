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
using ImageFilters.Library.Imager.Interface;
using ImageFilters.UI.Classes.ImageManipulators;
using word = System.UInt16;

namespace ImageFilters.UI.Classes.ScriptActions; 

public class ResizeCommand : IScriptAction {
  #region Implementation of IScriptAction
  public bool ChangesSourceImage => false;
  public bool ChangesTargetImage => true;
  public bool ProvidesNewGdiSource => false;

  public bool Execute() {
    var source = _applyToTarget ? TargetImage : SourceImage;

    var width = Width;
    var height = Height;

    // pverwrite dimensions from percentage if needed
    var percentage = Percentage;
    if (percentage > 0) {
      width = (word)Math.Round(source.Width * percentage / 100d);
      height = (word)Math.Round(source.Height * percentage / 100d);
    }

    // correct aspect ratio if needed
    if (MaintainAspect) {
      if (width == 0) {
        width = (word)Math.Round((double)height * source.Width / source.Height);
      } else {
        height = (word)Math.Round((double)width * source.Height / source.Width);
      }
    }

    sPixel.AllowThresholds = UseThresholds;
    source.HorizontalOutOfBoundsMode = HorizontalBph;
    source.VerticalOutOfBoundsMode = VerticalBph;

    cImage result = null;
    var method = Manipulator;
    var scaler = method as AScaler;
    var interpolator = method as Interpolator;
    var planeExtractor = method as PlaneExtractor;
    var resampler = method as Resampler;
    var radiusResampler = method as RadiusResampler;

    if (scaler != null) {
      result = source;
      for (var i = 0; i < Count; i++)
        result = scaler.Apply(result);
    } else {
      if (interpolator != null)
        result = interpolator.Apply(source, width, height);
      else if (planeExtractor != null)
        result = planeExtractor.Apply(source);
      else if (resampler != null)
        result = resampler.Apply(source, width, height, UseCenteredGrid);
      else if (radiusResampler != null)
        result = radiusResampler.Apply(source, width, height, Radius, UseCenteredGrid);
    }

    TargetImage = result;
    return true;
  }

  public Bitmap GdiSource => null;
  public cImage SourceImage { get; set; }
  public cImage TargetImage { get; set; }
    
  #endregion

  public IImageManipulator Manipulator { get; }
  public word Width { get; }
  public word Height { get; }
  public bool MaintainAspect { get; }
  public OutOfBoundsMode HorizontalBph { get; }
  public OutOfBoundsMode VerticalBph { get; }
  public byte Count { get; }
  public bool UseThresholds { get; }
  public bool UseCenteredGrid { get; }
  public float Radius { get; }
  public word Percentage { get; }

  private readonly bool _applyToTarget;

  public ResizeCommand(bool applyToTarget, IImageManipulator manipulator, word width, word height, word percentage, bool maintainAspect, OutOfBoundsMode horizontalBph, OutOfBoundsMode verticalBph, byte count, bool useThresholds, bool useCenteredGrid, float radius) {
    _applyToTarget = applyToTarget;
    Manipulator = manipulator;
    Width = width;
    Height = height;
    MaintainAspect = maintainAspect;
    HorizontalBph = horizontalBph;
    VerticalBph = verticalBph;
    Count = count;
    UseThresholds = useThresholds;
    UseCenteredGrid = useCenteredGrid;
    Radius = radius;
    Percentage = percentage;
  }
}