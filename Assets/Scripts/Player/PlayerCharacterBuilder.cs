using UnityEngine;
namespace RealDrive.Player
{
    public static class PlayerCharacterBuilder
    {
        public static GameObject BuildPlayer(Vector3 pos)
        {
            GameObject p = new GameObject("PlayerOnFoot");
            p.transform.position = pos;
            p.tag = "Player";
            Shader ps = Shader.Find("Standard");
            if (ps == null) ps = Shader.Find("Universal Render Pipeline/Lit"); if (ps == null) ps = Shader.Find("Unlit/Color");
            // Build 11: shader null ho to koi null-shader material assign na ho (pink se bacho).
            Material shirt = null, skin = null, pants = null;
            if (ps != null)
            {
                shirt = new Material(ps);
                shirt.color = new Color(0.1f, 0.6f, 0.9f);
                skin = new Material(shirt.shader); skin.color = new Color(0.85f, 0.65f, 0.5f);
                pants = new Material(shirt.shader); pants.color = new Color(0.15f, 0.15f, 0.2f);
            }
            // body
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body"; body.transform.SetParent(p.transform, false);
            body.transform.localPosition = new Vector3(0, 1.05f, 0); body.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            if (shirt != null) body.GetComponent<Renderer>().material = shirt;
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head"; head.transform.SetParent(p.transform, false);
            head.transform.localPosition = new Vector3(0, 1.85f, 0); head.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            if (skin != null) head.GetComponent<Renderer>().material = skin;
            // cap
            GameObject cap = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cap.name = "Cap"; cap.transform.SetParent(p.transform, false);
            cap.transform.localPosition = new Vector3(0, 2.05f, 0); cap.transform.localScale = new Vector3(0.55f, 0.18f, 0.55f);
            if (shirt != null) cap.GetComponent<Renderer>().material = shirt;
            GameObject brim = GameObject.CreatePrimitive(PrimitiveType.Cube);
            brim.name = "CapBrim"; brim.transform.SetParent(p.transform, false);
            brim.transform.localPosition = new Vector3(0, 2.0f, 0.32f); brim.transform.localScale = new Vector3(0.5f, 0.06f, 0.3f);
            if (shirt != null) brim.GetComponent<Renderer>().material = shirt;
            // arms
            foreach (float x in new float[] { -0.52f, 0.52f })
            {
                GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                arm.name = "Arm"; arm.transform.SetParent(p.transform, false);
                arm.transform.localPosition = new Vector3(x, 1.05f, 0); arm.transform.localScale = new Vector3(0.32f, 0.55f, 0.32f);
                if (shirt != null) arm.GetComponent<Renderer>().material = shirt;
            }
            GameObject legs = GameObject.CreatePrimitive(PrimitiveType.Cube);
            legs.name = "Legs"; legs.transform.SetParent(p.transform, false);
            legs.transform.localPosition = new Vector3(0, 0.45f, 0); legs.transform.localScale = new Vector3(0.6f, 0.9f, 0.4f);
            if (pants != null) legs.GetComponent<Renderer>().material = pants;
            // shoes
            foreach (float x in new float[] { -0.16f, 0.16f })
            {
                GameObject shoe = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shoe.name = "Shoe"; shoe.transform.SetParent(p.transform, false);
                shoe.transform.localPosition = new Vector3(x, 0.06f, 0.08f); shoe.transform.localScale = new Vector3(0.24f, 0.12f, 0.5f);
                if (pants != null) shoe.GetComponent<Renderer>().material = pants;
            }
            CharacterController cc = p.AddComponent<CharacterController>();
            cc.height = 1.8f; cc.radius = 0.4f; cc.center = new Vector3(0, 0.9f, 0);
            p.AddComponent<PlayerOnFootController>();
            return p;
        }
    }
}
