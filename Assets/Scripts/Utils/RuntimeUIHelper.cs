using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RealDrive.Utils
{
    public static class RuntimeUIHelper
    {
        public static void EnsureEventSystem()
        {
            if (Object.FindObjectOfType<EventSystem>() != null) return;
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
        public static Canvas CreateCanvas(string name, int sortOrder = 10)
        {
            EnsureEventSystem();
            GameObject go = new GameObject(name);
            Canvas c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.sortingOrder = sortOrder;
            CanvasScaler s = go.AddComponent<CanvasScaler>();
            s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            s.referenceResolution = new Vector2(1920, 1080);
            s.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return c;
        }
        public static Text CreateText(Transform parent, string txt, int size, Color col, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject go = new GameObject("Text_" + txt.Substring(0, Mathf.Min(12, txt.Length)));
            go.transform.SetParent(parent, false);
            Text t = go.AddComponent<Text>();
            t.text = txt; t.fontSize = size; t.color = col; t.alignment = anchor;
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            RectTransform r = go.GetComponent<RectTransform>();
            r.anchorMin = new Vector2(0.5f, 0.5f); r.anchorMax = new Vector2(0.5f, 0.5f);
            r.sizeDelta = new Vector2(800, 80);
            return t;
        }
        public static Button CreateButton(Transform parent, string label, Vector2 anchoredPos, Vector2 size, Color bg, System.Action onClick)
        {
            GameObject go = new GameObject("Btn_" + label);
            go.transform.SetParent(parent, false);
            RectTransform r = go.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(0.5f, 0.5f); r.anchorMax = new Vector2(0.5f, 0.5f);
            r.anchoredPosition = anchoredPos; r.sizeDelta = size;
            Image img = go.AddComponent<Image>();
            img.color = bg;
            Button b = go.AddComponent<Button>();
            b.targetGraphic = img;
            ColorBlock cb = b.colors; cb.highlightedColor = new Color(1f, 0.8f, 0f); cb.pressedColor = new Color(0.9f, 0.6f, 0f); b.colors = cb;
            GameObject tg = new GameObject("Label");
            tg.transform.SetParent(go.transform, false);
            RectTransform tr = tg.AddComponent<RectTransform>();
            tr.anchorMin = Vector2.zero; tr.anchorMax = Vector2.one; tr.offsetMin = Vector2.zero; tr.offsetMax = Vector2.zero;
            Text t = tg.AddComponent<Text>();
            t.text = label; t.fontSize = 28; t.color = Color.white; t.alignment = TextAnchor.MiddleCenter;
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.resizeTextForBestFit = true; t.resizeTextMinSize = 18; t.resizeTextMaxSize = 32;
            if (onClick != null) b.onClick.AddListener(() => { RealDrive.Core.AudioManager.Instance?.PlayClick(); onClick(); });
            return b;
        }
        public static GameObject CreatePanel(Transform parent, Vector2 pos, Vector2 size, Color bg)
        {
            GameObject go = new GameObject("Panel");
            go.transform.SetParent(parent, false);
            RectTransform r = go.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(0.5f, 0.5f); r.anchorMax = new Vector2(0.5f, 0.5f);
            r.anchoredPosition = pos; r.sizeDelta = size;
            Image img = go.AddComponent<Image>(); img.color = bg;
            return go;
        }
        public static Slider CreateSlider(Transform parent, Vector2 pos, Vector2 size, float val)
        {
            GameObject go = new GameObject("Slider");
            go.transform.SetParent(parent, false);
            RectTransform r = go.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(0.5f, 0.5f); r.anchorMax = new Vector2(0.5f, 0.5f);
            r.anchoredPosition = pos; r.sizeDelta = size;
            Slider s = go.AddComponent<Slider>();
            GameObject bg = new GameObject("Background"); bg.transform.SetParent(go.transform, false);
            RectTransform br = bg.AddComponent<RectTransform>(); br.anchorMin = Vector2.zero; br.anchorMax = Vector2.one; br.offsetMin = Vector2.zero; br.offsetMax = Vector2.zero;
            Image bi = bg.AddComponent<Image>(); bi.color = new Color(0.2f, 0.2f, 0.25f, 1f);
            GameObject fillArea = new GameObject("Fill Area"); fillArea.transform.SetParent(go.transform, false);
            RectTransform fr = fillArea.AddComponent<RectTransform>(); fr.anchorMin = Vector2.zero; fr.anchorMax = Vector2.one; fr.offsetMin = Vector2.zero; fr.offsetMax = Vector2.zero;
            GameObject fill = new GameObject("Fill"); fill.transform.SetParent(fillArea.transform, false);
            RectTransform fir = fill.AddComponent<RectTransform>(); fir.anchorMin = new Vector2(0, 0.25f); fir.anchorMax = new Vector2(1, 0.75f); fir.offsetMin = Vector2.zero; fir.offsetMax = Vector2.zero;
            Image fi = fill.AddComponent<Image>(); fi.color = new Color(1f, 0.75f, 0f);
            GameObject handleArea = new GameObject("Handle Slide Area"); handleArea.transform.SetParent(go.transform, false);
            RectTransform hr = handleArea.AddComponent<RectTransform>(); hr.anchorMin = Vector2.zero; hr.anchorMax = Vector2.one; hr.offsetMin = new Vector2(10, 0); hr.offsetMax = new Vector2(-10, 0);
            GameObject handle = new GameObject("Handle"); handle.transform.SetParent(handleArea.transform, false);
            RectTransform har = handle.AddComponent<RectTransform>(); har.sizeDelta = new Vector2(30, 30);
            Image hi = handle.AddComponent<Image>(); hi.color = Color.white;
            s.targetGraphic = hi; s.fillRect = fir; s.handleRect = har;
            s.minValue = 0f; s.maxValue = 1f; s.value = val;
            return s;
        }
        public static void SetRect(RectTransform r, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size)
        {
            r.anchorMin = anchorMin; r.anchorMax = anchorMax; r.anchoredPosition = pos; r.sizeDelta = size;
        }
    }
}
