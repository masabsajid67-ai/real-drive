using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace RealDrive.Utils
{
    // Menu buttons ka backup tap handler.
    // Masla: Build 13 mein menu dikhne laga (title, buttons, gari sab theek)
    // lekin koi button dabta hi nahi tha — StandaloneInputModule phone par
    // touch pakad nahi raha tha. Ye handler khud har tap ko pakadta hai:
    // touch ya mouse down par canvas par raycast karke jo Button mile
    // uska onClick chalata hai. StandaloneInputModule bhi rehta hai;
    // double-tap se koi nuksan nahi kyunke saare button actions
    // idempotent hain (PLAY/GARAGE mein IsLoading guard hai, panel
    // khulna dobara khulna ek hi baat hai).
    public class TouchButtonHandler : MonoBehaviour
    {
        GraphicRaycaster raycaster;
        EventSystem es;

        void Awake()
        {
            raycaster = GetComponent<GraphicRaycaster>();
            if (raycaster == null) raycaster = gameObject.AddComponent<GraphicRaycaster>();
            es = FindObjectOfType<EventSystem>();
        }

        void Update()
        {
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
            if (es == null) es = FindObjectOfType<EventSystem>();
            if (es == null || raycaster == null) return;

            PointerEventData ped = new PointerEventData(es);
            ped.position = pos;
            List<RaycastResult> hits = new List<RaycastResult>();
            raycaster.Raycast(ped, hits);
            foreach (var h in hits)
            {
                Button b = h.gameObject.GetComponentInParent<Button>();
                if (b != null && b.interactable)
                {
                    b.onClick.Invoke();
                    return;
                }
            }
        }
    }
}
