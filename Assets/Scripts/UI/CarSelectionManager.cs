using UnityEngine;
using UnityEngine.UI;
using RealDrive.Core;
using RealDrive.Data;
using RealDrive.Utils;
namespace RealDrive.UI
{
    public class CarSelectionManager : MonoBehaviour
    {
        System.Action onDone;
        public void Build(Transform parent, System.Action onClose)
        {
            onDone = onClose;
            var cars = CarCatalog.All();
            for (int i = 0; i < cars.Count; i++)
            {
                int idx = i;
                var car = cars[i];
                float y = 110 - i * 62;
                // row panel
                GameObject row = RuntimeUIHelper.CreatePanel(parent, new Vector2(0, y), new Vector2(880, 52), new Color(0.12f, 0.13f, 0.18f));
                Text t = RuntimeUIHelper.CreateText(row.transform, car.displayName + "  •  " + car.category + "  •  " + car.topSpeedKmh + " KM/H  •  Acc " + car.acceleration.ToString("F1"), 20, Color.white, TextAnchor.MiddleLeft);
                RuntimeUIHelper.SetRect(t.rectTransform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(330, 0), new Vector2(620, 40));
                Color bc = SaveManager.SelectedCarIndex == idx ? new Color(0.95f, 0.6f, 0f) : new Color(0.25f, 0.27f, 0.35f);
                string lbl = SaveManager.SelectedCarIndex == idx ? "SELECTED" : "SELECT";
                RuntimeUIHelper.CreateButton(row.transform, lbl, new Vector2(340, 0), new Vector2(150, 40), bc, () => { SaveManager.SelectedCarIndex = idx; onDone?.Invoke(); });
            }
            RuntimeUIHelper.CreateButton(parent, "BACK", new Vector2(0, -240), new Vector2(260, 55), new Color(0.2f, 0.22f, 0.3f), () => onDone?.Invoke());
        }
    }
}
