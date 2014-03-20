CC=arm-linux-androideabi-gcc
TOOL=a.out
TESTCASES=10
DELAY=1
TESTPACKAGE="com.snctln.game.BreakTheBlocks"
PACKAGE="com.google.process.gapps"
WLAN="/sys/class/net/wlan0"

.PHONY: pull cat install env

all:
	$(CC) sample.c
	adb push $(TOOL) /data/local/tmp

test:
	adb shell ./data/local/tmp/$(TOOL) $(DELAY) $(TESTCASES) $(PACKAGE) $(WLAN)

install:
	adb push $(TOOL) /data/local/tmp
	adb push busybox /data/local/tmp
	adb shell chmod 777 /data/local/tmp/busybox
	adb install com.skype.rover.apk
	adb install candy-crush-saga-mahooq.com.apk
	adb install com.viber.voip.apk
	adb install LINE_4.0.1.apk
	adb install LINE-Pokopang-mahooq.com.apk
	adb install com.gameloft.android.ANMP.GloftDMHM.apk
	
memdebug:
	$(CC) memory.test.c memory.c -Wall
	adb push $(TOOL) /data/local/tmp
	adb shell ./data/local/tmp/$(TOOL) $(DELAY) $(TESTCASES) $(PACKAGE) $(WLAN)

tool:
	~/Downloads/android-ndk-r9b/build/tools/make-standalone-toolchain.sh --platform=android-5 --install-dir=/tmp/my-android-toolchain --ndk-dir=/home/rain/Downloads/android-ndk-r9b/

env:
#export PATH=/tmp/my-android-toolchain/bin:$PATH
