# REAL DRIVE — Cloud Build Guide (Phone se, Roman Urdu me)

PC nahi hai to APK is tarah banega. Koi step skip mat karna, order wahi rakho jo neeche likha hai.

## Pehle ye 2 accounts banao (phone ke browser se, free hain)
1. Unity ID banao:
   - Chrome kholo, unity.com par jao, Create a Unity ID dabao.
   - Email, password aur naam dalo, email verify karo.
   - Yehi ID Cloud Build me kaam ayega.
2. GitHub account banao:
   - github.com par jao, Sign up karo, email verify karo.
   - GitHub mobile app bhi install kar lo, baad me kaam ayega.

## Step 1 — GitHub repo banao
1. GitHub app ya browser me New repository dabao.
2. Name: `real-drive` rakho.
3. Public rakho (Private bhi chalega, lekin Cloud Build ke liye Public asaan hai).
4. Add a README file ko tick mat karo, khaali repo banao.
5. Create repository dabao.

## Step 2 — Project ko GitHub par upload karo
Tareeqa A (phone browser se, asaan):
1. Drive link se `REAL-DRIVE-CloudBuild-Ready-v1.0.0.zip` download karo.
2. Phone me Files app se ZIP ko extract karo — folder `RealDrive-Unity` milega.
3. GitHub repo kholo, Add file > Upload files dabao.
4. `RealDrive-Unity` folder ke andar wali sab files select karo — Assets, ProjectSettings, Packages, ProjectVersion.txt, .github, BUILD-APK-GUIDE.md, sab.
5. Important: Library, Temp, Builds, Logs folders upload mat karo. Ye bade hote hain aur zaroori nahi hain. `.gitignore` pehle se project me hai, wohi inko rokega agar tum git use karo.
6. Commit changes dabao. Upload me 5-10 minute lag sakte hain, WiFi par karna.

Tareeqa B (agar upload fail ho):
- ZIP ko seedha GitHub par upload mat karo, GitHub ko project ki files chahiye hoti hain, ZIP file se Cloud Build nahi chalega.

## Step 3 — Unity Cloud Build connect karo (Tareeqa 1, recommended)
1. Chrome me dashboard.unity3d.com kholo, Unity ID se login karo.
2. Cloud Build dabao, phir New Project / Create.
3. Source: GitHub select karo, GitHub account connect karo, repo `real-drive` select karo, branch `main` rakho.
4. Config me ye dalo (poori list UNITY-CLOUD-BUILD-CONFIG.md me bhi hai):
   - Unity Version: 2022.3.20f1
   - Platform: Android
   - Build Target: Android
5. Android Keystore:
   - Settings > Android > Keystore > Create New Keystore dabao.
   - Alias: `realdrive`, password apni marzi ka rakho, aur usko kisi safe jagah likh lo. Ye password dobara chahiye hoga.
6. Build Now dabao.
7. Build chalte waqt 15-30 minute lag sakte hain. Pehli build sab se lambi hoti hai.
8. Build Succeeded dikhe to Download APK dabao. APK phone me download ho jayega.
9. APK install karo: Files me APK kholo, Install Unknown Apps allow karo, install karo.

## Step 3B — GitHub Actions se build (Tareeqa 2, backup)
Agar Cloud Build kaam na kare to ye tareeqa hai. Isme Unity License secret chahiye hota hai:
1. GitHub repo > Actions tab kholo.
2. Pehli baar workflow `Build REAL DRIVE APK (Android)` dikhega, Run workflow dabao.
3. Agar license error aaye to ye karna hoga:
   - Repo > Settings > Secrets and variables > Actions me jao.
   - `UNITY_EMAIL` aur `UNITY_PASSWORD` apni Unity ID ke dalo.
   - `UNITY_LICENSE` ke liye: game-ci ka activation file step alag se chalana padta hai, uski detail game-ci ki docs me hai. Ye step phone se mushkil hai.
4. Keystore secrets bhi wahi dalne hain: ANDROID_KEYSTORE_BASE64, ANDROID_KEYSTORE_PASS, ANDROID_KEYALIAS_NAME, ANDROID_KEYALIAS_PASS.
5. Is liye pehle Tareeqa 1 try karo, Tareeqa 2 backup hai.

## Agar build fail ho to
- Pehli red error padho, aksar ye hoti hai:
  - Unity Version galat hai — 2022.3.20f1 hi rakho.
  - Scene missing hai — Build Settings me 4 scenes honi chahiye, order: MainMenu, LoadingScene, OpenWorld, Garage.
  - Keystore missing hai — keystore banao aur dobara build karo.
  - URP material pink hai — ye build fail nahi karta, game khulne par pink dikhe to Editor me material upgrade karna padta hai, Cloud Build se theek nahi hota.
- Build fail ho to mujhe us error ka screenshot / text bhej dena, main bata dunga kya fix karna hai.

## Play Store par upload (APK milne ke baad)
1. Play Console account banao: play.google.com/console, $25 one-time fee lagti hai, Google account se payment hoti hai.
2. App banao: REAL DRIVE, package `com.jolet.realdrive`.
3. Play Store ko APK nahi, AAB chahiye hota hai. Cloud Build / GitHub Actions me `androidAppBundle: true` karne se AAB banta hai. Pehle APK se phone par test karo, phir AAB banao.
4. Store listing, screenshots, description aur privacy policy chahiye hogi.

## Tumhe abhi kya karna hai
1. Unity ID aur GitHub account banao.
2. Repo banao aur project upload karo.
3. Mujhe repo ka link bhej do, main config check kar dunga.
4. Cloud Build me Build Now dabao, aur result mujhe batao.
