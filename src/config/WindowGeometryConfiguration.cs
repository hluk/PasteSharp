/*
    Copyright (c) 2015, Lukas Holecek <hluk@email.cz>

    This file is part of CopyQ.

    CopyQ is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    CopyQ is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with CopyQ.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Configuration;

public sealed class WindowGeometryConfiguration : ConfigurationSection
{
    public static Configuration GeometryConfiguration()
    {
        return ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal);
    }

    public static WindowGeometryConfiguration GeometryConfigurationSection(
           Configuration config, string sectionName)
    {
        try {
            var configSection =
                config.GetSection("MainWindowGeometry") as WindowGeometryConfiguration;

            if (configSection == null) {
                configSection = new WindowGeometryConfiguration();
                config.Sections.Add("MainWindowGeometry", configSection);
            }

            return configSection;
        } catch (ConfigurationErrorsException e) {
            Console.WriteLine(
                    "Error loading window geometry from \""
                    + config.FilePath + "\": " + e.Message);
            return new WindowGeometryConfiguration();
        }
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
