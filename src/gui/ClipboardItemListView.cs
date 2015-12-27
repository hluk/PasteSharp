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

public class ClipboardItemListView : Gtk.TreeView
{
    ClipboardItemStore store;

    private static void SetColumnSortable(TreeViewColumn column, int columnId)
    {
        column.SortIndicator = true;
        column.SortColumnId = columnId;
        column.Clickable = true;
    }

    private static void SetDateTimeRendererText(
            TreeViewColumn column, CellRenderer renderer, ITreeModel model, TreeIter iter)
    {
        var dateTime = (DateTime)model.GetValue(iter, 1);
        var textRenderer = (CellRendererText)renderer;
        textRenderer.Text = dateTime.ToString();
    }

    private static int CompareDateTime(ITreeModel model, TreeIter lhs, TreeIter rhs)
    {
        var l = (DateTime)model.GetValue(lhs, 1);
        var r = (DateTime)model.GetValue(rhs, 1);
        return DateTime.Compare(l, r);
    }

    public ClipboardItemListView()
    {
        HeadersVisible = true;

        Selection.Mode = SelectionMode.Multiple;

        int columnId = -1;

        var renderer = new CellRendererText();

        var textColumn = AppendColumn("Items", renderer, "text", 0);
        SetColumnSortable(textColumn, ++columnId);

        var dateTimeColumn = AppendColumn("Created", renderer, SetDateTimeRendererText);
        SetColumnSortable(dateTimeColumn, ++columnId);

        store = new ClipboardItemStore();
        var sortableStore = new TreeModelSort(store);
        sortableStore.SetSortFunc(1, CompareDateTime);
        Model = sortableStore;

        ClipboardNotifier.registerCallback(store.AddText);
    }

    protected override bool OnKeyPressEvent(Gdk.EventKey ev)
    {
        switch (ev.Key) {
            case Gdk.Key.Return:
            case Gdk.Key.KP_Enter:
                OnSelectionActivated();
                return true;

            default:
                return base.OnKeyPressEvent(ev);
        }
    }

    protected override void OnRowActivated(TreePath path, TreeViewColumn column)
    {
        var text = store.GetText(path);
        SetClipboardText(text);
    }

    private void OnSelectionActivated()
    {
        var text = "";

        Selection.SelectedForeach(
                (model, path, iter) => {
                    text += store.GetText(path) + "\n";
                });

        SetClipboardText(text.Substring(0, text.Length - 1));
    }

    private void Iconify()
    {
        if (Window != null)
            Window.Iconify();
    }

    private void SetClipboardText(string text)
    {
        ClipboardNotifier.GetClipboard().Text = text;
        Iconify();
    }
}
