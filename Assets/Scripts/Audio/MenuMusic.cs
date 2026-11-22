using UnityEngine;
namespace RealDrive.Audio
{
    public class MenuMusic : MonoBehaviour
    {
        AudioSource src;
        void Awake()
        {
            src = gameObject.AddComponent<AudioSource>();
            src.loop = true; src.playOnAwake = false;
            src.volume = RealDrive.Core.SaveManager.MusicVolume * 0.3f;
            int rate = 22050; int len = rate * 4;
            AudioClip clip = AudioClip.Create("MenuMusic", len, 1, rate, false);
            float[] d = new float[len];
            float[] notes = { 220f, 277f, 330f, 440f };
            for (int i = 0; i < len; i++)
            {
                float t = (float)i / rate;
                int n = (int)(t) % 4;
                d[i] = Mathf.Sin(2f * Mathf.PI * notes[n] * t) * 0.25f * (1f - (t % 1f) * 0.5f);
            }
            clip.SetData(d, 0);
            src.clip = clip;
            src.Play();
        }
        void Update()
        {
            if (src != null) src.volume = RealDrive.Core.SaveManager.MusicVolume * 0.3f;
        }
    }
}
