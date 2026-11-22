# Stage 1 — Test Checklist (for Unity Editor / Android)

Run these in Unity 2022.3.20f1, MainMenu scene, Play mode:
[ ] 1. MainMenu loads, REAL DRIVE logo visible, car rotates, camera slowly orbits
[ ] 2. PLAY -> LoadingScene shows, progress 0% -> 100%, then OpenWorld placeholder loads
[ ] 3. OpenWorld placeholder: ground, road, buildings, car visible, PAUSE/MENU returns to MainMenu
[ ] 4. GARAGE -> Garage loads, car rotates, colour buttons change car colour, BACK returns
[ ] 5. CARS -> panel opens, select another car, menu car name/speed/colour updates
[ ] 6. SETTINGS -> Sound/Music sliders move, Quality LOW/MEDIUM/HIGH changes, Close works, reopen keeps values
[ ] 7. EXIT -> in build quits app; in Editor stops Play mode
[ ] 8. Rotate phone / resize Game view to landscape 1920x1080 — UI stays inside screen
[ ] 9. Stop Play, Play again — selected car + settings are remembered (PlayerPrefs)
[ ] 10. Console: no red errors. If a URP/material warning appears, upgrade materials to URP.

Remaining limitations after Stage 1:
- No driving, no traffic, no full city, no upgrades, no weather, no APK yet.
