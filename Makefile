# Build and run application with make and Mono.
#
# make -- build app
# make run -- build and execute app
# make clean -- remove all build files
# make nuget -- fetch dependencies
# make test -- build and run tests
# make vim -- format errors and warnings for Vim
# make monodevelop -- run monodevelop for project
#
# Default configuration is Debug. For release version use "make CONF=Release".
#
# Example:
#   make CONF=Release nuget tests
PROJ ?= PasteSharp
CONF ?= Debug

PROJ_DIR := $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
PKG_DIR ?= $(PROJ_DIR)packages/
MONO_ARGS ?=
MONO ?= mono --debug $(MONO_ARGS)
NUNIT_CONSOLE_EXE ?= $(PKG_DIR)NUnit.Runners.2.6.4/tools/nunit-console.exe
NUNIT_CONSOLE ?= MONO_IOMAP=all $(MONO) $(NUNIT_CONSOLE_EXE)
BIN_PATH ?= ./bin/$(CONF)

.PHONY: all clean distclean run tests nuget vim monodevelop

all:
	xbuild /verbosity:quiet /p:Configuration=$(CONF) $(PROJ).csproj

clean:
	rm -rf bin obj

distclean: clean
	rm -rf packages

run: all
	$(MONO) ./$(BIN_PATH)/$(PROJ).exe

tests: all
	$(NUNIT_CONSOLE) --config=$(CONF) $(PROJ).csproj

nuget:
	nuget restore -Verbosity detailed
	nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory $(PKG_DIR)

vim:
	$(MAKE) | sed -r 's/\(([0-9]+),([0-9]+)\)/:\1:\2/'; exit "$${PIPESTATUS[0]}"

monodevelop:
	monodevelop $(PROJ).csproj

