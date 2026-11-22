using UnityEngine;
namespace RealDrive.Traffic
{
    public enum TrafficType { Sedan, SUV, Van, Sports, Truck, Bus }
    public static class TrafficCarBuilder
    {
        public static GameObject Build(TrafficType type, Vector3 pos, Quaternion rot, Color col)
        {
            GameObject car = new GameObject("Traffic_" + type);
            car.transform.position = pos;
            car.transform.rotation = rot;
            car.tag = "Traffic";
            Shader s = Shader.Find("Standard");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
            if (s == null) s = Shader.Find("Unlit/Color");
            Material body = new Material(s); body.color = col; body.SetFloat("_Smoothness", 0.7f); body.SetFloat("_Metallic", 0.4f);
            Material dark = new Material(s); dark.color = new Color(0.08f, 0.08f, 0.1f);
            Material glass = new Material(s); glass.color = new Color(0.15f, 0.25f, 0.35f);
            Material tyre = new Material(s); tyre.color = Color.black;
            float len = 4.4f, wid = 1.9f, hgt = 0.6f;
            if (type == TrafficType.Van) { len = 5.2f; hgt = 0.9f; }
            if (type == TrafficType.Truck) { len = 6.5f; wid = 2.2f; hgt = 1.0f; }
            if (type == TrafficType.Bus) { len = 8f; wid = 2.4f; hgt = 1.1f; }
            if (type == TrafficType.SUV) { hgt = 0.8f; }
            GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube);
            b.name = "Body"; b.transform.SetParent(car.transform, false);
            b.transform.localPosition = new Vector3(0, 0.6f, 0); b.transform.localScale = new Vector3(wid, hgt, len);
            b.GetComponent<Renderer>().material = body;
            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.name = "Cabin"; cabin.transform.SetParent(car.transform, false);
            cabin.transform.localPosition = new Vector3(0, 0.6f + hgt / 2f + 0.25f, type == TrafficType.Bus ? 0 : -0.3f);
            cabin.transform.localScale = new Vector3(wid * 0.85f, 0.5f, len * 0.45f);
            cabin.GetComponent<Renderer>().material = glass;
            // wheels - 4, bus/truck 6 visual (4 hi kaafi)
            Vector3[] wp = {
                new Vector3(-wid/2f, 0.35f, len/2f - 0.8f), new Vector3(wid/2f, 0.35f, len/2f - 0.8f),
                new Vector3(-wid/2f, 0.35f, -len/2f + 0.8f), new Vector3(wid/2f, 0.35f, -len/2f + 0.8f)
            };
            foreach (var p in wp)
            {
                GameObject w = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                w.name = "Wheel"; w.transform.SetParent(car.transform, false);
                w.transform.localPosition = p; w.transform.localRotation = Quaternion.Euler(0, 0, 90); w.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                w.GetComponent<Renderer>().material = tyre;
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
