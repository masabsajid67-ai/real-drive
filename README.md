# REAL DRIVE — v1.0.0 FINAL: Complete Project (Stage 1-6 + APK Build Guide)

Unity 2022.3.20f1 LTS + URP 14.0.9 + C# + Android (Landscape)

## How to open
1. Install Unity Hub, install Unity 2022.3.20f1 (LTS) with Android Build Support module.
2. Open Unity Hub > Open > select this folder `RealDrive-Unity`.
3. Unity will import. If URP asset shows a warning, go to Edit > Render Pipeline > Universal Render Pipeline > Upgrade Project Materials to URP Materials.
4. File > Build Settings: Scenes in Build must be MainMenu (0), LoadingScene (1), OpenWorld (2), Garage (3). Platform = Android, switch platform.
5. Player Settings: Package Name com.jolet.realdrive, Minimum API 24, Orientation Landscape Left, Scripting Backend IL2CPP, Target Architectures ARM64.
6. Press Play in Editor with MainMenu scene open. PLAY loads LoadingScene then OpenWorld placeholder. GARAGE loads Garage. CARS opens car list panel. SETTINGS opens settings panel. EXIT quits (in Editor it stops Play).

## What works in Stage 1 (tested by code review, NOT by Unity Editor playtest on this VM)
- MainMenu with animated 3D sports car (built from primitives, rotates + camera orbits), REAL DRIVE logo, PLAY / GARAGE / CARS / SETTINGS / EXIT — all buttons wired, no fake buttons.
- LoadingScene with real AsyncOperation progress bar (0-100%), tips, then auto-activates target scene.
- Garage scene: 3D car preview rotates, 6 colours, car info panel, BACK and PLAY work, selection saved in PlayerPrefs.
- CARS panel in MainMenu: 8 fictional cars, select saves, menu car colour/name/speed updates.
- Settings: Sound volume, Music volume, Graphics Low/Medium/High (QualitySettings), saved in PlayerPrefs, applied on start.
- AudioManager singleton (DontDestroyOnLoad), click sound generated in code, no audio files needed.
- SceneLoader singleton (DontDestroyOnLoad), landscape enforced.

## What is NOT in Stage 1 yet
- No real driving / WheelCollider physics (Stage 2) — Stage 2 me add ho gaya hai, neeche dekho.
- No full city (Stage 3), no traffic (Stage 4), no upgrades/rims/spoiler (Stage 5), no weather/day-night (Stage 6).
- OpenWorld is a placeholder: ground + road + 8 buildings + showroom car + Back/Garage buttons.
- No APK in this zip. Build the APK from Unity on a PC (File > Build Settings > Android > Build) or via Unity Cloud Build. This VM has no Unity Editor, so the APK was NOT built here — do not claim otherwise.

## Folder structure
Assets/Scenes/ — MainMenu, LoadingScene, OpenWorld, Garage
Assets/Scripts/Core/ — GameConstants, SaveManager, AudioManager, SceneLoader
Assets/Scripts/Data/ — CarData, CarCatalog (8 cars)
Assets/Scripts/UI/ — MainMenuManager, LoadingScreenManager, SettingsManager, CarSelectionManager, GarageManager
Assets/Scripts/Menu/ — MenuCarBuilder, AnimatedMenuCar
Assets/Scripts/World/ — OpenWorldPlaceholder
Assets/Scripts/Utils/ — RuntimeUIHelper, SafeAreaHelper
Assets/Settings/ — UniversalRenderPipelineAsset

## Known limitations / honest notes
- Scenes were hand-authored as Unity YAML. UI is built at runtime by C# (RuntimeUIHelper) so buttons work even if a scene layout changes. On first open Unity may re-serialize scenes — that is normal.
- URP asset in this zip is a minimal hand-authored asset. If Unity reports a URP error on open, create a new URP asset via Assets > Create > Rendering > URP Asset and assign it in Project Settings > Graphics.
- No external 3D models are included. Cars/city are primitive-based placeholders, intentionally fictional (no Porsche/Supra branding).

## Stage 2 — kya naya hai (Masab ke naye rules ke saath)
- NO MISSION. Sirf free roam open world + driving. Koi mission, koi countdown, koi forced track nahi hai.
- Player car se bahar nikal sakta hai (GET OUT button / E key), paidal chal phir sakta hai, car ke paas jao to ENTER CAR button aata hai, dobara car me baith jao.
- Car physics: WheelCollider, motor, brake, reverse (rukne par brake dabao to reverse), handbrake, steering, gears 1-6 + R, RPM, speed KM/H, nitro boost.
- Mobile HUD: left side steering ◀ ▶, right side ACCEL (press and hold), BRAKE, HANDBRAKE, NITRO, CAM, LIGHT, RESPAWN, PAUSE/MENU. Top par speedometer, gear, RPM.
- Cameras: 4 - Chase, Close Rear, Cockpit, Front. CAM button / C key se badlo.
- Engine sound RPM ke saath badalta hai. Headlights on/off kar sakte ho.
- Garage me ab sirf 5 cars hain (Masab: zyada cars nahi) - Storm GT, Thunder V8, Phantom X, Apex One, Royal S.
- OpenWorld me ab road cross, buildings, petrol station, trees, lamps hain. Stage 1 ka menu, loading, garage, settings sab waise hi kaam karta hai, kuch delete nahi kiya.
- Keyboard test (Editor): W accel, S brake/reverse, A/D steer, Space handbrake, E enter/exit, R respawn, C camera, L lights.

