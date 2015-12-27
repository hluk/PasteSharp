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

using Gtk;

public class MainWindow : Gtk.Window
{
    SearchEntry searchEntry;
    ClipboardItemListView clipboardItemListView;

    static private string GetWindowTitle()
    {
        return Assembly.GetExecutingAssembly().GetName().Name;
    }

    public MainWindow() : base(GetWindowTitle())
    {
        var box = new VBox();
        Add(box);

        searchEntry = new SearchEntry();
        searchEntry.Changed += OnSearch;
        box.PackStart(searchEntry, expand:false, fill:true, padding:0);

        clipboardItemListView = new ClipboardItemListView();
        box.PackStart(clipboardItemListView, expand:true, fill:true, padding:0);

        SetSizeRequest(250, 350);
        ShowAll();

        DeleteEvent += OnDeleteEvent;
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    private void OnSearch(object sender, EventArgs a)
    {
        clipboardItemListView.Filter = searchEntry.Text;
    }
}
