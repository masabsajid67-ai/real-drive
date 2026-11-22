using UnityEngine;
using RealDrive.Core;
using RealDrive.Data;
using RealDrive.Menu;
using RealDrive.Utils;
using RealDrive.Vehicle;
using RealDrive.Player;
using RealDrive.Traffic;
using RealDrive.Audio;
namespace RealDrive.World
{
    // Stage 2: OpenWorld placeholder ab asli driving world hai.
    // NO MISSION - sirf free roam + driving + on-foot walk + car enter/exit.
    public class OpenWorldPlaceholder : MonoBehaviour
    {
        void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            if (FindObjectOfType<SceneLoader>() == null) { GameObject sl = new GameObject("SceneLoader"); sl.AddComponent<SceneLoader>(); }
            if (FindObjectOfType<AudioManager>() == null) { GameObject am = new GameObject("AudioManager"); am.AddComponent<AudioManager>(); }
            CityBuilder.BuildCity();
            GameObject mmGo = new GameObject("MinimapController");
            MinimapController mm = mmGo.AddComponent<MinimapController>();
            CarData carData = CarCatalog.Selected();
            Color col = ColorUtility.TryParseHtmlString(SaveManager.CarColorHex, out Color c) ? c : carData.bodyColor;
            CarController car = DrivableCarBuilder.BuildDrivableCar(new Vector3(0, 1f, 0), col, carData.topSpeedKmh, carData.acceleration);
            GameObject player = PlayerCharacterBuilder.BuildPlayer(new Vector3(3f, 0.5f, 0));
            Camera cam = Camera.main ?? FindObjectOfType<Camera>();
            if (cam == null) { GameObject cg = new GameObject("Main Camera"); cam = cg.AddComponent<Camera>(); cam.tag = "MainCamera"; }
            cam.backgroundColor = new Color(0.5f, 0.75f, 0.95f);
            CameraController camCtrl = cam.gameObject.AddComponent<CameraController>();
            camCtrl.carTarget = car.transform;
            camCtrl.playerTarget = player.transform;
            mm.followTarget = car.transform;
            GameObject respGo = new GameObject("RespawnManager");
            RespawnManager resp = respGo.AddComponent<RespawnManager>();
            resp.car = car;
            GameObject lightGo = new GameObject("TrafficLightVisualUpdater");
            lightGo.AddComponent<TrafficLightVisualUpdater>();
            GameObject trafficGo = new GameObject("TrafficManager");
            TrafficManager traffic = trafficGo.AddComponent<TrafficManager>();
            traffic.Setup(car.transform);
            GameObject vmGo = new GameObject("PlayerVehicleManager");
            PlayerVehicleManager vm = vmGo.AddComponent<PlayerVehicleManager>();
            vm.Setup(car, player, camCtrl);
            // Stage 6 polish
            GameObject dnGo = new GameObject("DayNightCycle");
            DayNightCycle dn = dnGo.AddComponent<DayNightCycle>();
            dn.playerCar = car;
            GameObject wGo = new GameObject("WeatherSystem");
            WeatherSystem ws = wGo.AddComponent<WeatherSystem>();
            ws.followTarget = car.transform;
            car.gameObject.AddComponent<CollisionAudio>();
            GameObject ambGo = new GameObject("CityAmbience");
            ambGo.AddComponent<CityAmbience>();
            // Clouds - safed halke spheres, aasmaan me
            for (int i = 0; i < 6; i++)
            {
                GameObject cl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cl.name = "Cloud_" + i;
                cl.transform.position = new Vector3(-150f + i * 60f, 55f + (i % 3) * 8f, -100f + (i % 2) * 150f);
                cl.transform.localScale = new Vector3(28, 6, 16);
                cl.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.85f);
                Object.Destroy(cl.GetComponent<Collider>());
            }
            GameObject hudGo = new GameObject("DrivingHUD");
            DrivingHUD hud = hudGo.AddComponent<DrivingHUD>();
            hud.Setup(car, vm, camCtrl);
        }

    }
}
