using UnityEngine;
namespace RealDrive.Vehicle
{
    [RequireComponent(typeof(AudioSource))]
    public class EngineAudio : MonoBehaviour
    {
        AudioSource src;
        CarController car;
        void Awake()
        {
            src = GetComponent<AudioSource>();
            car = GetComponent<CarController>();
            src.loop = true; src.playOnAwake = false; src.volume = 0.25f;
            src.clip = CreateEngineLoop();
            if (src.clip != null) src.Play();
        }
        void Update()
        {
            if (src == null || car == null) return;
            float t = Mathf.Clamp01(car.rpm / 6500f);
            src.pitch = 0.7f + t * 1.4f;
            bool driving = car.isDrivingEnabled;
            float throttle = (MobileInput.Accelerator || MobileInput.KeyboardAccel()) ? 1f : 0.3f;
            src.volume = driving ? 0.12f + t * 0.25f * throttle : 0f;
            if (!driving && src.isPlaying) src.Pause();
            if (driving && !src.isPlaying) src.Play();
        }
        AudioClip CreateEngineLoop()
        {
            int rate = 22050; int len = rate / 2;
            AudioClip clip = AudioClip.Create("EngineLoop", len, 1, rate, false);
            float[] d = new float[len];
            for (int i = 0; i < len; i++)
            {
                float t = (float)i / rate;
                d[i] = (Mathf.Sin(2f * Mathf.PI * 90f * t) * 0.5f + Mathf.Sin(2f * Mathf.PI * 180f * t) * 0.3f) * 0.4f;
            }
            clip.SetData(d, 0);
            return clip;
        }
    }
}
