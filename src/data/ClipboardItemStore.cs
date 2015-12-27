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

using Gtk;

public class ClipboardItemStore : Gtk.ListStore
{
    private static TreeIter GetIter(ITreeModel model, TreePath path)
    {
        TreeIter iter;
        model.GetIter(out iter, path);
        return iter;
    }

    private static TreeIter GetIter(ITreeModel model, int row)
    {
        TreeIter rootIter;
        model.GetIterFirst(out rootIter);

        var path = new TreePath(new int[]{row});
        return GetIter(model, path);
    }

    public static string GetText(ITreeModel model, TreeIter iter)
    {
        return model.GetValue(iter, column:0) as string;
    }

    public static string GetText(ITreeModel model, TreePath path)
    {
        var iter = GetIter(model, path);
        return GetText(model, iter);
    }

    public ClipboardItemStore() : base(typeof(string), typeof(DateTime))
    {
    }

    public void AddText(string text)
    {
        InsertWithValues(0, text, DateTime.Now);
    }

    public void RemoveItems(TreePath[] paths)
    {
        // Remove rows from bottom to top so as not to invalidate paths.
        int[] rows = new int[paths.Length];
        for (int i = 0; i < paths.Length; ++i)
            rows[i] = paths[i].Indices[0];
        Array.Sort(rows, (lhs, rhs) => rhs.CompareTo(lhs));

        foreach (int row in rows) {
            var iter = GetIter(this, row);
            Remove(ref iter);
        }
    }

    public string GetText(int row)
    {
        return GetText(this, GetIter(row));
    }

    public int Count
    {
        get { return IterNChildren(); }
    }

    public TreeIter GetIter(int row)
    {
        return GetIter(this, row);
    }
}

