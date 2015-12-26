# Build and run application with make and Mono.
#
# make -- build app
# make run -- build and execute app
# make clean -- remove all build files
# make nuget -- fetch dependencies
# make test -- build and run tests
# make vim -- Format errors and warnings for Vim
#
# Default configuration is Debug. For release version use "make CONF=Release".
#
# Example:
#   make CONF=Release nuget tests
PROJ ?= CopySharp
CONF ?= Debug

PROJ_DIR := $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
PKG_DIR ?= $(PROJ_DIR)packages/
MONO_PATH ?= $(PKG_DIR)GtkSharp.3.1.3/lib/net45:$(PKG_DIR)NUnit.3.0.1/lib/net45
MONO_ARGS ?= --config $(PROJ_DIR)CopySharp.exe.config
MONO ?= MONO_PATH=$(MONO_PATH) mono --debug $(MONO_ARGS)
NUNIT_CONSOLE_EXE ?= $(PKG_DIR)NUnit.Runners.2.6.4/tools/nunit-console.exe
NUNIT_CONSOLE ?= MONO_IOMAP=all $(MONO) $(NUNIT_CONSOLE_EXE)

.PHONY: all clean distclean run tests nuget vim

all:
	xbuild /verbosity:quiet /p:Configuration=$(CONF) $(PROJ).csproj

clean:
	rm -rf bin obj

distclean: clean
	rm -rf packages

run: all
	$(MONO) ./bin/$(CONF)/$(PROJ).exe

tests: all
	$(NUNIT_CONSOLE) --config=$(CONF) $(PROJ).csproj

nuget:
	nuget restore -Verbosity detailed
	nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory $(PKG_DIR)

vim:
	$(MAKE) | sed -r 's/\(([0-9]+),([0-9]+)\)/:\1:\2/'; exit "$${PIPESTATUS[0]}"

