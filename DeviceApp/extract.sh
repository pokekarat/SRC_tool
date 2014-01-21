#!/bin/bash

TOOL=apktool.jar

function dissambly {
	echo $1 | tee -a AndroidManifest.log
	echo "--------------------------------------------------------------------------------" | tee -a AndroidManifest.log

	echo "java -jar $TOOL decode $2 $1"
	cat $1/AndroidManifest.xml | egrep 'package=' | awk '{print $5" "$6}' >> AndroidManifest.log
	cat $1/AndroidManifest.xml | egrep 'activity.*android:name' | awk '{print $3" "$4" "$5}' >> AndroidManifest.log
	

	echo "" | tee -a AndroidManifest.log
	echo "" >> AndroidManifest.log
	echo "" >> AndroidManifest.log
}

rm -f AndroidManifest.log
#java -jar apktool.jar decode com.skype.rover.apk skype
#java -jar apktool.jar decode com.viber.voip.apk viber
#java -jar apktool.jar decode candy-crush-saga-mahooq.com.apk candycrush
#java -jar apktool.jar decode LINE-Pokopang-mahooq.com.apk pokopang
#java -jar apktool.jar decode com.gameloft.android.ANMP.GloftDMHM.apk minor

#echo "Skype" >> AndroidManifest.log
#cat skype/AndroidManifiest.xml | egrep 'package' | awk '{print $5}' >> AndroidManifest.log
#cat skype/AndroidManifest.xml | egrep 'activity.*android:name' >> AndroidManifest.log

#echo "viber" >> AndroidManifest.log
#cat viber/AndroidManifest.xml | egrep 'package' | awk '{print $5}' >> AndroidManifest.log
#cat viber/AndroidManifest.xml | egrep 'activity.*android:name' >> AndroidManifest.log

#echo "candycrush" >> AndroidManifest.log
#cat candycrush/AndroidManifest.xml | egrep 'package' | awk '{print $5}' >> AndroidManifest.log
#cat candycrush/AndroidManifest.xml | egrep 'activity.*android:name' >> AndroidManifest.log

#echo "pokopang" >> AndroidManifest.log
#cat pokopang/AndroidManifest.xml | egrep 'package' | awk '{print $5}' >> AndroidManifest.log
#cat pokopang/AndroidManifest.xml | egrep 'activity.*android:name' >> AndroidManifest.log

#echo "minor" >> AndroidManifest.log
#cat minor/AndroidManifest.xml | egrep 'package' | awk '{print $5}' >> AndroidManifest.log
#cat minor/AndroidManifest.xml | egrep 'activity.*android:name' >> AndroidManifest.log

dissambly skype com.skype.rover.apk
dissambly viber icom.viber.voip.apk
dissambly candycrush candy-crush-saga-mahooq.com.apk
dissambly pokopang LINE-Pokopang-mahooq.com.apk
dissambly minor com.gameloft.android.ANMP.GloftDMHM.apk
