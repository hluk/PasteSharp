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
using System.IO;
using System.Reflection;

using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

public partial class MainWindow : Gtk.Window
{
    //[UI] Gtk.Entry searchEntry;
    [UI] Gtk.TreeView tree;

    //ClipboardItemStore store;

    private static string MainWindowUi()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "CopySharp.interfaces.MainWindow.glade";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName)) {
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }

    public static MainWindow Create()
    {
        var builder = new Builder();
        builder.AddFromString(MainWindowUi());
        return new MainWindow(builder, builder.GetObject("window1").Handle);
    }

    protected MainWindow(Builder builder, IntPtr handle) : base(handle)
    {
        builder.Autoconnect(this);

        var store = new ClipboardItemStore();
        tree.Model = store;

        DeleteEvent += OnDeleteEvent;
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
