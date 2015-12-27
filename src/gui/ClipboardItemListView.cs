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
using System.Text.RegularExpressions;

using Gtk;

public class ClipboardItemListView : Gtk.TreeView
{
    ClipboardItemStore store;
    TreeModelSort modelSort;
    TreeModelFilter modelFilter;

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

        modelFilter = new TreeModelFilter(store, null);
        modelFilter.VisibleFunc = FilterFunc;

        modelSort = new TreeModelSort(modelFilter);
        modelSort.SetSortFunc(1, CompareDateTime);

        Model = modelSort;

        ClipboardNotifier.registerCallback(store.AddText);
    }

    private Regex filter;
    public string Filter {
        set {
            try {
                filter = new Regex(value, RegexOptions.IgnoreCase);
            } catch (ArgumentException) {
                filter = null;
            }
            modelFilter.Refilter();
        }
    }

    protected override bool OnKeyPressEvent(Gdk.EventKey ev)
    {
        switch (ev.Key) {
            case Gdk.Key.Return:
            case Gdk.Key.KP_Enter:
                activateSelection();
                return true;

            case Gdk.Key.Delete:
                deleteSelection();
                return true;

            default:
                return base.OnKeyPressEvent(ev);
        }
    }

    protected override void OnRowActivated(TreePath path, TreeViewColumn column)
    {
        var text = ClipboardItemStore.GetText(Model, path);
        SetClipboardText(text);
    }

    private void activateSelection()
    {
        var text = "";

        Selection.SelectedForeach(
                (model, path, iter) => {
                    text += ClipboardItemStore.GetText(Model, path) + "\n";
                });

        SetClipboardText(text.Substring(0, text.Length - 1));
    }

    private void deleteSelection()
    {
        var paths = Selection.GetSelectedRows();

        for (int i = 0; i < paths.Length; ++i) {
            var path1 = modelSort.ConvertPathToChildPath(paths[i]);
            paths[i] = modelFilter.ConvertPathToChildPath(path1);
        }

        store.RemoveItems(paths);
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

    private bool FilterFunc(ITreeModel model, TreeIter iter)
    {
        if (filter == null)
            return true;

        var text = ClipboardItemStore.GetText(model, iter);
        return filter.IsMatch(text);
    }
}
