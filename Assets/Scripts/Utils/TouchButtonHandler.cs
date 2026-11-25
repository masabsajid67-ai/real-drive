using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace RealDrive.Utils
{
    // Menu buttons ka tap handler + on-screen test.
    // Build 14 mein bhi koi button nahi daba — is liye ye build screen par
    // khud batayega ke masla kahan hai:
    //  - Upar left corner mein "FRAME 12345" chalega: agar ye number
    //    badhta rahe to game chal rahi hai (freeze nahi hai).
    //  - Tap karne par "TAP x,y" likha aayega: agar ye aaye to touch
    //    phone se game tak pahunch raha hai.
    //  - Uske baad ya to button ka naam aayega (tap lag gaya) ya
    //    "KOI BUTTON NAHI" (tap button par nahi laga).
    // Tap dhoondne ke 3 tareeqe hain: GraphicRaycaster, EventSystem
    // RaycastAll, aur har Button par RectangleContainsScreenPoint.
    // Debug text khud tap nahi rokta (raycastTarget = false).
    public class TouchButtonHandler : MonoBehaviour
    {
        GraphicRaycaster raycaster;
        EventSystem es;
        Canvas canvas;
        Text debugText;
        int frame;
        string lastTap = "TAP: koi tap nahi";

        void Awake()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            if (raycaster == null) raycaster = gameObject.AddComponent<GraphicRaycaster>();
            es = FindObjectOfType<EventSystem>();

            GameObject go = new GameObject("DebugTapLabel");
            go.transform.SetParent(transform, false);
            RectTransform r = go.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(0, 1); r.anchorMax = new Vector2(0, 1);
            r.anchoredPosition = new Vector2(230, -30); r.sizeDelta = new Vector2(460, 60);
            debugText = go.AddComponent<Text>();
            debugText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            debugText.fontSize = 18;
            debugText.color = Color.yellow;
            debugText.alignment = TextAnchor.UpperLeft;
            debugText.horizontalOverflow = HorizontalWrapMode.Overflow;
            debugText.raycastTarget = false;
            debugText.text = "FRAME 0";
        }

        void Update()
        {
            frame++;
            if (frame % 30 == 0 && debugText != null)
                debugText.text = "FRAME " + frame + "\n" + lastTap;

            Vector2 pos;
            bool down = false;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                pos = Input.GetTouch(0).position;
                down = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                pos = Input.mousePosition;
                down = true;
            }
            else return;

            if (!down) return;
            lastTap = "TAP " + (int)pos.x + "," + (int)pos.y;
            if (debugText != null) debugText.text = "FRAME " + frame + "\n" + lastTap;

            Button hit = FindButtonByRaycast(pos);
            if (hit == null) hit = FindButtonByRect(pos);

            if (hit != null)
            {
                lastTap += " -> " + hit.gameObject.name;
                if (debugText != null) debugText.text = "FRAME " + frame + "\n" + lastTap;
                hit.onClick.Invoke();
            }
            else
            {
                lastTap += " -> KOI BUTTON NAHI";
                if (debugText != null) debugText.text = "FRAME " + frame + "\n" + lastTap;
            }
        }

        Button FindButtonByRaycast(Vector2 pos)
        {
            if (es == null) es = FindObjectOfType<EventSystem>();
            if (es == null || raycaster == null) return null;
            PointerEventData ped = new PointerEventData(es);
            ped.position = pos;
            List<RaycastResult> hits = new List<RaycastResult>();
            raycaster.Raycast(ped, hits);
            foreach (var h in hits)
            {
                Button b = h.gameObject.GetComponentInParent<Button>();
                if (b != null && b.interactable) return b;
            }
            // EventSystem ka apna raycast bhi try karo
            List<RaycastResult> hits2 = new List<RaycastResult>();
            es.RaycastAll(ped, hits2);
            foreach (var h in hits2)
            {
                Button b = h.gameObject.GetComponentInParent<Button>();
                if (b != null && b.interactable) return b;
            }
            return null;
        }

        Button FindButtonByRect(Vector2 pos)
        {
            Camera cam = null;
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                cam = canvas.worldCamera;
            Button[] all = FindObjectsOfType<Button>();
            foreach (Button b in all)
            {
                if (b == null || !b.interactable || !b.gameObject.activeInHierarchy) continue;
                RectTransform rt = b.GetComponent<RectTransform>();
                if (rt != null && RectTransformUtility.RectangleContainsScreenPoint(rt, pos, cam))
                    return b;
            }
            return null;
        }
    }
}
