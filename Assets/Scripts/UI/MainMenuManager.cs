using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Data;
using RealDrive.Menu;
using RealDrive.Utils;
using RealDrive.Audio;
namespace RealDrive.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        Canvas canvas;
        GameObject settingsPanel;
        GameObject carsPanel;
        GameObject carObject;
        Text carNameText;

        void Awake()
        {
            SaveManager.ApplyQuality();
            if (FindObjectOfType<AudioManager>() == null)
            {
                GameObject am = new GameObject("AudioManager");
                am.AddComponent<AudioManager>();
            }
            if (FindObjectOfType<SceneLoader>() == null)
            {
                GameObject sl = new GameObject("SceneLoader");
                sl.AddComponent<SceneLoader>();
            }
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            if (FindObjectOfType<MenuMusic>() == null) { GameObject mu = new GameObject("MenuMusic"); mu.AddComponent<MenuMusic>(); }
            Application.targetFrameRate = 60;
        }
        void Start()
        {
            SetupCameraAndCar();
            BuildUI();
        }
        void SetupCameraAndCar()
        {
            Camera cam = Camera.main;
            if (cam == null) cam = FindObjectOfType<Camera>();
            if (cam != null)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.03f, 0.04f, 0.07f);
                cam.transform.position = new Vector3(0, 3.2f, 7f);
                cam.transform.LookAt(new Vector3(0, 0.8f, 0));
            }
            MenuCarBuilder.BuildShowroomFloor(new Vector3(0, 0, 0));
            CarData car = CarCatalog.Selected();
            carObject = MenuCarBuilder.BuildSportsCarCustom(new Vector3(0, 0.1f, 0), car.bodyColor);
            GameObject anim = new GameObject("MenuCarAnimator");
            var a = anim.AddComponent<AnimatedMenuCar>();
            a.carTransform = carObject.transform;
            if (cam != null) a.cameraTransform = cam.transform;
            // lights
            GameObject dl = GameObject.Find("Directional Light");
            if (dl == null) { dl = new GameObject("Directional Light"); dl.AddComponent<Light>().type = LightType.Directional; }
            dl.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
        void BuildUI()
        {
            canvas = RuntimeUIHelper.CreateCanvas("MainMenuCanvas", 10);
            Transform root = canvas.transform;
            // Logo
            Text logo = RuntimeUIHelper.CreateText(root, "REAL DRIVE", 72, new Color(1f, 0.8f, 0f));
            RuntimeUIHelper.SetRect(logo.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -90), new Vector2(900, 100));
            Text sub = RuntimeUIHelper.CreateText(root, "OPEN WORLD  •  FREE ROAM  •  ANDROID", 22, Color.white);
            RuntimeUIHelper.SetRect(sub.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -150), new Vector2(900, 40));
            carNameText = RuntimeUIHelper.CreateText(root, CarCatalog.Selected().displayName + "  •  " + CarCatalog.Selected().topSpeedKmh + " KM/H", 24, new Color(0.6f, 1f, 0.4f));
            RuntimeUIHelper.SetRect(carNameText.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 210), new Vector2(900, 40));
            // Buttons - right side vertical stack, landscape friendly
            Color btnCol = new Color(0.1f, 0.12f, 0.18f, 0.92f);
            RuntimeUIHelper.CreateButton(root, "▶  PLAY", new Vector2(620, 90), new Vector2(320, 70), new Color(0.95f, 0.6f, 0f), OnPlay);
            RuntimeUIHelper.CreateButton(root, "GARAGE", new Vector2(620, 0), new Vector2(320, 60), btnCol, OnGarage);
            RuntimeUIHelper.CreateButton(root, "CARS", new Vector2(620, -80), new Vector2(320, 60), btnCol, OnCars);
            RuntimeUIHelper.CreateButton(root, "SETTINGS", new Vector2(620, -160), new Vector2(320, 60), btnCol, OnSettings);
            RuntimeUIHelper.CreateButton(root, "EXIT", new Vector2(620, -240), new Vector2(320, 60), new Color(0.35f, 0.08f, 0.08f, 0.92f), OnExit);
            Text ver = RuntimeUIHelper.CreateText(root, "v1.0.14  •  Build 14  •  jolet", 18, new Color(1, 1, 1, 0.6f));
            RuntimeUIHelper.SetRect(ver.rectTransform, new Vector2(0, 0), new Vector2(0, 0), new Vector2(140, 30), new Vector2(400, 30));
            BuildSettingsPanel(root);
            BuildCarsPanel(root);
        }
        void BuildSettingsPanel(Transform root)
        {
            settingsPanel = RuntimeUIHelper.CreatePanel(root, Vector2.zero, new Vector2(900, 560), new Color(0.05f, 0.06f, 0.1f, 0.97f));
            settingsPanel.SetActive(false);
            Text t = RuntimeUIHelper.CreateText(settingsPanel.transform, "SETTINGS", 36, Color.white);
            RuntimeUIHelper.SetRect(t.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -50), new Vector2(600, 60));
            // sliders are added by SettingsManager at runtime on this panel
            var sm = settingsPanel.AddComponent<SettingsManager>();
            sm.Build(settingsPanel.transform, () => { settingsPanel.SetActive(false); });
        }
        void BuildCarsPanel(Transform root)
        {
            carsPanel = RuntimeUIHelper.CreatePanel(root, Vector2.zero, new Vector2(1000, 600), new Color(0.05f, 0.06f, 0.1f, 0.97f));
            carsPanel.SetActive(false);
            Text t = RuntimeUIHelper.CreateText(carsPanel.transform, "SELECT CAR", 36, Color.white);
            RuntimeUIHelper.SetRect(t.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -50), new Vector2(600, 60));
            var cm = carsPanel.AddComponent<CarSelectionManager>();
            cm.Build(carsPanel.transform, () => { carsPanel.SetActive(false); RefreshCar(); });
        }
        void RefreshCar()
        {
            CarData car = CarCatalog.Selected();
            if (carNameText != null) carNameText.text = car.displayName + "  •  " + car.topSpeedKmh + " KM/H";
            if (carObject != null) Destroy(carObject);
            carObject = MenuCarBuilder.BuildSportsCarCustom(new Vector3(0, 0.1f, 0), car.bodyColor);
            var anim = FindObjectOfType<AnimatedMenuCar>();
            if (anim != null) anim.carTransform = carObject.transform;
        }
        void OnPlay() { SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneOpenWorld); }
        void OnGarage() { SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneGarage); }
        void OnCars() { carsPanel.SetActive(true); }
        void OnSettings() { settingsPanel.SetActive(true); }
        void OnExit() { SceneLoader.Instance.QuitGame(); }
    }
}
