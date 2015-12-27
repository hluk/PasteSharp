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

using NUnit.Framework;
using System;

using Gtk;

[TestFixture]
public class ClipboardItemStoreTest : Assert {
    ClipboardItemStore store;

    private TreePath[] TreePaths(int[] rows)
    {
        var paths = new TreePath[rows.Length];

        for (int i = 0; i < rows.Length; ++i)
            paths[i] = new TreePath(rows[i].ToString());

        return paths;
    }

    [SetUp]
    public void GetReady()
    {
        store = new ClipboardItemStore();
    }

    [TearDown]
    public void Clean()
    {
    }

    [Test]
    public void AppendItems()
    {
        store.AddText("1");
        Assert.AreEqual(1, store.Count);
        Assert.AreEqual("1", store.GetText(0));

        store.AddText("2");
        Assert.AreEqual(2, store.Count);
        Assert.AreEqual("2", store.GetText(0));
    }

    [Test]
    public void RemoveItem()
    {
        store.AddText("1");
        var paths = TreePaths(new int[]{0});
        store.RemoveItems(paths);
        Assert.AreEqual(0, store.Count);
    }

    // FIXME: Parametrized test doen't seem to work.
    public void RemoveItems(int[] rows)
    {
        store.AddText("1");
        store.AddText("2");
        store.AddText("3");
        store.AddText("4");

        var paths = TreePaths(rows);
        store.RemoveItems(paths);
        Assert.AreEqual(2, store.Count);
        Assert.AreEqual("4", store.GetText(0));
        Assert.AreEqual("1", store.GetText(1));
    }

    [Test]
    public void RemoveItems()
    {
        RemoveItems(new int[]{1,2});
    }

    [Test]
    public void RemoveItemsReversed()
    {
        RemoveItems(new int[]{2,1});
    }
}
