using UnityEngine;
namespace RealDrive.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        AudioSource musicSource;
        AudioSource sfxSource;
        AudioClip clickClip;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true; musicSource.playOnAwake = false;
            sfxSource.loop = false; sfxSource.playOnAwake = false;
            clickClip = CreateTone(880f, 0.08f);
            ApplyVolumes();
        }
        public void ApplyVolumes()
        {
            if (musicSource != null) musicSource.volume = SaveManager.MusicVolume * 0.5f;
            if (sfxSource != null) sfxSource.volume = SaveManager.SoundVolume;
            AudioListener.volume = Mathf.Clamp01(SaveManager.SoundVolume);
        }
        public void PlayClick()
        {
            if (sfxSource != null && clickClip != null) sfxSource.PlayOneShot(clickClip);
        }
        public void SetMusicVolume(float v) { SaveManager.MusicVolume = v; ApplyVolumes(); }
        public void SetSoundVolume(float v) { SaveManager.SoundVolume = v; ApplyVolumes(); }

        AudioClip CreateTone(float freq, float dur)
        {
            int rate = 44100; int len = (int)(rate * dur);
            AudioClip clip = AudioClip.Create("Click", len, 1, rate, false);
            float[] data = new float[len];
            for (int i = 0; i < len; i++) data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / rate) * (1f - (float)i / len) * 0.5f;
            clip.SetData(data, 0);
            return clip;
        }
    }
}
