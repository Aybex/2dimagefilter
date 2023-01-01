#region (c)2008-2019 Hawkynt
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

using Imager;
using Imager.Interface;

namespace Classes.ImageManipulators.Scalers; 

public class NqScaler : AScaler {
  private readonly NqScalerType _type;
  private readonly NqMode _mode;

  #region Implementation of AScaler
  public override cImage Apply(cImage source) => source.ApplyScaler(_type, _mode, default(Rectangle?));
  public override byte ScaleFactorX { get; }
  public override byte ScaleFactorY { get; }
  public override string Description => ReflectionUtils.GetDescriptionForEnumValue(_type);

  #endregion

  public NqScaler(NqScalerType type, NqMode mode) {
    var info = cImage.GetPixelScalerInfo(type);
    _type = type;
    _mode = mode;
    ScaleFactorX = info.Item1;
    ScaleFactorY = info.Item2;
  }
}