using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RealDrive.Core;
using RealDrive.Vehicle;
using RealDrive.Player;
using RealDrive.Utils;
using RealDrive.World;
using RealDrive.Audio;
namespace RealDrive.UI
{
    public class DrivingHUD : MonoBehaviour
    {
        public CarController car;
        public PlayerVehicleManager vehicleManager;
        public CameraController camController;
        Canvas canvas;
        Text speedText;
        Text gearText;
        Text rpmText;
        Text stateText;
        GameObject drivingControls;
        GameObject onFootControls;
        Button enterExitButton;
        PauseMenu pauseMenu;
        UnityEngine.UI.Text timeText;
        UnityEngine.UI.Text weatherText;
        GameObject mapPanel;
        UnityEngine.UI.RawImage minimapImage;
        UnityEngine.UI.RawImage bigMapImage;
        Button nitroButton;

        public void Setup(CarController c, PlayerVehicleManager vm, CameraController cam)
        {
            car = c; vehicleManager = vm; camController = cam;
            if (vm != null) vm.OnStateChanged += OnStateChanged;
            Build();
            GameObject pmGo = new GameObject("PauseMenu");
            pauseMenu = pmGo.AddComponent<PauseMenu>();
            pauseMenu.Build(canvas.transform, car, cam);
            // Weather + Time buttons - bottom centre
            RuntimeUIHelper.CreateButton(canvas.transform, "WEATHER", new Vector2(0, -380), new Vector2(180, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => { var ws = FindObjectOfType<WeatherSystem>(); ws?.CycleWeather(); if (weatherText != null && ws != null) weatherText.text = ws.Label(); });
            RuntimeUIHelper.CreateButton(canvas.transform, "TIME: DAY", new Vector2(200, -380), new Vector2(180, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => { var dn = FindObjectOfType<DayNightCycle>(); if (dn != null) { dn.SetDay(); if (timeText != null) timeText.text = dn.TimeLabel(); } });
            RuntimeUIHelper.CreateButton(canvas.transform, "TIME: NIGHT", new Vector2(-200, -380), new Vector2(180, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => { var dn = FindObjectOfType<DayNightCycle>(); if (dn != null) { dn.SetNight(); if (timeText != null) timeText.text = dn.TimeLabel(); } });
            timeText = RuntimeUIHelper.CreateText(canvas.transform, "10:00 AM", 18, Color.white);
            RuntimeUIHelper.SetRect(timeText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -140), new Vector2(300, 30));
            weatherText = RuntimeUIHelper.CreateText(canvas.transform, "SUNNY", 18, Color.white);
            RuntimeUIHelper.SetRect(weatherText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -170), new Vector2(300, 30));
            OnStateChanged(true);
        }
        void Build()
        {
            canvas = RuntimeUIHelper.CreateCanvas("DrivingHUD", 20);
            Transform root = canvas.transform;
            // Top: speedometer
            speedText = RuntimeUIHelper.CreateText(root, "0 KM/H", 44, Color.white);
            RuntimeUIHelper.SetRect(speedText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -55), new Vector2(400, 60));
            gearText = RuntimeUIHelper.CreateText(root, "Gear 1", 24, new Color(1f, 0.8f, 0f));
            RuntimeUIHelper.SetRect(gearText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-180, -55), new Vector2(200, 40));
            rpmText = RuntimeUIHelper.CreateText(root, "RPM 900", 20, Color.white);
            RuntimeUIHelper.SetRect(rpmText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(180, -55), new Vector2(220, 40));
            stateText = RuntimeUIHelper.CreateText(root, "DRIVING", 20, new Color(0.6f, 1f, 0.4f));
            RuntimeUIHelper.SetRect(stateText.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -105), new Vector2(400, 35));
            // Minimap - top left, Stage 3
            GameObject mmGo = new GameObject("Minimap");
            mmGo.transform.SetParent(root, false);
            RectTransform mmr = mmGo.AddComponent<RectTransform>();
            RuntimeUIHelper.SetRect(mmr, new Vector2(0, 1), new Vector2(0, 1), new Vector2(110, -110), new Vector2(200, 200));
            minimapImage = mmGo.AddComponent<UnityEngine.UI.RawImage>();
            minimapImage.color = Color.white;
            if (MinimapController.Instance != null) minimapImage.texture = MinimapController.Instance.MapTexture;
            RuntimeUIHelper.CreateButton(root, "MAP", new Vector2(-820, 460), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => { if (mapPanel != null) mapPanel.SetActive(!mapPanel.activeSelf); });
            // World map panel - bada map
            mapPanel = RuntimeUIHelper.CreatePanel(root, Vector2.zero, new Vector2(1100, 650), new Color(0.03f, 0.04f, 0.07f, 0.97f));
            mapPanel.SetActive(false);
            UnityEngine.UI.Text mt = RuntimeUIHelper.CreateText(mapPanel.transform, "WORLD MAP  •  Downtown NW  •  Residential NE  •  Commercial SE  •  Suburban SW  •  Highway North", 20, Color.white);
            RuntimeUIHelper.SetRect(mt.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -40), new Vector2(1050, 35));
            GameObject bigGo = new GameObject("BigMap");
            bigGo.transform.SetParent(mapPanel.transform, false);
            RectTransform br = bigGo.AddComponent<RectTransform>();
            RuntimeUIHelper.SetRect(br, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -20), new Vector2(520, 520));
            bigMapImage = bigGo.AddComponent<UnityEngine.UI.RawImage>();
            if (MinimapController.Instance != null) bigMapImage.texture = MinimapController.Instance.MapTexture;
            RuntimeUIHelper.CreateButton(mapPanel.transform, "CLOSE MAP", new Vector2(0, -280), new Vector2(260, 55), new Color(0.95f, 0.6f, 0f), () => mapPanel.SetActive(false));
            // Top right buttons
            RuntimeUIHelper.CreateButton(root, "CAM", new Vector2(820, 460), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => camController?.NextCamera());
            RuntimeUIHelper.CreateButton(root, "LIGHT", new Vector2(660, 460), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => car?.ToggleHeadlights());
            RuntimeUIHelper.CreateButton(root, "II", new Vector2(820, 390), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => pauseMenu?.Toggle());
            RuntimeUIHelper.CreateButton(root, "RESPAWN", new Vector2(660, 390), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => car?.Respawn(new Vector3(0, 1f, 0), Quaternion.identity));
            // Driving controls group
            drivingControls = new GameObject("DrivingControls");
            drivingControls.transform.SetParent(root, false);
            RectTransform dr = drivingControls.AddComponent<RectTransform>();
            dr.anchorMin = Vector2.zero; dr.anchorMax = Vector2.one; dr.offsetMin = Vector2.zero; dr.offsetMax = Vector2.zero;
            // Steering left/right - left side
            AddHoldButton(drivingControls.transform, "◀", new Vector2(-780, -180), new Vector2(130, 130), (down) => { MobileInput.Steer = down ? -1f : 0f; });
            AddHoldButton(drivingControls.transform, "▶", new Vector2(-620, -180), new Vector2(130, 130), (down) => { MobileInput.Steer = down ? 1f : 0f; });
            // Accelerator / Brake right side - press and hold
            AddHoldButton(drivingControls.transform, "▲\nACCEL", new Vector2(780, -140), new Vector2(150, 150), (down) => { MobileInput.Accelerator = down; });
            AddHoldButton(drivingControls.transform, "▼\nBRAKE", new Vector2(600, -180), new Vector2(140, 130), (down) => { MobileInput.Brake = down; });
            AddHoldButton(drivingControls.transform, "HANDBRAKE", new Vector2(780, -320), new Vector2(170, 65), (down) => { MobileInput.Handbrake = down; });
            nitroButton = AddHoldButton(drivingControls.transform, "NITRO", new Vector2(600, -320), new Vector2(140, 65), (down) => { MobileInput.Nitro = down; });
            // Exit car button
            RuntimeUIHelper.CreateButton(drivingControls.transform, "GET OUT", new Vector2(-780, 180), new Vector2(220, 60), new Color(0.35f, 0.08f, 0.08f, 0.9f), () => vehicleManager?.ExitCar());
            // On-foot controls group
            onFootControls = new GameObject("OnFootControls");
            onFootControls.transform.SetParent(root, false);
            RectTransform fr = onFootControls.AddComponent<RectTransform>();
            fr.anchorMin = Vector2.zero; fr.anchorMax = Vector2.one; fr.offsetMin = Vector2.zero; fr.offsetMax = Vector2.zero;
            AddHoldButton(onFootControls.transform, "▲", new Vector2(-700, -120), new Vector2(120, 110), (down) => { MobileInput.MoveStick = down ? new Vector2(0, 1) : Vector2.zero; });
            AddHoldButton(onFootControls.transform, "◀", new Vector2(-820, -220), new Vector2(110, 100), (down) => { MobileInput.MoveStick = down ? new Vector2(-1, 0) : Vector2.zero; });
            AddHoldButton(onFootControls.transform, "▶", new Vector2(-580, -220), new Vector2(110, 100), (down) => { MobileInput.MoveStick = down ? new Vector2(1, 0) : Vector2.zero; });
            AddHoldButton(onFootControls.transform, "▼", new Vector2(-700, -320), new Vector2(120, 100), (down) => { MobileInput.MoveStick = down ? new Vector2(0, -1) : Vector2.zero; });
            AddHoldButton(onFootControls.transform, "RUN", new Vector2(700, -180), new Vector2(150, 80), (down) => { MobileInput.Run = down; });
            enterExitButton = RuntimeUIHelper.CreateButton(onFootControls.transform, "ENTER CAR", new Vector2(700, -300), new Vector2(220, 70), new Color(0.95f, 0.6f, 0f), () => vehicleManager?.EnterCar());
            RuntimeUIHelper.CreateButton(onFootControls.transform, "MENU", new Vector2(820, 390), new Vector2(140, 55), new Color(0.1f, 0.12f, 0.18f, 0.9f), () => SceneLoader.Instance.LoadWithLoadingScreen(GameConstants.SceneMainMenu));
        }
        Button AddHoldButton(Transform parent, string label, Vector2 pos, Vector2 size, System.Action<bool> onHold)
        {
            Button b = RuntimeUIHelper.CreateButton(parent, label, pos, size, new Color(0.1f, 0.12f, 0.18f, 0.85f), null);
            EventTrigger tr = b.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry down = new EventTrigger.Entry(); down.eventID = EventTriggerType.PointerDown;
            down.callback.AddListener((e) => onHold(true));
            EventTrigger.Entry up = new EventTrigger.Entry(); up.eventID = EventTriggerType.PointerUp;
            up.callback.AddListener((e) => onHold(false));
            EventTrigger.Entry exit = new EventTrigger.Entry(); exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((e) => onHold(false));
            tr.triggers.Add(down); tr.triggers.Add(up); tr.triggers.Add(exit);
            return b;
        }
        void OnStateChanged(bool driving)
        {
            if (drivingControls != null) drivingControls.SetActive(driving);
            if (onFootControls != null) onFootControls.SetActive(!driving);
            if (stateText != null) stateText.text = driving ? "DRIVING  •  E = Get Out" : "ON FOOT  •  Car ke paas jao, ENTER CAR dabao";
        }
        void Update()
        {
            if (car != null && speedText != null)
            {
                speedText.text = Mathf.RoundToInt(car.speedKmh) + " KM/H";
                gearText.text = car.gear == -1 ? "Gear R" : "Gear " + car.gear;
                rpmText.text = "RPM " + Mathf.RoundToInt(car.rpm);
            }
            if (vehicleManager != null && enterExitButton != null)
            {
                enterExitButton.gameObject.SetActive(vehicleManager.CanEnterCar());
            }
            if (minimapImage != null && minimapImage.texture == null && MinimapController.Instance != null) minimapImage.texture = MinimapController.Instance.MapTexture;
            if (bigMapImage != null && bigMapImage.texture == null && MinimapController.Instance != null) bigMapImage.texture = MinimapController.Instance.MapTexture;
            if (Input.GetKeyDown(KeyCode.M) && mapPanel != null) mapPanel.SetActive(!mapPanel.activeSelf);
            var dn2 = FindObjectOfType<DayNightCycle>();
            if (dn2 != null && timeText != null && Time.frameCount % 60 == 0) timeText.text = dn2.TimeLabel();
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) pauseMenu?.Toggle();
            // Keyboard camera switch
            if (Input.GetKeyDown(KeyCode.C)) camController?.NextCamera();
            if (Input.GetKeyDown(KeyCode.L)) car?.ToggleHeadlights();
        }
    }
}