## Stage 3 — kya naya hai
- Bada connected city 420 x 420 m, ek hi scene me, koi alag loading nahi, districts ke beech free drive karo.
- District 1 Downtown NW: 12 skyscrapers, offices, plaza, billboards.
- District 2 Residential NE: 12 ghar, garden, garage, driveway.
- District 3 Commercial SE: 8 shops, restaurant, parking lot, benches.
- District 4 Highway North: elevated highway z=195, pillars, bridge overpass, centre lines.
- District 5 Suburban SW: park, 18 trees, benches, 6 chhote ghar.
- Grid roads har 60 m par, centre yellow lines, sidewalks, street lamps, 4 intersections par traffic lights (abhi green, cycle Stage 4 me), 2 petrol stations, 3 billboards, road signs.
- Minimap: top-left me live top-down camera, player ke saath chalti hai. MAP button / M key se bada World Map khulta hai, districts ke naam likhe hain.
- Respawn: car ulta ho jaye 3 sec tak ya neeche gir jaye to auto respawn (0,1,0) par. RESPAWN button bhi hai.
- Performance: shared materials, primitives, koi external model nahi, mobile ke liye halka rakha gaya hai. LOD / occlusion culling ka proper setup Stage 6 polish me aayega, abhi draw calls kam rakhne ke liye objects ko ek City parent me rakha gaya hai.
- NO MISSION rule waise hi hai - sirf free roam + driving + on-foot.

## Stage 4 — kya naya hai
- Traffic: 14 cars ka object pool, player ke aas paas spawn, 200 m door jaye to despawn karke dobara player ke paas spawn.
- Types: Sedan 12 m/s, SUV 11, Van 9, Sports 15, Truck 8, Bus 9 — alag size, colour aur speed.
- AI lane following: grid roads par lane offset 2.2 m, intersection par 30% chance left/right turn, warna seedha.
- Traffic lights: cycle Green 6s -> Yellow 2s -> Red 6s -> Yellow 2s, NS aur EW opposite. Car intersection se 12 m pehle ho aur red/yellow ho to rukti hai, green par chalti hai. Lights ki visual spheres bhi cycle ke saath badalti hain.
- Collision avoidance: aage raycast 14 m, 6 m se kam ho to full stop, 12 m se kam ho to slow, safe distance rakhti hai. Player aage ho to bhi rukti hai.
- Horn: 3 sec se zyada blocked rahe to horn bajta hai.
- Player traffic se takra sakta hai — traffic cars par BoxCollider + Rigidbody (kinematic) hai.
- Performance: pooling + distance-based despawn, har car har frame raycast nahi karti zyada door, sirf active cars update hoti hain.
- NO MISSION rule waise hi hai.

## Stage 5 — kya naya hai
- Garage ab poora hai: 5 cars hi hain (Masab ka rule), har car ki alag speed/accel/handling/braking/weight hai, info panel me bars ke saath dikhta hai.
- 3D preview: auto-rotate, ROTATE button, mouse/touch drag se ghumao.
- Colour: 6 colours. Paint finish: Glossy / Matte / Metallic. Rims: Standard / Sport / Chrome. Spoiler: None / Small / Large / GT Wing.
- Upgrades free hain (koi mission/paisa nahi): Engine Lv 0-3 (+8% speed har level), Brakes Lv 0-3 (+15% braking), Handling Lv 0-3 (+7% steering). Sab PlayerPrefs me save hota hai.
- Selection + customization OpenWorld se connected hai: jo car garage me select/customize karo, wahi car OpenWorld me chalti hai, usi colour/finish/spoiler/rims aur upgrades ke saath.
- Fictional cars hi hain, koi branded copy nahi hai.

## Stage 6 — kya naya hai
- Day-Night cycle: suraj ghoomta hai, din 10 AM se shuru, auto cycle 300 sec me poora din, sunset orange, raat ko aasmaan gehra, fog aur ambient kam, raat ko headlights auto on.
- Weather: Sunny / Cloudy / Rain — WEATHER button se badlo. Rain me particles girte hain, fog badhta hai, suraj kam, roads geeli aur chamakdar ho jati hain. Cloudy me halka fog.
- Clouds: aasmaan me 6 safed clouds.
- Pause menu: II button / Esc / P se khulta hai — Resume, Restart Position, Camera Change, Settings (Sound/Music sliders), Main Menu. Pause par Time.timeScale 0, car ruk jati hai.
- Audio: engine (pehle se), horn (pehle se), collision thud (takkar par awaz, speed ke hisab se), tire screech (handbrake + tez speed), city ambience loop, main menu me background music loop.
- Graphics polish: Directional Light soft shadows on, fog, sky colour, car materials glossy/metallic, glass, road wet look. Bloom / AO ka proper URP Volume is project me hand-authored nahi hai — Unity me Volume + Bloom add karna Stage 6 ke baad ka optional step hai, README me note hai.
- Saving: pehle se hi — car, upgrades, customization, sound, graphics sab PlayerPrefs me save hote hain.
- Gameplay: NO MISSION rule waise hi — free roam, koi countdown, koi forced track nahi.

## Final Stage — APK Build
- APK is zip me NAHI hai. Wajah: is system me Unity Editor / Android SDK / JDK / keystore nahi hai.
- APK banane ke liye BUILD-APK-GUIDE.md padho (PC tareeqa recommended, Cloud Build tareeqa bhi likha hai).
- Editor script: Unity me REAL DRIVE > Build Android APK dabao.
- Final review: FINAL-REVIEW.md padho — kya fix hua, kya baaki hai, aur 16 tests ki list.
- Version 1.0.0, bundle com.jolet.realdrive, min SDK 24, landscape, IL2CPP, ARM64.
