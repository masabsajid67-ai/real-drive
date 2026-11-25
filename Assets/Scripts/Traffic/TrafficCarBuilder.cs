using UnityEngine;
using RealDrive.Utils;
namespace RealDrive.Traffic
{
    // Build 13 realism: har traffic type ki apni shape — windshield, bumpers,
    // mirrors, taillights, tyre + rim; truck ko cargo box, bus ko window strip.
    public enum TrafficType { Sedan, SUV, Van, Sports, Truck, Bus }
    public static class TrafficCarBuilder
    {
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

        public static GameObject Build(TrafficType type, Vector3 pos, Quaternion rot, Color col)
        {
            GameObject car = new GameObject("Traffic_" + type);
            car.transform.position = pos;
            car.transform.rotation = rot;
            car.tag = "Traffic";
            Shader s = TextureHelper.LitShader();
            // Build 11: shader null ho to koi null-shader material assign na ho (pink se bacho).
            Material body = null, dark = null, glass = null, tyre = null, rim = null;
            if (s != null)
            {
                body = new Material(s); body.color = col; body.SetFloat("_Smoothness", 0.7f); body.SetFloat("_Metallic", 0.4f);
                dark = new Material(s); dark.color = new Color(0.08f, 0.08f, 0.1f);
                glass = new Material(s); glass.color = new Color(0.1f, 0.16f, 0.24f); glass.SetFloat("_Smoothness", 1f); glass.SetFloat("_Metallic", 0.8f);
                tyre = new Material(s); tyre.color = Color.black;
                rim = new Material(s); rim.color = new Color(0.75f, 0.75f, 0.8f); rim.SetFloat("_Metallic", 1f); rim.SetFloat("_Smoothness", 0.9f);
            }
            float len = 4.4f, wid = 1.9f, hgt = 0.6f;
            if (type == TrafficType.Van) { len = 5.2f; hgt = 0.9f; }
            if (type == TrafficType.Truck) { len = 6.5f; wid = 2.2f; hgt = 1.0f; }
            if (type == TrafficType.Bus) { len = 8f; wid = 2.4f; hgt = 1.1f; }
            if (type == TrafficType.SUV) { hgt = 0.8f; }
            float front = len / 2f, rear = -len / 2f;

            Part(car, "Body", PrimitiveType.Cube, new Vector3(0, 0.6f, 0), new Vector3(wid, hgt, len), body);
            Part(car, "FrontBumper", PrimitiveType.Cube, new Vector3(0, 0.36f, front), new Vector3(wid + 0.02f, 0.24f, 0.24f), dark);
            Part(car, "RearBumper", PrimitiveType.Cube, new Vector3(0, 0.36f, rear), new Vector3(wid + 0.02f, 0.24f, 0.24f), dark);
            if (type == TrafficType.Truck)
            {
                // Cab + bada cargo box
                Part(car, "Cabin", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.3f, front - 0.9f), new Vector3(wid * 0.9f, 0.6f, 1.2f), glass);
                Part(car, "Cargo", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.35f, -0.9f), new Vector3(wid, hgt + 0.7f, 4.2f), body);
                Part(car, "CargoStripe", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt, -0.9f), new Vector3(wid + 0.02f, 0.3f, 4.22f), dark);
            }
            else if (type == TrafficType.Bus)
            {
                // Lambi bus: upar sheeshe ki patti (windows)
                Part(car, "WindowStrip", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.35f, 0), new Vector3(wid * 0.92f, 0.55f, len * 0.85f), glass);
                Part(car, "BusRoof", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.65f, 0), new Vector3(wid * 0.95f, 0.1f, len * 0.9f), body);
            }
            else
            {
                // Sedan / SUV / Van / Sports: cabin + windshield + roof
                float cabinLen = len * 0.45f;
                float cabinZ = type == TrafficType.Van ? 0.2f : -0.3f;
                Part(car, "Cabin", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.25f, cabinZ), new Vector3(wid * 0.85f, 0.5f, cabinLen), glass);
                Part(car, "Roof", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.52f, cabinZ), new Vector3(wid * 0.8f, 0.07f, cabinLen * 0.85f), body);
                Part(car, "Windshield", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.28f, cabinZ + cabinLen / 2f + 0.25f), new Vector3(wid * 0.8f, 0.06f, 0.7f), glass, new Vector3(-26, 0, 0));
                if (type == TrafficType.Sports)
                    Part(car, "Spoiler", PrimitiveType.Cube, new Vector3(0, 0.6f + hgt / 2f + 0.25f, rear + 0.15f), new Vector3(wid * 0.9f, 0.07f, 0.45f), dark);
            }
            // Grille + headlights + taillights
            Part(car, "Grille", PrimitiveType.Cube, new Vector3(0, 0.55f, front + 0.01f), new Vector3(wid * 0.6f, 0.16f, 0.06f), dark);
            foreach (float x in new float[] { -wid * 0.32f, wid * 0.32f })
            {
                GameObject h = Part(car, "Headlight", PrimitiveType.Cube, new Vector3(x, 0.62f, front + 0.01f), new Vector3(0.42f, 0.13f, 0.06f), null);
                if (s != null)
                {
                    Material lm = TextureHelper.Glow(Color.white, Color.white, 1.4f);
                    if (lm != null) h.GetComponent<Renderer>().material = lm;
                }
            }
            GameObject tl = Part(car, "Taillight", PrimitiveType.Cube, new Vector3(0, 0.64f, rear - 0.01f), new Vector3(wid * 0.85f, 0.12f, 0.06f), null);
            if (s != null)
            {
                Material tm = TextureHelper.Glow(Color.red, Color.red, 1.3f);
                if (tm != null) tl.GetComponent<Renderer>().material = tm;
            }
            // Side mirrors
            Part(car, "MirrorL", PrimitiveType.Cube, new Vector3(-wid / 2f - 0.06f, 0.6f + hgt / 2f + 0.15f, front - 1.1f), new Vector3(0.12f, 0.16f, 0.2f), dark);
            Part(car, "MirrorR", PrimitiveType.Cube, new Vector3(wid / 2f + 0.06f, 0.6f + hgt / 2f + 0.15f, front - 1.1f), new Vector3(0.12f, 0.16f, 0.2f), dark);
            // wheels - tyre + rim (bus/truck 6 visual, 4 hi kaafi)
            Vector3[] wp = {
                new Vector3(-wid/2f, 0.35f, len/2f - 0.8f), new Vector3(wid/2f, 0.35f, len/2f - 0.8f),
                new Vector3(-wid/2f, 0.35f, -len/2f + 0.8f), new Vector3(wid/2f, 0.35f, -len/2f + 0.8f)
            };
            foreach (var p in wp)
            {
                GameObject w = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                w.name = "Wheel"; w.transform.SetParent(car.transform, false);
                w.transform.localPosition = p; w.transform.localRotation = Quaternion.Euler(0, 0, 90); w.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                if (tyre != null) w.GetComponent<Renderer>().material = tyre;
                GameObject r = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                r.name = "Rim"; r.transform.SetParent(w.transform, false);
                r.transform.localPosition = Vector3.zero;
                r.transform.localScale = new Vector3(0.55f, 1.12f, 0.55f);
                if (rim != null) r.GetComponent<Renderer>().material = rim;
            }
            // Collider - player isse takra sakta hai
            BoxCollider bc = car.AddComponent<BoxCollider>();
            bc.center = new Vector3(0, 0.7f, 0); bc.size = new Vector3(wid + 0.2f, 1.4f, len + 0.2f);
            Rigidbody rb = car.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            return car;
        }
        public static float SpeedFor(TrafficType t)
        {
            switch (t)
            {
                case TrafficType.Sports: return 15f;
                case TrafficType.Sedan: return 12f;
                case TrafficType.SUV: return 11f;
                case TrafficType.Bus: return 9f;
                case TrafficType.Van: return 9f;
                case TrafficType.Truck: return 8f;
                default: return 11f;
            }
        }
        public static Color ColorFor(TrafficType t, int variant)
        {
            Color[] cols = { new Color(0.8f, 0.1f, 0.1f), new Color(0.1f, 0.3f, 0.8f), new Color(0.9f, 0.9f, 0.9f), new Color(0.15f, 0.15f, 0.18f), new Color(1f, 0.7f, 0.1f), new Color(0.1f, 0.6f, 0.3f) };
            return cols[variant % cols.Length];
        }
    }
}
