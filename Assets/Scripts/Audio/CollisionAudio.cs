using UnityEngine;
namespace RealDrive.Audio
{
    public class CollisionAudio : MonoBehaviour
    {
        AudioSource src;
        AudioClip thudClip;
        AudioClip screechClip;
        float lastPlay;
        void Awake()
        {
            src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false; src.volume = 0.6f;
            thudClip = CreateTone(90f, 0.2f, 0.9f);
            screechClip = CreateTone(1200f, 0.4f, 0.3f);
        }
        void OnCollisionEnter(Collision col)
        {
            if (Time.time - lastPlay < 0.3f) return;
            float impact = col.relativeVelocity.magnitude;
            if (impact < 2f) return;
            lastPlay = Time.time;
            if (src != null && thudClip != null) src.PlayOneShot(thudClip, Mathf.Clamp01(impact / 15f));
        }
        public void PlayScreech()
        {
            if (src != null && screechClip != null && !src.isPlaying) src.PlayOneShot(screechClip, 0.3f);
        }
        AudioClip CreateTone(float freq, float dur, float vol)
        {
            int rate = 22050; int len = (int)(rate * dur);
            AudioClip clip = AudioClip.Create("Sfx_" + freq, len, 1, rate, false);
            float[] d = new float[len];
            for (int i = 0; i < len; i++) d[i] = Mathf.Sin(2f * Mathf.PI * freq * i / rate) * (1f - (float)i / len) * vol;
            clip.SetData(d, 0);
            return clip;
        }
    }
}
