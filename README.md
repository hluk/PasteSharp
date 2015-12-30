[![Build Status](https://travis-ci.org/hluk/PasteSharp.svg?branch=master)](https://travis-ci.org/hluk/PasteSharp)
[![Windows Build Status](https://ci.appveyor.com/api/projects/status/github/hluk/pastesharp?branch=master&svg=true)](https://ci.appveyor.com/project/hluk/pastesharp)

PasteSharp is simple cross-platform clipboard manager.

NuGet package is [here](https://www.nuget.org/packages/PasteSharp).

Application is written in C# with GUI toolkit Gtk#3.

Application is licensed under GPLv3.

To build and run the app execute following commands.

    nuget restore
    xbuild /p:Configuration=Release CopySharp.csproj
    mono bin/Release/CopySharp.exe

