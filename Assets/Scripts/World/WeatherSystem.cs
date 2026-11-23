using UnityEngine;
namespace RealDrive.World
{
    public enum Weather { Sunny, Cloudy, Rain }
    public class WeatherSystem : MonoBehaviour
    {
        public Weather current = Weather.Sunny;
        public Transform followTarget;
        ParticleSystem rain;
        Light sunLight;
        void Start()
        {
            GameObject dl = GameObject.Find("Directional Light");
            if (dl != null) sunLight = dl.GetComponent<Light>();
            // Rain particles - player ke upar box se girte hain
            GameObject go = new GameObject("RainParticles");
            go.transform.SetParent(transform, false);
            rain = go.AddComponent<ParticleSystem>();
            var main = rain.main;
            main.loop = true; main.startLifetime = 0.8f; main.startSpeed = 25f; main.startSize = 0.08f; main.maxParticles = 2000;
            main.startColor = new Color(0.7f, 0.8f, 1f, 0.6f);
            var em = rain.emission; em.rateOverTime = 800f;
            var shape = rain.shape; shape.shapeType = ParticleSystemShapeType.Box; shape.scale = new Vector3(40, 1, 40);
            var rend = go.GetComponent<ParticleSystemRenderer>();
            Shader rs = Shader.Find("Standard");
            if (rs == null) rs = Shader.Find("Universal Render Pipeline/Lit"); if (rs == null) rs = Shader.Find("Unlit/Color");
            if (rs != null) rend.material = new Material(rs);
            rain.Stop();
        }
        void Update()
        {
            if (rain != null && followTarget != null)
            {
                rain.transform.position = followTarget.position + new Vector3(0, 15f, 0);
            }
        }
        public void SetWeather(Weather w)
        {
            current = w;
            if (w == Weather.Sunny)
            {
                if (rain != null) rain.Stop();
                RenderSettings.fogDensity = 0.002f;
                if (sunLight != null) sunLight.intensity = 1.2f;
                SetRoadWet(false);
            }
            else if (w == Weather.Cloudy)
            {
                if (rain != null) rain.Stop();
                RenderSettings.fogDensity = 0.006f;
                if (sunLight != null) sunLight.intensity = 0.7f;
                SetRoadWet(false);
            }
            else // Rain
            {
                if (rain != null) rain.Play();
                RenderSettings.fogDensity = 0.01f;
                if (sunLight != null) sunLight.intensity = 0.5f;
                SetRoadWet(true);
            }
        }
        void SetRoadWet(bool wet)
        {
            // Roads ka rang geela hone par gehra ho jata hai, dry par wapas
            var renderers = FindObjectsOfType<Renderer>();
            foreach (var r in renderers)
            {
                if (r.gameObject.name.StartsWith("Road_") || r.gameObject.name.StartsWith("Highway") || r.gameObject.name.StartsWith("Bridge"))
                {
                    if (r.material != null)
                    {
                        r.material.color = wet ? new Color(0.05f, 0.05f, 0.07f) : new Color(0.12f, 0.12f, 0.14f);
                        r.material.SetFloat("_Smoothness", wet ? 0.95f : 0.4f);
                    }
                }
            }
        }
        public void CycleWeather()
        {
            if (current == Weather.Sunny) SetWeather(Weather.Cloudy);
            else if (current == Weather.Cloudy) SetWeather(Weather.Rain);
            else SetWeather(Weather.Sunny);
        }
        public string Label() { return current.ToString().ToUpper(); }
    }
}
