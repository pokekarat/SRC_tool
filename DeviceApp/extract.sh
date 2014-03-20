#!/bin/bash

TOOL=apktool.jar

function dissambly {
	echo $1 | tee -a AndroidManifest.log
	echo "--------------------------------------------------------------------------------" | tee -a AndroidManifest.log

#	java -jar $TOOL decode $2 $1
	cat $1/AndroidManifest.xml | egrep 'package=' | awk '{print $5" "$6}' >> AndroidManifest.log
	cat $1/AndroidManifest.xml | grep 'activity' | sed -n 's/.*\(android:targetActivity[^ ]*\).*/\1/p' >> AndroidManifest.log
	cat $1/AndroidManifest.xml | grep 'activity' | sed -n 's/.*\(android:name[^ ]*\).*/\1/p' >> AndroidManifest.log

	echo "" | tee -a AndroidManifest.log
	echo "" >> AndroidManifest.log
	echo "" >> AndroidManifest.log
}

rm -f AndroidManifest.log

dissambly skype com.skype.rover.apk
dissambly viber com.viber.voip.apk
dissambly candycrush candy-crush-saga-mahooq.com.apk
dissambly pokopang LINE-Pokopang-mahooq.com.apk
dissambly minor com.gameloft.android.ANMP.GloftDMHM.apk
