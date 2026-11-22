# REAL DRIVE — APK Build Guide (Final Stage, v1.0.0)

## Seedhi baat pehle
Is ZIP me APK NAHI hai. Yahan is system me Unity Editor, Android SDK, JDK aur keystore nahi hai, is liye APK yahan se build nahi ho saka. APK tumhe PC par Unity se, ya Unity Cloud Build se banana hoga. Neeche dono tareeqe step by step hain.

## Tareeqa 1 — PC par Unity se (sab se reliable, recommended)
1. PC / laptop par Unity Hub install karo.
2. Unity 2022.3.20f1 (LTS) install karo, saath ye modules tick karo: Android Build Support, Android SDK & NDK Tools, OpenJDK.
3. Unity Hub > Open > ye folder `RealDrive-Unity` select karo. Pehli baar import me 5-15 minute lag sakte hain.
4. Agar URP ka warning aaye: Project Settings > Graphics me dekho. Agar Render Pipeline khaali / missing dikhe to Assets > Create > Rendering > URP Asset banao, phir usko Graphics me assign karo. Ye ek dafa ka kaam hai, is zip ke URP asset ka script GUID hand-authored hai, is liye Unity usko dobara banana chahega.
5. File > Build Settings: Platform = Android, Switch Platform dabao. Scenes in Build me ye 4 honi chahiye, isi order me: MainMenu (0), LoadingScene (1), OpenWorld (2), Garage (3). Agar koi missing ho to Add Open Scenes se add karo.
6. Player Settings: Package Name `com.jolet.realdrive`, Version 1.0.0, Bundle Version Code 1, Minimum API Level 24, Target API Automatic, Orientation Landscape Left, Scripting Backend IL2CPP, Target Architectures ARM64, Graphics API OpenGLES3.
7. Keystore: Player Settings > Publishing Settings > Keystore Manager > Create New. Keystore ka password aur alias ka password kahin safe likh lo, ye dobara chahiye hoga. Bina keystore ke APK install nahi hoga Play Store par, aur update ke liye wahi keystore chahiye hoga.
8. Build: Menu me REAL DRIVE > Build Android APK dabao (ye script is project me hai: Assets/Editor/AndroidBuild.cs), ya File > Build Settings > Build. Pehli build me 10-30 minute lag sakte hain.
9. APK milega: `Builds/Android/REAL-DRIVE-v1.0.0.apk`. Phone me copy karo, Unknown Sources / Install Unknown Apps allow karo, install karo, landscape me khelo.
10. Agar build fail ho: Console me pehli red error padho, aksar SDK / JDK path ya URP material ka masla hota hai. Edit > Preferences > External Tools me SDK/JDK ka path check karo.

## Tareeqa 2 — Unity Cloud Build (bina PC ke, phone se setup mushkil)
1. Unity ID banao (unity.com), GitHub account banao.
2. Is project ko GitHub repo me upload karo (phone me GitHub app se, ya PC se ek dafa).
3. Unity Dashboard > Cloud Build > New Project > GitHub repo connect karo, Branch select karo.
4. Target: Android, Unity Version 2022.3.20f1, Build Target Android.
5. Keystore Cloud Build me upload karo (Tareeqa 1 step 7 wala).
6. Build Now dabao. Build Cloud par chalega, APK download link milega.
7. Free tier me build time aur builds ki limit hoti hai, aur setup phone se karna mushkil hai. Is liye Tareeqa 1 behtar hai.

## Graphics settings — Low / Medium / High
QualitySettings me 3 levels hain: Low (0), Medium (1), High (2). Settings menu me jo quality chuno, wahi apply hoti hai aur save rehti hai. Entry-level phone par Low, average par Medium, powerful par High rakho.

## Final test — 16 tests (spec ke mutabiq)
Ye sab Unity Editor me Play karke, phir APK install karke phone par check karo:
1. Main menu navigation — sab buttons kaam karte hain
2. PLAY button — Loading dikhta hai, phir OpenWorld khulta hai
3. Scene loading — progress 0-100%, koi atakna nahi
4. Forward driving — ACCEL dabake rakho, car aage badhti hai
5. Reverse driving — ruki car me BRAKE dabake rakho, reverse chalti hai
6. Left / right steering — dono taraf mudti hai
7. Braking — BRAKE se rukti hai
8. Handbrake — peeche wale wheels lock, slide
9. Camera switching — CAM se 4 cameras badalte hain
10. Traffic AI — traffic chalti hai, red par rukti hai, takrati nahi
11. Garage — khulta hai, car ghoomti hai
12. Vehicle selection — 5 cars me se select karo, menu aur OpenWorld me wahi car
13. Customization — colour, finish, rims, spoiler, upgrades badalte hain aur save rehte hain
14. Save / load — game band karke dobara kholo, sab yaad rehta hai
15. Pause / resume — II se pause, RESUME se wapas, game rukta hai
16. Performance / memory — Low par bhi frame rate stable, koi crash nahi, phone garam nahi hota zyada

## Agar kuch kaam na kare to
- Console me red error dekho, pehli error ko fix karo, baaki aksar usi se theek ho jati hain.
- Scene me Manager GameObject missing ho to scene dobara kholo, Manager ka script GUID check karo (FINAL-REVIEW.md dekho).
- URP pink materials dikhen to Edit > Render Pipeline > Upgrade Project Materials to URP Materials karo.
