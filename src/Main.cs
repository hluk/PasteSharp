/*
    Copyright (c) 2015, Lukas Holecek <hluk@email.cz>

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
using System.Reflection;

using Gtk;

namespace PasteSharp
{
    class MainClass
    {
        private static void PrintHelp()
        {
            var exe = System.AppDomain.CurrentDomain.FriendlyName;
            Console.WriteLine("PasteSharp is Small cross-platform clipboard manager");
            Console.WriteLine("Usage: " + exe + " [ARGUMENTS]");
            Console.WriteLine("ARGUMENTS:");
            Console.WriteLine("  -h, --help     Print help.");
            Console.WriteLine("  -v, --version  Print version.");
        }

        private static void PrintVersion()
        {
            var name = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine(name.Name + " " + name.Version);
        }

        private static int ParseArguments(string[] args)
        {
            if (args.Length == 1) {
                switch (args[0]) {
                    case "-h":
                    case "--help":
                        PrintHelp();
                        return 0;

                    case "-v":
                    case "--version":
                        PrintVersion();
                        return 0;

                    default:
                        break;
                }
            }

            Console.WriteLine("Error: Unknown command line argument (use --help).");
            return 1;
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
