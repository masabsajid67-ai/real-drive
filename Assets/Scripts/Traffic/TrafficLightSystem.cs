using UnityEngine;
namespace RealDrive.Traffic
{
    // Stage 4: traffic lights ka cycle. NS direction aur EW direction opposite chalte hain.
    // Green 6s -> Yellow 2s -> Red 6s -> Yellow 2s, phir repeat.
    public enum LightState { Green, Yellow, Red }
    public static class TrafficLightSystem
    {
        public static float CycleTime = 16f;
        public static float TimeInCycle { get; private set; }
        public static void Tick(float dt) { TimeInCycle = (TimeInCycle + dt) % CycleTime; }
        public static LightState NorthSouth
        {
            get
            {
                if (TimeInCycle < 6f) return LightState.Green;
                if (TimeInCycle < 8f) return LightState.Yellow;
                if (TimeInCycle < 14f) return LightState.Red;
                return LightState.Yellow;
            }
        }
        public static LightState EastWest
        {
            get
            {
                LightState ns = NorthSouth;
                if (ns == LightState.Green) return LightState.Red;
                if (ns == LightState.Red) return LightState.Green;
                return LightState.Yellow;
            }
        }
        // Car X axis par chal rahi hai to EW light, Z axis par to NS light
        public static LightState ForAxis(bool movingAlongX)
        {
            return movingAlongX ? EastWest : NorthSouth;
        }
        public static bool MustStop(bool movingAlongX)
        {
            LightState s = ForAxis(movingAlongX);
            return s == LightState.Red || s == LightState.Yellow;
        }
    }
    public class TrafficLightVisualUpdater : MonoBehaviour
    {
        Renderer[] reds, yellows, greens;
        LightState lastNS = LightState.Green;
        void Start()
        {
            // CityBuilder ne TL_0=red, TL_1=yellow, TL_2=green spheres banaye hain, 4 intersections par
            var all = FindObjectsOfType<Renderer>();
            var r = new System.Collections.Generic.List<Renderer>();
            var y = new System.Collections.Generic.List<Renderer>();
            var g = new System.Collections.Generic.List<Renderer>();
            foreach (var ren in all)
            {
                if (ren.gameObject.name == "TL_0") r.Add(ren);
                else if (ren.gameObject.name == "TL_1") y.Add(ren);
                else if (ren.gameObject.name == "TL_2") g.Add(ren);
            }
            reds = r.ToArray(); yellows = y.ToArray(); greens = g.ToArray();
        }
        void Update()
        {
            TrafficLightSystem.Tick(Time.deltaTime);
            LightState ns = TrafficLightSystem.NorthSouth;
            if (ns == lastNS) return;
            lastNS = ns;
            SetAll(reds, ns == LightState.Red, Color.red);
            SetAll(yellows, ns == LightState.Yellow, Color.yellow);
            SetAll(greens, ns == LightState.Green, Color.green);
        }
        void SetAll(Renderer[] arr, bool on, Color c)
        {
            if (arr == null) return;
            foreach (var ren in arr)
            {
                if (ren == null || ren.material == null) continue;
                ren.material.color = on ? c : new Color(0.25f, 0.25f, 0.25f);
                if (on) { ren.material.EnableKeyword("_EMISSION"); ren.material.SetColor("_EmissionColor", c * 1.2f); }
                else { ren.material.SetColor("_EmissionColor", Color.black); }
            }
        }
    }
}
