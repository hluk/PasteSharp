/*
    Copyright (c) 2016, Lukas Holecek <hluk@email.cz>

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

using NUnit.Framework;
using System;

[TestFixture]
public class SqliteItemStoreTest : Assert {
    SqliteItemStore store;

    [SetUp]
    public void GetReady()
    {
        store = new SqliteItemStore();
        store.Clear();
    }

    [TearDown]
    public void Clean()
    {
        store.Clear();
    }

    [Test]
    public void AppendItems()
    {
        Assert.AreEqual(0, store.RowCount());

        store.AddText("1");
        Assert.AreEqual(1, store.RowCount());
        Assert.AreEqual("1", store.GetText(0));

        store.AddText("2");
        Assert.AreEqual(2, store.RowCount());
        Assert.AreEqual("2", store.GetText(1));
        Assert.AreEqual("1", store.GetText(0));
    }
}
