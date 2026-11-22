using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Data;
using RealDrive.Menu;
using RealDrive.Garage;
using RealDrive.Utils;
namespace RealDrive.UI
{
    public class GarageManager : MonoBehaviour
    {
        GameObject carObject; Text infoText; Text upgradeText; Canvas canvas;
        bool autoRotate = true;
        float manualAngle;
        void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            if (FindObjectOfType<SceneLoader>() == null) { GameObject sl = new GameObject("SceneLoader"); sl.AddComponent<SceneLoader>(); }
            if (FindObjectOfType<AudioManager>() == null) { GameObject am = new GameObject("AudioManager"); am.AddComponent<AudioManager>(); }
            Camera cam = Camera.main ?? FindObjectOfType<Camera>();
            if (cam != null) { cam.transform.position = new Vector3(0, 3f, 6.5f); cam.transform.LookAt(new Vector3(0, 0.8f, 0)); cam.backgroundColor = new Color(0.04f, 0.05f, 0.08f); }
            MenuCarBuilder.BuildShowroomFloor(new Vector3(0, 0, 0));
            RefreshCar();
            GameObject anim = new GameObject("GarageCarAnimator");
            var a = anim.AddComponent<AnimatedMenuCar>(); a.carTransform = carObject.transform; if (cam != null) a.cameraTransform = cam.transform;
            BuildUI();
        }
        void RefreshCar()
        {
            if (carObject != null) Destroy(carObject);
            CarData car = CarCatalog.Selected();
            Color col = ColorUtility.TryParseHtmlString(SaveManager.CarColorHex, out Color c) ? c : car.bodyColor;
            carObject = MenuCarBuilder.BuildSportsCarCustom(new Vector3(0, 0.1f, 0), col);
            carObject.transform.rotation = Quaternion.Euler(0, manualAngle, 0);
            var anim = FindObjectOfType<AnimatedMenuCar>(); if (anim != null) anim.carTransform = carObject.transform;
            if (infoText != null) UpdateInfo();
        }
        void UpdateInfo()
        {
            CarData car = CarCatalog.Selected();
            int top = Mathf.RoundToInt(car.topSpeedKmh * Customization.EngineMultiplier(SaveManager.EngineLevel));
            infoText.text = car.displayName + "  •  " + car.category + "  •  " + car.weightKg + " KG\n" +
                "Top Speed: " + top + " KM/H  •  Acc: " + car.acceleration.ToString("F1") + "  •  Handling: " + car.handling.ToString("F1") + "  •  Braking: " + car.braking.ToString("F1") + "\n" +
                "Bars: Speed " + Bar(car.topSpeedKmh / 380f) + "  Acc " + Bar(car.acceleration / 10f) + "  Han " + Bar(car.handling / 10f) + "  Brk " + Bar(car.braking / 10f);
            if (upgradeText != null) upgradeText.text = "Engine Lv " + SaveManager.EngineLevel + "/3  •  Brakes Lv " + SaveManager.BrakeLevel + "/3  •  Handling Lv " + SaveManager.HandlingLevel + "/3\nRims: " + Customization.RimNames[SaveManager.RimStyle] + "  •  Spoiler: " + Customization.SpoilerNames[SaveManager.SpoilerStyle] + "  •  Finish: " + Customization.FinishNames[SaveManager.PaintFinish];
        }
        string Bar(float f) { int n = Mathf.RoundToInt(Mathf.Clamp01(f) * 10f); return new string('|', n) + new string('.', 10 - n); }
        void BuildUI()
        {
            canvas = RuntimeUIHelper.CreateCanvas("GarageCanvas", 10);
            Transform root = canvas.transform;
            Text title = RuntimeUIHelper.CreateText(root, "GARAGE  •  5 CARS ONLY", 44, new Color(1f, 0.8f, 0f));
            RuntimeUIHelper.SetRect(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -55), new Vector2(900, 60));
            infoText = RuntimeUIHelper.CreateText(root, "", 20, Color.white);
            RuntimeUIHelper.SetRect(infoText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -140), new Vector2(1100, 80));
            upgradeText = RuntimeUIHelper.CreateText(root, "", 18, new Color(0.6f, 1f, 0.4f));
            RuntimeUIHelper.SetRect(upgradeText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -200), new Vector2(1100, 50));
            // Prev / Next car
            RuntimeUIHelper.CreateButton(root, "◀ PREV", new Vector2(-750, 120), new Vector2(200, 60), new Color(0.15f, 0.17f, 0.25f), () => { var l = CarCatalog.All(); SaveManager.SelectedCarIndex = (SaveManager.SelectedCarIndex - 1 + l.Count) % l.Count; RefreshCar(); });
            RuntimeUIHelper.CreateButton(root, "NEXT ▶", new Vector2(750, 120), new Vector2(200, 60), new Color(0.15f, 0.17f, 0.25f), () => { var l = CarCatalog.All(); SaveManager.SelectedCarIndex = (SaveManager.SelectedCarIndex + 1) % l.Count; RefreshCar(); });
            // Rotate
            RuntimeUIHelper.CreateButton(root, "⟲ ROTATE", new Vector2(-750, 40), new Vector2(200, 55), new Color(0.15f, 0.17f, 0.25f), () => { autoRotate = false; var an2 = FindObjectOfType<AnimatedMenuCar>(); if (an2 != null) an2.enabled = false; manualAngle += 45f; if (carObject != null) carObject.transform.rotation = Quaternion.Euler(0, manualAngle, 0); });
            RuntimeUIHelper.CreateButton(root, autoRotate ? "AUTO: ON" : "AUTO: OFF", new Vector2(750, 40), new Vector2(200, 55), new Color(0.15f, 0.17f, 0.25f), () => { autoRotate = !autoRotate; var anim = FindObjectOfType<AnimatedMenuCar>(); if (anim != null) anim.enabled = autoRotate; });
            // Colours row
            string[] cols = { "#7CFC00", "#FF4500", "#FF0000", "#1E90FF", "#FFD700", "#FFFFFF" };
            string[] names = { "GREEN", "ORANGE", "RED", "BLUE", "GOLD", "WHITE" };
            for (int i = 0; i < cols.Length; i++)
            {
                int idx = i;
                ColorUtility.TryParseHtmlString(cols[i], out Color cc);
                RuntimeUIHelper.CreateButton(root, names[i], new Vector2(-420 + i * 168, -120), new Vector2(150, 50), new Color(cc.r, cc.g, cc.b, 1f), () => { SaveManager.CarColorHex = cols[idx]; RefreshCar(); });
            }
            // Finish / Rims / Spoiler rows
            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                RuntimeUIHelper.CreateButton(root, Customization.FinishNames[i], new Vector2(-300 + i * 180, -185), new Vector2(160, 50), SaveManager.PaintFinish == i ? new Color(0.95f, 0.6f, 0f) : new Color(0.2f, 0.22f, 0.3f), () => { SaveManager.PaintFinish = idx; RefreshCar(); });
                RuntimeUIHelper.CreateButton(root, Customization.RimNames[i], new Vector2(-300 + i * 180, -250), new Vector2(160, 50), SaveManager.RimStyle == i ? new Color(0.95f, 0.6f, 0f) : new Color(0.2f, 0.22f, 0.3f), () => { SaveManager.RimStyle = idx; RefreshCar(); });
            }
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                RuntimeUIHelper.CreateButton(root, Customization.SpoilerNames[i], new Vector2(-270 + i * 180, -315), new Vector2(160, 50), SaveManager.SpoilerStyle == i ? new Color(0.95f, 0.6f, 0f) : new Color(0.2f, 0.22f, 0.3f), () => { SaveManager.SpoilerStyle = idx; RefreshCar(); });
            }
            // Upgrades - free, no mission / money
            RuntimeUIHelper.CreateButton(root, "ENGINE +  (Lv " + SaveManager.EngineLevel + "/3)", new Vector2(-300, -380), new Vector2(260, 55), new Color(0.95f, 0.6f, 0f), () => { if (SaveManager.EngineLevel < 3) SaveManager.EngineLevel++; RefreshCar(); });
            RuntimeUIHelper.CreateButton(root, "BRAKES +  (Lv " + SaveManager.BrakeLevel + "/3)", new Vector2(0, -380), new Vector2(260, 55), new Color(0.95f, 0.6f, 0f), () => { if (SaveManager.BrakeLevel < 3) SaveManager.BrakeLevel++; RefreshCar(); });
            RuntimeUIHelper.CreateButton(root, "HANDLING +  (Lv " + SaveManager.HandlingLevel + "/3)", new Vector2(300, -380), new Vector2(260, 55), new Color(0.95f, 0.6f, 0f), () => { if (SaveManager.HandlingLevel < 3) SaveManager.HandlingLevel++; RefreshCar(); });
            Text note = RuntimeUIHelper.CreateText(root, "Upgrades free hain, koi mission ya paisa nahi hai. Sab kuch PlayerPrefs me save hota hai aur OpenWorld wali car par lagta hai.", 16, new Color(1, 1, 1, 0.65f));
            RuntimeUIHelper.SetRect(note.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 110), new Vector2(1100, 30));
            RuntimeUIHelper.CreateButton(root, "◀  BACK TO MENU", new Vector2(-700, 220), new Vector2(300, 60), new Color(0.15f, 0.17f, 0.25f), () => SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneMainMenu));
            RuntimeUIHelper.CreateButton(root, "▶  PLAY", new Vector2(700, 220), new Vector2(260, 60), new Color(0.95f, 0.6f, 0f), () => SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneOpenWorld));
        }
        void Update()
        {
            // Drag se rotate: mouse / touch drag
            if (Input.GetMouseButton(0))
            {
                manualAngle += Input.GetAxis("Mouse X") * 3f;
                if (carObject != null && !autoRotate) carObject.transform.rotation = Quaternion.Euler(0, manualAngle, 0);
            }
        }
    }
}
