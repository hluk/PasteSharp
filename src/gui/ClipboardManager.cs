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

public class ClipboardTextChangedEventArgs : EventArgs
{
    public string Text {
        get;
        private set;
    }

    public ClipboardTextChangedEventArgs(string text)
    {
        Text = text;
    }
}

public class ClipboardManager
{
    enum DataType { PlainText, Ignore };

    public delegate void ClipboardTextChangedEventHandler(object sender, ClipboardTextChangedEventArgs a);
    public event ClipboardTextChangedEventHandler ClipboardTextChangedEvent;

    private string text;
    public string Text {
        get { return text; }
        set {
            text = value;
            SetClipboard();
        }
    }

    private static Clipboard GetClipboard()
    {
        var atom = Gdk.Atom.Intern("CLIPBOARD", false);
        return Clipboard.Get(atom);
    }

    public ClipboardManager()
    {
        GetClipboard().OwnerChange += OnClipboardChanged;
    }

    private void SetClipboard()
    {
        var targets = new TargetEntry[]{
            new TargetEntry("UTF8_STRING", TargetFlags.OtherApp, (uint)DataType.PlainText),
            new TargetEntry("application/x-copysharp-ignore", TargetFlags.OtherApp, (uint)DataType.Ignore),
        };
        GetClipboard().Clear();
        GetClipboard().SetWithData(targets, GetClipboardData, ClearClipboardData);
    }

    private void GetClipboardText(SelectionData data)
    {
        var atom = Gdk.Atom.Intern("UTF8_STRING", false);
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);
        data.Set(atom, 8, bytes);
    }

    private void GetClipboardIgnore(SelectionData data)
    {
        var atom = Gdk.Atom.Intern("application/x-copysharp-ignore", false);
        data.Set(atom, 8, new byte[]{});
    }

    private void GetClipboardData(Clipboard clipboard, SelectionData data, uint info)
    {
        switch ((DataType)info) {
            case DataType.PlainText:
                GetClipboardText(data);
                break;

            case DataType.Ignore:
                GetClipboardIgnore(data);
                break;
        }
    }

    private void ClearClipboardData(Clipboard clipboard)
    {
    }

    private void RaiseClipboardTextChanged(string text)
    {
        var handler = ClipboardTextChangedEvent;
        if (handler != null)
            handler(this, new ClipboardTextChangedEventArgs(text));
    }

    private void OnClipboardChanged(object sender, OwnerChangeArgs a)
    {
        var atom = Gdk.Atom.Intern("application/x-copysharp-ignore", false);
        GetClipboard().RequestContents(atom,
                (clipboard1, data) => {
                    if (data.Length == -1) {
                        GetClipboard().RequestText(
                                (clipboard2, text) => RaiseClipboardTextChanged(text));
                    }
                });
    }
}
