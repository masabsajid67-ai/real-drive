using UnityEngine;
namespace RealDrive.World
{
    // Stage 6: din-raat cycle. Time 0-24, auto mode me 300 sec me poora din.
    // Suraj ghoomta hai, aasmaan ka rang, roshni aur fog badalta hai. Raat ko headlights kaam aate hain.
    public class DayNightCycle : MonoBehaviour
    {
        public float timeOfDay = 10f; // 10 AM se shuru
        public float dayLengthSeconds = 300f;
        public bool autoCycle = true;
        public Light sunLight;
        public RealDrive.Vehicle.CarController playerCar;
        Color daySky = new Color(0.5f, 0.75f, 0.95f);
        Color sunsetSky = new Color(0.95f, 0.55f, 0.3f);
        Color nightSky = new Color(0.03f, 0.04f, 0.1f);
        void Start()
        {
            if (sunLight == null)
            {
                GameObject dl = GameObject.Find("Directional Light");
                if (dl != null) sunLight = dl.GetComponent<Light>();
            }
            if (sunLight != null) sunLight.shadows = LightShadows.Soft;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = 0.002f;
        }
        void Update()
        {
            if (autoCycle) timeOfDay = (timeOfDay + Time.deltaTime * 24f / dayLengthSeconds) % 24f;
            float sunAngle = (timeOfDay / 24f) * 360f - 90f;
            if (sunLight != null)
            {
                sunLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0);
                bool isDay = timeOfDay > 6f && timeOfDay < 18f;
                bool isSunset = (timeOfDay > 17f && timeOfDay < 19f) || (timeOfDay > 5f && timeOfDay < 7f);
                sunLight.intensity = isDay ? 1.2f : (isSunset ? 0.7f : 0.15f);
                sunLight.color = isSunset ? new Color(1f, 0.6f, 0.35f) : Color.white;
                RenderSettings.ambientIntensity = isDay ? 1f : 0.35f;
            }
            Camera cam = Camera.main;
            if (cam != null)
            {
                if (timeOfDay >= 6f && timeOfDay < 17f) cam.backgroundColor = daySky;
                else if (timeOfDay >= 17f && timeOfDay < 19f) cam.backgroundColor = sunsetSky;
                else if (timeOfDay >= 5f && timeOfDay < 6f) cam.backgroundColor = sunsetSky;
                else cam.backgroundColor = nightSky;
            }
            RenderSettings.fogColor = Camera.main != null ? Camera.main.backgroundColor : daySky;
            // Raat ko headlights auto on ka suggestion: car ko on kar do agar raat hai aur car driving me hai
            bool isNight = timeOfDay < 5.5f || timeOfDay > 18.5f;
            if (playerCar != null && isNight && playerCar.isDrivingEnabled && !playerCar.headlightsOn)
            {
                playerCar.SetHeadlights(true);
            }
        }
        public void SetTime(float hour) { timeOfDay = hour; autoCycle = false; }
        public void SetDay() { SetTime(10f); }
        public void SetSunset() { SetTime(18f); }
        public void SetNight() { SetTime(22f); }
        public void ToggleAuto() { autoCycle = !autoCycle; }
        public string TimeLabel()
        {
            int h = Mathf.FloorToInt(timeOfDay);
            int m = Mathf.FloorToInt((timeOfDay - h) * 60f);
            string period = timeOfDay < 12f ? "AM" : "PM";
            int hh = h % 12; if (hh == 0) hh = 12;
            return hh + ":" + m.ToString("00") + " " + period + (autoCycle ? " • AUTO" : "");
        }
    }
}
