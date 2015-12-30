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

public sealed class ItemListConfiguration : ConfigurationSection
{
    public static ItemListConfiguration GetConfigurationSection(Configuration config)
    {
        return ConfigurationTools.GetConfigurationSection<ItemListConfiguration>(
                config, "ItemList");
    }

    public uint MaxItems
    {
        get { return (uint)MaxItemsInt; }
        set { MaxItemsInt = (int)value; }
    }

    [ConfigurationProperty("maxItems", DefaultValue = 100)]
    [IntegerValidator(MinValue = 0, MaxValue = 10000, ExcludeRange = false)]
    private int MaxItemsInt
    {
        get { return (int)this["maxItems"]; }
        set { this["maxItems"] = value; }
    }
}
