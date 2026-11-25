using UnityEngine;
using RealDrive.Core;
using RealDrive.Garage;
using RealDrive.Utils;
namespace RealDrive.Menu
{
    // Build 13 realism: showroom car bhi player car jaisi detailed — hood,
    // windshield, roof, bumpers, mirrors, taillight strip, tyre + alag rim.
    public static class MenuCarBuilder
    {
        static Material bodyMat, darkMat, glassMat, tyreMat, rimMat;
        static bool hasShader;

        static GameObject Part(GameObject parent, string name, PrimitiveType type, Vector3 pos, Vector3 scale, Material mat)
        {
            return Part(parent, name, type, pos, scale, mat, Vector3.zero);
        }
        static GameObject Part(GameObject parent, string name, PrimitiveType type, Vector3 pos, Vector3 scale, Material mat, Vector3 euler)
        {
            GameObject go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = pos;
            go.transform.localScale = scale;
            if (euler != Vector3.zero) go.transform.localRotation = Quaternion.Euler(euler);
            if (mat != null) go.GetComponent<Renderer>().material = mat;
            return go;
        }
        static void InitMats(Color bodyColor)
        {
            Material bm = new Material(TextureHelper.LitShader());
            hasShader = bm.shader != null;
            bodyMat = darkMat = glassMat = tyreMat = rimMat = null;
            if (!hasShader) return;
            bodyMat = bm;
            bodyMat.color = bodyColor; bodyMat.SetFloat("_Smoothness", 0.85f); bodyMat.SetFloat("_Metallic", 0.6f);
            darkMat = new Material(bodyMat.shader); darkMat.color = new Color(0.08f, 0.08f, 0.1f);
            glassMat = new Material(bodyMat.shader); glassMat.color = new Color(0.08f, 0.14f, 0.2f, 0.9f); glassMat.SetFloat("_Smoothness", 1f); glassMat.SetFloat("_Metallic", 0.9f);
            tyreMat = new Material(bodyMat.shader); tyreMat.color = Color.black; tyreMat.SetFloat("_Smoothness", 0.9f);
            rimMat = new Material(bodyMat.shader); rimMat.color = new Color(0.85f, 0.85f, 0.9f); rimMat.SetFloat("_Metallic", 1f); rimMat.SetFloat("_Smoothness", 0.9f);
        }

