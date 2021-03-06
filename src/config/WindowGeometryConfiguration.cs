/*
    Copyright (c) 2016, Lukas Holecek <hluk@email.cz>

    This file is part of PasteSharp.

    PasteSharp is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PasteSharp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PasteSharp.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Configuration;

public sealed class WindowGeometryConfiguration : ConfigurationSection
{
    public static Configuration GeometryConfiguration()
    {
        return ConfigurationTools.GetConfiguration();
    }

    public static WindowGeometryConfiguration GeometryConfigurationSection(
           Configuration config, string sectionName)
    {
        return ConfigurationTools.GetConfigurationSection<WindowGeometryConfiguration>(
                config, sectionName);
    }

    [ConfigurationProperty("x", DefaultValue = 0)]
    public int X
    {
        get { return (int)this["x"]; }
        set { this["x"] = value; }
    }

    [ConfigurationProperty("y", DefaultValue = 0)]
    public int Y
    {
        get { return (int)this["y"]; }
        set { this["y"] = value; }
    }

    [ConfigurationProperty("width", DefaultValue = 250)]
    public int Width
    {
        get { return (int)this["width"]; }
        set { this["width"] = value; }
    }

    [ConfigurationProperty("height", DefaultValue = 350)]
    public int Height
    {
        get { return (int)this["height"]; }
        set { this["height"] = value; }
    }
}
