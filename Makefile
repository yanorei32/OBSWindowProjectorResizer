CSC		= /cygdrive/c/windows/microsoft.net/framework/v4.0.30319/csc.exe

PROJ_NAME	= SteamVRVRViewResizer
TARGET		= SteamVRVRViewResizer.exe
SRC			= src\\Program.cs \
			  src\\VRViewWindow.cs \
			  src\\Form\\MainForm.cs \
			  src\\Form\\MainForm.Design.cs

DEPS	=

CSC_FLAGS		=	/nologo \
					/utf8output \

DEBUG_FLAGS		= 
RELEASE_FLAGS	= 

$(PROJ_NAME)/$(TARGET): $(SRC)
	-mkdir -p $(PROJ_NAME)
	$(CSC) $(CSC_FLAGS) "/out:$(PROJ_NAME)\\$(TARGET)" $(SRC)

.PHONY: all
all: $(PROJ_NAME)/$(TARGET)


.PHONY: clean
clean:
	rm $(PROJ_NAME)/$(TARGET)


