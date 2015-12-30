/*
    Copyright (c) 2015, Lukas Holecek <hluk@email.cz>

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

public sealed class ConfigurationTools
{
    public static Configuration GetConfiguration()
    {
        return ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal);
    }

    public static T GetConfigurationSection<T>(
           Configuration config, string sectionName)
        where T : ConfigurationSection, new()
    {
        try {
            var configSection = config.GetSection(sectionName) as T;

            if (configSection == null) {
                configSection = new T();
                config.Sections.Add(sectionName, configSection);
                config.Save(ConfigurationSaveMode.Full);
            }

            return configSection;
        } catch (ConfigurationErrorsException e) {
            Console.WriteLine(
                    "Error loading configuration setion \""
                    + sectionName + "\" from \""
                    + config.FilePath + "\": " + e.Message);
            return new T();
        }
    }
}

