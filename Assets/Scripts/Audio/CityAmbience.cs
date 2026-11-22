using UnityEngine;
namespace RealDrive.Audio
{
    public class CityAmbience : MonoBehaviour
    {
        AudioSource src;
        void Awake()
        {
            src = gameObject.AddComponent<AudioSource>();
            src.loop = true; src.playOnAwake = false; src.volume = 0.08f;
            int rate = 22050; int len = rate * 2;
            AudioClip clip = AudioClip.Create("CityAmbience", len, 1, rate, false);
            float[] d = new float[len];
            // Halki hawa + city hum: low sine + noise
            System.Random rnd = new System.Random(123);
            for (int i = 0; i < len; i++)
            {
                float t = (float)i / rate;
                d[i] = (Mathf.Sin(2f * Mathf.PI * 55f * t) * 0.3f + (float)(rnd.NextDouble() * 0.2 - 0.1) * 0.3f) * 0.5f;
            }
            clip.SetData(d, 0);
            src.clip = clip;
            src.Play();
        }
        void Update()
        {
            if (src != null) src.volume = RealDrive.Core.SaveManager.SoundVolume * 0.12f;
        }
    }
}
