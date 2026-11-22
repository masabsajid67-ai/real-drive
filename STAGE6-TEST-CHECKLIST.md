# Stage 6 — Test Checklist

Unity 2022.3.20f1, OpenWorld scene, Play mode:
[ ] 1. PLAY -> city din me dikhta hai, suraj aur clouds dikhte hain
[ ] 2. 5 minute ruko ya TIME: NIGHT dabao -> raat ho jati hai, aasmaan gehra, headlights auto on
[ ] 3. TIME: DAY dabao -> din wapas, sunset ke liye DayNightCycle.SetSunset() Editor me test karo
[ ] 4. WEATHER dabao -> Cloudy, phir Rain: baarish ke particles girte hain, roads geeli aur chamakdar
[ ] 5. II / Esc / P dabao -> Pause menu khulta hai, game ruk jata hai, RESUME se wapas chalta hai
[ ] 6. Pause me RESTART POSITION -> car wapas (0,1,0) par, CAMERA -> camera badalta hai, Settings sliders kaam karte hain, MAIN MENU -> menu par jata hai
[ ] 7. Traffic ya building se takrao -> thud awaz aati hai
[ ] 8. Tez speed par HANDBRAKE dabao -> tire screech awaz
[ ] 9. MainMenu par jao -> background music halki bajti hai, OpenWorld me city ambience
[ ] 10. Stage 1-5 ka sab kuch abhi bhi kaam karta hai
[ ] 11. Console me koi red error nahi hai

Limitations: Bloom / Ambient Occlusion ka URP Volume is zip me hand-authored nahi hai, Unity Editor me manually add karna hoga (Render Pipeline > Volume). Rain sirf player ke aas paas girti hai, poore city me nahi.
