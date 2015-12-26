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

[TestFixture]
public class ClipboardItemStoreTest : Assert {
    ClipboardItemStore store;

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
            Assert.AreEqual(store.Count, 1);
            Assert.AreEqual(store.GetText(0), "1");

            store.AddText("2");
            Assert.AreEqual(store.Count, 2);
            Assert.AreEqual(store.GetText(0), "2");
        }
}