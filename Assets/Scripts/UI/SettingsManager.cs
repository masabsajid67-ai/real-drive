using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Utils;
namespace RealDrive.UI
{
    public class SettingsManager : MonoBehaviour
    {
        public void Build(Transform parent, System.Action onClose)
        {
            Text sLbl = RuntimeUIHelper.CreateText(parent, "SOUND VOLUME", 20, Color.white, TextAnchor.MiddleLeft);
            RuntimeUIHelper.SetRect(sLbl.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-180, -130), new Vector2(300, 35));
            Slider s = RuntimeUIHelper.CreateSlider(parent, new Vector2(120, -130), new Vector2(320, 30), SaveManager.SoundVolume);
            s.onValueChanged.AddListener(v => { SaveManager.SoundVolume = v; AudioManager.Instance?.ApplyVolumes(); });
            Text mLbl = RuntimeUIHelper.CreateText(parent, "MUSIC VOLUME", 20, Color.white, TextAnchor.MiddleLeft);
            RuntimeUIHelper.SetRect(mLbl.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-180, -190), new Vector2(300, 35));
            Slider m = RuntimeUIHelper.CreateSlider(parent, new Vector2(120, -190), new Vector2(320, 30), SaveManager.MusicVolume);
            m.onValueChanged.AddListener(v => { SaveManager.MusicVolume = v; AudioManager.Instance?.ApplyVolumes(); });
            Text qLbl = RuntimeUIHelper.CreateText(parent, "GRAPHICS QUALITY", 20, Color.white, TextAnchor.MiddleLeft);
            RuntimeUIHelper.SetRect(qLbl.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-180, -250), new Vector2(300, 35));
            string[] qNames = { "LOW", "MEDIUM", "HIGH" };
            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                Color col = SaveManager.QualityLevel == i ? new Color(0.95f, 0.6f, 0f) : new Color(0.2f, 0.22f, 0.3f);
                Button b = RuntimeUIHelper.CreateButton(parent, qNames[i], new Vector2(-120 + i * 170, -250), new Vector2(150, 48), col, () => { SaveManager.QualityLevel = idx; SaveManager.ApplyQuality(); });
            }
            Text cLbl = RuntimeUIHelper.CreateText(parent, "CONTROLS: 0 = Buttons  •  1 = Wheel (Stage 2 me active)", 18, new Color(1, 1, 1, 0.7f));
            RuntimeUIHelper.SetRect(cLbl.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -40), new Vector2(800, 35));
            RuntimeUIHelper.CreateButton(parent, "CLOSE", new Vector2(0, -210), new Vector2(260, 60), new Color(0.95f, 0.6f, 0f), () => onClose?.Invoke());
            Text note = RuntimeUIHelper.CreateText(parent, "Settings PlayerPrefs me save hoti hain.", 16, new Color(1, 1, 1, 0.55f));
            RuntimeUIHelper.SetRect(note.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 35), new Vector2(800, 30));
        }
    }
}
