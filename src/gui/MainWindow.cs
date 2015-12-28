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
using System.Reflection;
using System.Configuration;

using Gtk;

public class MainWindow : Gtk.Window
{
    SearchEntry searchEntry;
    ClipboardItemListView clipboardItemListView;
    ClipboardManager clipboardManager;

    private static string GetWindowTitle()
    {
        return Assembly.GetExecutingAssembly().GetName().Name;
    }

    private static WindowGeometryConfiguration GeometryConfigurationSection(
            Configuration config)
    {
        return WindowGeometryConfiguration.GeometryConfigurationSection(
                config, "MainWindowGeometry");
    }

    public MainWindow() : base(GetWindowTitle())
    {
        var box = new VBox();
        Add(box);

        searchEntry = new SearchEntry();
        searchEntry.Changed += OnSearch;
        box.PackStart(searchEntry, expand:false, fill:true, padding:0);

        clipboardItemListView = new ClipboardItemListView();
        clipboardItemListView.ItemsActivatedEvent += OnItemsActivated;
        box.PackStart(clipboardItemListView, expand:true, fill:true, padding:0);

        clipboardManager = new ClipboardManager();
        clipboardManager.ClipboardTextChangedEvent += OnClipboardTextChanged;

        LoadGeometry();
        ShowAll();

        DeleteEvent += OnDeleteEvent;
    }

    private void SaveGeometry()
    {
        var config = WindowGeometryConfiguration.GeometryConfiguration();
        var configSection = GeometryConfigurationSection(config);

        int width;
        int height;
        GetSize(out width, out height);
        configSection.Width = width;
        configSection.Height = height;

        int x;
        int y;
        GetPosition(out x, out y);
        configSection.X = x;
        configSection.Y = y;

        try {
            config.Save();
        } catch (ConfigurationErrorsException e) {
            Console.WriteLine("Error saving window geometry to \""
                    + config.FilePath + "\": " + e.Message);
        }
    }

    private void LoadGeometry()
    {
        var config = WindowGeometryConfiguration.GeometryConfiguration();
        var configSection = GeometryConfigurationSection(config);
        SetSizeRequest(configSection.Width, configSection.Height);
        Move(configSection.X, configSection.Y);
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        SaveGeometry();
        Application.Quit();
        a.RetVal = true;
    }

    private void OnSearch(object sender, EventArgs a)
    {
        clipboardItemListView.Filter = searchEntry.Text;
    }

    private void OnItemsActivated(object sender, ItemsActivatedEventArgs a)
    {
        clipboardManager.Text = a.ItemText;
        Iconify();
    }

    private void OnClipboardTextChanged(object sender, ClipboardTextChangedEventArgs a)
    {
        if (!string.IsNullOrEmpty(a.Text))
        clipboardItemListView.AddText(a.Text);
    }
}
