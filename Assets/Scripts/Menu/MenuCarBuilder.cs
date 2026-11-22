using UnityEngine;
using RealDrive.Core;
using RealDrive.Garage;
namespace RealDrive.Menu
{
    public static class MenuCarBuilder
    {
        public static GameObject BuildSportsCar(Vector3 pos, Color bodyColor)
        {
            GameObject car = new GameObject("ShowroomCar");
            car.transform.position = pos;
            Material bodyMat = new Material(Shader.Find("Standard"));
            if (bodyMat.shader == null) bodyMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if (bodyMat.shader == null) bodyMat = new Material(Shader.Find("Unlit/Color"));
            bodyMat.color = bodyColor; bodyMat.SetFloat("_Smoothness", 0.85f); bodyMat.SetFloat("_Metallic", 0.6f);
            Material darkMat = new Material(bodyMat.shader); darkMat.color = new Color(0.08f, 0.08f, 0.1f);
            Material glassMat = new Material(bodyMat.shader); glassMat.color = new Color(0.1f, 0.2f, 0.3f, 0.9f); glassMat.SetFloat("_Smoothness", 1f); glassMat.SetFloat("_Metallic", 0.9f);
            Material tyreMat = new Material(bodyMat.shader); tyreMat.color = Color.black;
            Material rimMat = new Material(bodyMat.shader); rimMat.color = new Color(0.85f, 0.85f, 0.9f); rimMat.SetFloat("_Metallic", 1f); rimMat.SetFloat("_Smoothness", 0.9f);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body"; body.transform.SetParent(car.transform, false);
            body.transform.localPosition = new Vector3(0, 0.55f, 0); body.transform.localScale = new Vector3(1.9f, 0.45f, 4.4f);
            body.GetComponent<Renderer>().material = bodyMat;
            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.name = "Cabin"; cabin.transform.SetParent(car.transform, false);
            cabin.transform.localPosition = new Vector3(0, 0.95f, -0.3f); cabin.transform.localScale = new Vector3(1.6f, 0.45f, 2.2f);
            cabin.GetComponent<Renderer>().material = glassMat;
            GameObject spoiler = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spoiler.name = "Spoiler"; spoiler.transform.SetParent(car.transform, false);
            spoiler.transform.localPosition = new Vector3(0, 1.05f, -2.1f); spoiler.transform.localScale = new Vector3(1.8f, 0.08f, 0.5f);
            spoiler.GetComponent<Renderer>().material = darkMat;
            // wheels
            Vector3[] wheelPos = { new Vector3(-0.95f, 0.35f, 1.45f), new Vector3(0.95f, 0.35f, 1.45f), new Vector3(-0.95f, 0.35f, -1.45f), new Vector3(0.95f, 0.35f, -1.45f) };
            foreach (var wp in wheelPos)
            {
                GameObject w = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                w.name = "Wheel"; w.transform.SetParent(car.transform, false);
                w.transform.localPosition = wp; w.transform.localRotation = Quaternion.Euler(0, 0, 90); w.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                w.GetComponent<Renderer>().material = tyreMat;
            }
            // headlights - two small emissive cubes
            foreach (float x in new float[]{-0.6f, 0.6f})
            {
                GameObject h = GameObject.CreatePrimitive(PrimitiveType.Cube);
                h.name = "Headlight"; h.transform.SetParent(car.transform, false);
                h.transform.localPosition = new Vector3(x, 0.6f, 2.21f); h.transform.localScale = new Vector3(0.45f, 0.12f, 0.05f);
                Material lm = new Material(bodyMat.shader); lm.color = Color.white; lm.EnableKeyword("_EMISSION"); lm.SetColor("_EmissionColor", Color.white * 1.5f);
                h.GetComponent<Renderer>().material = lm;
            }
            // ground disc / showroom platform
            GameObject plat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            plat.name = "Platform"; plat.transform.SetParent(car.transform, false);
            plat.transform.localPosition = new Vector3(0, -0.05f, 0); plat.transform.localScale = new Vector3(4.5f, 0.1f, 4.5f);
            plat.GetComponent<Renderer>().material = darkMat;
            return car;
        }
        public static GameObject BuildSportsCarCustom(Vector3 pos, Color bodyColor)
        {
            GameObject car = BuildSportsCar(pos, bodyColor);
            // Paint finish
            foreach (var r in car.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.name == "Body") Customization.ApplyFinish(r.material, SaveManager.PaintFinish);
            }
            // Spoiler style: 0 none, 1 small, 2 large, 3 GT wing
            Transform spoiler = car.transform.Find("Spoiler");
            if (spoiler != null)
            {
                int s = SaveManager.SpoilerStyle;
                if (s == 0) spoiler.gameObject.SetActive(false);
                else if (s == 1) spoiler.localScale = new Vector3(1.4f, 0.08f, 0.4f);
                else if (s == 2) spoiler.localScale = new Vector3(1.8f, 0.08f, 0.5f);
                else spoiler.localScale = new Vector3(2.1f, 0.1f, 0.7f);
            }
            // Rims: wheel colour
            foreach (var w in car.GetComponentsInChildren<Renderer>())
            {
                if (w.gameObject.name == "Wheel") w.material.color = Customization.RimColor(SaveManager.RimStyle);
            }
            return car;
        }
        public static void BuildShowroomFloor(Vector3 pos)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "ShowroomFloor"; floor.transform.position = pos; floor.transform.localScale = new Vector3(4, 1, 4);
            var r = floor.GetComponent<Renderer>();
            Shader s = Shader.Find("Standard");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
            if (s == null) s = Shader.Find("Unlit/Color");
            if (s == null) s = Shader.Find("Unlit/Color");
            if (s != null) { Material m = new Material(s); m.color = new Color(0.07f, 0.08f, 0.1f); r.material = m; }
        }
    }
}
