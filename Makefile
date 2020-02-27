CSC		= /cygdrive/c/windows/microsoft.net/framework/v4.0.30319/csc.exe

PROJ_NAME	= SteamVRVRViewResizer
TARGET		= $(PROJ_NAME).exe
RELEASE_DIR	= $(PROJ_NAME)
REPO		= https://github.com/Yanorei32/SteamVRVRViewResizer

SRCS		= src\\Program.cs \
			  src\\VRViewWindow.cs \
			  src\\Util.cs \
			  src\\LaunchFromShortcut.cs \
			  src\\Form\\MainForm.cs \
			  src\\Form\\MainForm.Design.cs

DEPS	=

CSC_FLAGS		=	/nologo \
					/target:winexe \
					/utf8output \
					/win32icon:res\\icon.ico \
					/resource:res\\icon.ico,icon \
					/resource:res\\logo.png,logo

.PHONY: all
all: $(RELEASE_DIR)/$(TARGET) \
	$(RELEASE_DIR)/LICENSE.txt \
	$(RELEASE_DIR)/README.url

$(RELEASE_DIR)/$(TARGET): $(SRCS)
	-mkdir -p $(RELEASE_DIR)
	$(CSC) $(CSC_FLAGS) "/out:$(RELEASE_DIR)\\$(TARGET)" $(SRCS)

$(RELEASE_DIR)/LICENSE.txt: LICENSE
	-mkdir -p $(RELEASE_DIR)
	cp \
		LICENSE \
		$(RELEASE_DIR)/LICENSE.txt

$(RELEASE_DIR)/README.url:
	-mkdir -p $(RELEASE_DIR)
	echo -ne \
		"[InternetShortcut]\r\nURL=$(REPO)/blob/master/README.md" \
		> "$(RELEASE_DIR)/README.url"

.PHONY: genzip
genzip: $(PROJ_NAME).zip

$(PROJ_NAME).zip: all
	rm -f $(PROJ_NAME)/*.lnk
	zip -r $(PROJ_NAME).zip $(RELEASE_DIR)

.PHONY: clean
clean:
	rm -f $(PROJ_NAME).zip
	rm -rf $(RELEASE_DIR)


