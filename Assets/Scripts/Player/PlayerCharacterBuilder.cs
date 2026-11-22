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
            Shader ps = Shader.Find("Universal Render Pipeline/Lit");
            if (ps == null) ps = Shader.Find("Standard");
            Material shirt = new Material(ps);
            shirt.color = new Color(0.1f, 0.6f, 0.9f);
            Material skin = new Material(shirt.shader); skin.color = new Color(0.85f, 0.65f, 0.5f);
            Material pants = new Material(shirt.shader); pants.color = new Color(0.15f, 0.15f, 0.2f);
            // body
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body"; body.transform.SetParent(p.transform, false);
            body.transform.localPosition = new Vector3(0, 1.05f, 0); body.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            body.GetComponent<Renderer>().material = shirt;
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head"; head.transform.SetParent(p.transform, false);
            head.transform.localPosition = new Vector3(0, 1.85f, 0); head.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            head.GetComponent<Renderer>().material = skin;
            GameObject legs = GameObject.CreatePrimitive(PrimitiveType.Cube);
            legs.name = "Legs"; legs.transform.SetParent(p.transform, false);
            legs.transform.localPosition = new Vector3(0, 0.45f, 0); legs.transform.localScale = new Vector3(0.6f, 0.9f, 0.4f);
            legs.GetComponent<Renderer>().material = pants;
            CharacterController cc = p.AddComponent<CharacterController>();
            cc.height = 1.8f; cc.radius = 0.4f; cc.center = new Vector3(0, 0.9f, 0);
            p.AddComponent<PlayerOnFootController>();
            return p;
        }
    }
}