        public static GameObject BuildSportsCar(Vector3 pos, Color bodyColor)
        {
            GameObject car = new GameObject("ShowroomCar");
            car.transform.position = pos;
            InitMats(bodyColor);

            Part(car, "Body", PrimitiveType.Cube, new Vector3(0, 0.55f, 0), new Vector3(1.9f, 0.45f, 4.4f), bodyMat);
            Part(car, "Hood", PrimitiveType.Cube, new Vector3(0, 0.74f, 1.55f), new Vector3(1.82f, 0.16f, 1.15f), bodyMat, new Vector3(-10, 0, 0));
            Part(car, "FrontBumper", PrimitiveType.Cube, new Vector3(0, 0.34f, 2.18f), new Vector3(1.92f, 0.26f, 0.28f), darkMat);
            Part(car, "RearBumper", PrimitiveType.Cube, new Vector3(0, 0.34f, -2.18f), new Vector3(1.92f, 0.26f, 0.28f), darkMat);
            Part(car, "SkirtL", PrimitiveType.Cube, new Vector3(-0.96f, 0.32f, 0), new Vector3(0.1f, 0.18f, 3.1f), darkMat);
            Part(car, "SkirtR", PrimitiveType.Cube, new Vector3(0.96f, 0.32f, 0), new Vector3(0.1f, 0.18f, 3.1f), darkMat);
            Part(car, "Cabin", PrimitiveType.Cube, new Vector3(0, 0.95f, -0.3f), new Vector3(1.6f, 0.45f, 2.2f), glassMat);
            Part(car, "Roof", PrimitiveType.Cube, new Vector3(0, 1.2f, -0.35f), new Vector3(1.55f, 0.08f, 1.9f), bodyMat);
            Part(car, "Windshield", PrimitiveType.Cube, new Vector3(0, 0.98f, 0.82f), new Vector3(1.5f, 0.07f, 1.0f), glassMat, new Vector3(-28, 0, 0));
            Part(car, "RearWindow", PrimitiveType.Cube, new Vector3(0, 0.98f, -1.32f), new Vector3(1.5f, 0.07f, 0.9f), glassMat, new Vector3(24, 0, 0));
            Part(car, "Grille", PrimitiveType.Cube, new Vector3(0, 0.5f, 2.21f), new Vector3(1.2f, 0.18f, 0.06f), darkMat);
            Part(car, "Spoiler", PrimitiveType.Cube, new Vector3(0, 1.05f, -2.1f), new Vector3(1.8f, 0.08f, 0.5f), darkMat);
            Part(car, "SpoilerStrutL", PrimitiveType.Cube, new Vector3(-0.6f, 0.92f, -2.1f), new Vector3(0.08f, 0.22f, 0.08f), darkMat);
            Part(car, "SpoilerStrutR", PrimitiveType.Cube, new Vector3(0.6f, 0.92f, -2.1f), new Vector3(0.08f, 0.22f, 0.08f), darkMat);
            Part(car, "MirrorL", PrimitiveType.Cube, new Vector3(-1.02f, 0.98f, 0.85f), new Vector3(0.14f, 0.2f, 0.24f), darkMat);
            Part(car, "MirrorR", PrimitiveType.Cube, new Vector3(1.02f, 0.98f, 0.85f), new Vector3(0.14f, 0.2f, 0.24f), darkMat);
            // headlights - two small emissive cubes
            foreach (float x in new float[] { -0.6f, 0.6f })
            {
                GameObject h = Part(car, "Headlight", PrimitiveType.Cube, new Vector3(x, 0.62f, 2.21f), new Vector3(0.5f, 0.14f, 0.06f), null);
                if (hasShader)
                {
                    Material lm = TextureHelper.Glow(Color.white, Color.white, 1.5f);
                    if (lm != null) h.GetComponent<Renderer>().material = lm;
                }
            }
            // taillight strip
            GameObject tl = Part(car, "Taillight", PrimitiveType.Cube, new Vector3(0, 0.66f, -2.21f), new Vector3(1.7f, 0.13f, 0.06f), null);
            if (hasShader)
            {
                Material tm = TextureHelper.Glow(Color.red, Color.red, 1.4f);
                if (tm != null) tl.GetComponent<Renderer>().material = tm;
            }
            foreach (float x in new float[] { -0.5f, 0.5f })
                Part(car, "Exhaust", PrimitiveType.Cylinder, new Vector3(x, 0.28f, -2.28f), new Vector3(0.13f, 0.3f, 0.13f), darkMat, new Vector3(90, 0, 0));
            // wheels - tyre + alag rim
            Vector3[] wheelPos = { new Vector3(-0.95f, 0.35f, 1.45f), new Vector3(0.95f, 0.35f, 1.45f), new Vector3(-0.95f, 0.35f, -1.45f), new Vector3(0.95f, 0.35f, -1.45f) };
            foreach (var wp in wheelPos)
            {
                GameObject w = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                w.name = "Wheel"; w.transform.SetParent(car.transform, false);
                w.transform.localPosition = wp; w.transform.localRotation = Quaternion.Euler(0, 0, 90); w.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                if (tyreMat != null) w.GetComponent<Renderer>().material = tyreMat;
                GameObject rim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                rim.name = "Rim"; rim.transform.SetParent(w.transform, false);
                rim.transform.localPosition = Vector3.zero;
                rim.transform.localScale = new Vector3(0.55f, 1.12f, 0.55f);
                if (rimMat != null) rim.GetComponent<Renderer>().material = rimMat;
            }
            // ground disc / showroom platform
            GameObject plat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            plat.name = "Platform"; plat.transform.SetParent(car.transform, false);
            plat.transform.localPosition = new Vector3(0, -0.05f, 0); plat.transform.localScale = new Vector3(4.5f, 0.1f, 4.5f);
            if (darkMat != null) plat.GetComponent<Renderer>().material = darkMat;
            return car;
        }
        public static GameObject BuildSportsCarCustom(Vector3 pos, Color bodyColor)
        {
            GameObject car = BuildSportsCar(pos, bodyColor);
            // Paint finish
            foreach (var r in car.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.name == "Body" && r.material != null) Customization.ApplyFinish(r.material, SaveManager.PaintFinish);
            }
            // Spoiler style: 0 none, 1 small, 2 large, 3 GT wing
            Transform spoiler = car.transform.Find("Spoiler");
            if (spoiler != null)
            {
                int s = SaveManager.SpoilerStyle;
                if (s == 0)
                {
                    spoiler.gameObject.SetActive(false);
                    Transform sl = car.transform.Find("SpoilerStrutL"); if (sl != null) sl.gameObject.SetActive(false);
                    Transform sr = car.transform.Find("SpoilerStrutR"); if (sr != null) sr.gameObject.SetActive(false);
                }
                else if (s == 1) spoiler.localScale = new Vector3(1.4f, 0.08f, 0.4f);
                else if (s == 2) spoiler.localScale = new Vector3(1.8f, 0.08f, 0.5f);
                else spoiler.localScale = new Vector3(2.1f, 0.1f, 0.7f);
            }
            // Rims: wheel colour
            foreach (var w in car.GetComponentsInChildren<Renderer>())
            {
                if (w.gameObject.name == "Rim" && w.material != null) w.material.color = Customization.RimColor(SaveManager.RimStyle);
            }
            return car;
        }
        public static void BuildShowroomFloor(Vector3 pos)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "ShowroomFloor"; floor.transform.position = pos; floor.transform.localScale = new Vector3(4, 1, 4);
            var r = floor.GetComponent<Renderer>();
            // Showroom floor: concrete texture, tiling 8x8
            Material m = TextureHelper.Mat(new Color(0.32f, 0.33f, 0.36f), 0f, 0.6f, "Textures/concrete", 8, 8);
            if (m != null) r.material = m;
        }
    }
}
