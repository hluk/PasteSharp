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

using System;
using System.IO;
using System.Reflection;

using Gtk;
using Mono.Options;

namespace PasteSharp
{
    class MainClass
    {
        private static void PrintVersion()
        {
            var name = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine(name.Name + " " + name.Version);
        }

        private static void PrintClipboard(string mime)
        {
            Application.Init();
            var data = ClipboardManager.GetClipboardData(mime);
            Stream stdout = Console.OpenStandardOutput();
            stdout.Write(data, 0, data.Length);
        }

        private static int ParseArguments(string[] args)
        {
            bool printHelp = false;
            bool printVersion = false;
            string clipboardData = null;

            var exe = System.AppDomain.CurrentDomain.FriendlyName;
            var p = new OptionSet() {
                "PasteSharp is simple cross-platform clipboard manager.",
                "Usage: " + exe + " [OPTIONS]",
                "OPTIONS:",
                { "h|help", "Print help.", v => printHelp = v != null },
                { "v|version", "Print version.", v => printVersion = v != null },
                { "c|clipboard:", "Print clipboard text (or data in given format).",
                    v => clipboardData = v != null ? v : "UTF8_STRING" },
            };

            string error = "";
            try {
                var extraArgs = p.Parse(args);
                if (extraArgs.Count != 0)
                    error = "Unknown arguments: " + string.Join(",", extraArgs);
            } catch (OptionException e) {
                error = e.Message;
            }

            if (!string.IsNullOrEmpty(error)) {
                Console.WriteLine("Error: Failed to parse command line arguments: " + error);
                Console.WriteLine("Note: Use --help arguments to pring help.");
                return 1;
            }

            if (printHelp)
                p.WriteOptionDescriptions(Console.Out);
            else if (printVersion)
                PrintVersion();
            else if (clipboardData != null)
                PrintClipboard(clipboardData);

            return 0;
        }

        private static void CreateMainWindow(ClipboardManager clipboardManager)
        {
            var win = new MainWindow();
            win.Show();

            // Add new clipboard content to list.
            clipboardManager.ClipboardTextChangedEvent +=
                (o, a) => win.AddTextItem(a.Text);

            // Push activated item to clipboard.
            win.ItemsActivatedEvent +=
                (o, a) => clipboardManager.Text = a.ItemText;

            // Minimize main window after an item is activated.
            win.ItemsActivatedEvent +=
                (o, a) => win.Iconify();
        }

        private static void RunApp()
        {
            Application.Init();
            var clipboardManager = new ClipboardManager();
            CreateMainWindow(clipboardManager);
            Application.Run();
        }

        public static int Main(string[] args)
        {
            if (args.Length > 0)
                return ParseArguments(args);

            RunApp();
            return 0;
        }
    }
}
