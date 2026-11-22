using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Utils;
namespace RealDrive.UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        Slider bar; Text pct; Text tip;
        string[] tips = {
            "Tip: Handbrake se sharp turn lo.",
            "Tip: Garage me car ka colour badlo.",
            "Tip: 320 KM/H tak ja sakte ho, sambhal ke.",
            "Tip: Settings me quality Low / Medium / High karo.",
            "Loading: Open World tayyar ho raha hai..."
        };
        void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Canvas c = RuntimeUIHelper.CreateCanvas("LoadingCanvas", 10);
            Transform root = c.transform;
            Text logo = RuntimeUIHelper.CreateText(root, "REAL DRIVE", 56, new Color(1f, 0.8f, 0f));
            RuntimeUIHelper.SetRect(logo.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -120), new Vector2(800, 80));
            Text loadTxt = RuntimeUIHelper.CreateText(root, "LOADING...", 28, Color.white);
            RuntimeUIHelper.SetRect(loadTxt.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 80), new Vector2(600, 50));
            bar = RuntimeUIHelper.CreateSlider(root, new Vector2(0, 0), new Vector2(900, 36), 0f);
            pct = RuntimeUIHelper.CreateText(root, "0%", 26, Color.white);
            RuntimeUIHelper.SetRect(pct.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -50), new Vector2(200, 40));
            tip = RuntimeUIHelper.CreateText(root, tips[0], 20, new Color(1, 1, 1, 0.75f));
            RuntimeUIHelper.SetRect(tip.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 70), new Vector2(1000, 40));
            StartCoroutine(RunLoad());
        }
        IEnumerator RunLoad()
        {
            if (SceneLoader.Instance == null)
            {
                GameObject sl = new GameObject("SceneLoader"); sl.AddComponent<SceneLoader>();
                yield return null;
            }
            int ti = 0;
            Coroutine load = StartCoroutine(SceneLoader.Instance.LoadTargetAsync());
            while (SceneLoader.IsLoading)
            {
                if (bar != null) bar.value = SceneLoader.LoadProgress;
                if (pct != null) pct.text = Mathf.RoundToInt(SceneLoader.LoadProgress * 100f) + "%";
                if (tip != null && Time.frameCount % 180 == 0) { ti = (ti + 1) % tips.Length; tip.text = tips[ti]; }
                yield return null;
            }
            yield return load;
        }
    }
}
