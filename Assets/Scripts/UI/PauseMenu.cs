using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Utils;
using RealDrive.Vehicle;
namespace RealDrive.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public CarController car;
        public CameraController camController;
        public System.Action onResume;
        GameObject panel;
        bool isPaused;
        bool wasDriving;
        public void Build(Transform root, CarController c, CameraController cam)
        {
            car = c; camController = cam;
            panel = RuntimeUIHelper.CreatePanel(root, Vector2.zero, new Vector2(900, 600), new Color(0.03f, 0.04f, 0.07f, 0.97f));
            panel.SetActive(false);
            Text t = RuntimeUIHelper.CreateText(panel.transform, "PAUSED", 44, Color.white);
            RuntimeUIHelper.SetRect(t.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -60), new Vector2(600, 60));
            RuntimeUIHelper.CreateButton(panel.transform, "▶  RESUME", new Vector2(0, 140), new Vector2(320, 60), new Color(0.95f, 0.6f, 0f), Resume);
            RuntimeUIHelper.CreateButton(panel.transform, "↻  RESTART POSITION", new Vector2(0, 60), new Vector2(320, 60), new Color(0.2f, 0.22f, 0.3f), () => { car?.Respawn(new Vector3(0, 1f, 0), Quaternion.identity); Resume(); });
            RuntimeUIHelper.CreateButton(panel.transform, "CAMERA: CHANGE", new Vector2(0, -20), new Vector2(320, 60), new Color(0.2f, 0.22f, 0.3f), () => camController?.NextCamera());
            RuntimeUIHelper.CreateButton(panel.transform, "WEATHER: CHANGE", new Vector2(0, -100), new Vector2(320, 60), new Color(0.2f, 0.22f, 0.3f), () => { var ws = FindObjectOfType<RealDrive.World.WeatherSystem>(); if (ws != null) ws.CycleWeather(); });
            // Settings sliders - sound / music
            Text sLbl = RuntimeUIHelper.CreateText(panel.transform, "SOUND", 18, Color.white, TextAnchor.MiddleLeft);
            RuntimeUIHelper.SetRect(sLbl.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-180, -170), new Vector2(120, 30));
            Slider s = RuntimeUIHelper.CreateSlider(panel.transform, new Vector2(120, -170), new Vector2(280, 28), SaveManager.SoundVolume);
            s.onValueChanged.AddListener(v => { SaveManager.SoundVolume = v; AudioManager.Instance?.ApplyVolumes(); });
            Text mLbl = RuntimeUIHelper.CreateText(panel.transform, "MUSIC", 18, Color.white, TextAnchor.MiddleLeft);
            RuntimeUIHelper.SetRect(mLbl.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-180, -210), new Vector2(120, 30));
            Slider m = RuntimeUIHelper.CreateSlider(panel.transform, new Vector2(120, -210), new Vector2(280, 28), SaveManager.MusicVolume);
            m.onValueChanged.AddListener(v => { SaveManager.MusicVolume = v; AudioManager.Instance?.ApplyVolumes(); });
            RuntimeUIHelper.CreateButton(panel.transform, "MAIN MENU", new Vector2(0, -260), new Vector2(320, 55), new Color(0.35f, 0.08f, 0.08f), () => { Resume(); SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneMainMenu); });
        }
        public void Show()
        {
            if (panel == null) return;
            isPaused = true;
            panel.SetActive(true);
            Time.timeScale = 0f;
            if (car != null) { wasDriving = car.isDrivingEnabled; car.isDrivingEnabled = false; }
            MobileInput.ResetAll();
        }
        public void Resume()
        {
            if (panel == null) return;
            isPaused = false;
            panel.SetActive(false);
            Time.timeScale = 1f;
            if (car != null) car.isDrivingEnabled = wasDriving;
            onResume?.Invoke();
        }
        public bool IsPaused() { return isPaused; }
        public void Toggle() { if (isPaused) Resume(); else Show(); }
    }
}
