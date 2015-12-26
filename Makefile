# Build and run application with make and Mono.
#
# make -- build app
# make run -- build and execute app
# make clean -- remove all build files
#
# Default configuration is Debug. For release version use "make CONF=Release".
PROJ ?= CopySharp
CONF ?= Debug

.PHONY: all clean run

all:
	xbuild /verbosity:quiet /p:Configuration=$(CONF) $(PROJ).csproj \
		| sed -r 's/\(([0-9]+),([0-9]+)\)/:\1:\2/'; \
		exit "$${PIPESTATUS[0]}"

clean:
	rm -rf bin obj

run: all
	mono --debug ./obj/x86/$(CONF)/$(PROJ).exe

test tests: all
	MONO_IOMAP=all nunit-console --config=$(CONF) $(PROJ).csproj

