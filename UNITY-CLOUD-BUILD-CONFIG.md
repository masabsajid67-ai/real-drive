# Unity Cloud Build — Config (dashboard me ye values dalo)

Project: REAL DRIVE
Repo: GitHub repo jo tum banao ge (neeche guide dekho)
Branch: main
Unity Version: 2022.3.20f1
Platform / Target: Android
Build Target: Android
Scripting Backend: IL2CPP
Target Architectures: ARM64
Build Method: (Cloud Build khud scenes se build karta hai, CIBuild ki zaroorat nahi)
Scenes in Build (order):
  1. Assets/Scenes/MainMenu.unity
  2. Assets/Scenes/LoadingScene.unity
  3. Assets/Scenes/OpenWorld.unity
  4. Assets/Scenes/Garage.unity

Keystore:
- Cloud Build > Settings > Android > Keystore > Create New Keystore
- Alias: realdrive, Password: jo tum rakhoge, usko safe likh lo
- Yehi keystore har update ke liye chahiye hoga, isko mat khona

Environment / Secrets (sirf GitHub Actions wale tareeqe ke liye):
- UNITY_LICENSE, UNITY_EMAIL, UNITY_PASSWORD
- ANDROID_KEYSTORE_BASE64, ANDROID_KEYSTORE_PASS, ANDROID_KEYALIAS_NAME, ANDROID_KEYALIAS_PASS
- Ye secrets GitHub repo > Settings > Secrets and variables > Actions me dalne hain
- Unity License kaise milega: game-ci/unity-request-activation-file se activation file banao, phir Unity dashboard se .ulf license file lo, usko secret me dalo. Detail CLOUD-BUILD-GUIDE-URDU.md me hai.
