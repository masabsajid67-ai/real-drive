using UnityEngine;
using RealDrive.Core;
using RealDrive.Garage;
using RealDrive.Utils;
namespace RealDrive.Vehicle
{
    // Build 13 realism: asli sports-car shape — sloped hood, windshield, roof,
    // bumpers, grille, mirrors, exhaust, taillight strip, tyre + alag rim.
    // Physics (Rigidbody / WheelCollider / CarController) bilkul waisi hi hai.
    public static class DrivableCarBuilder
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

        public static CarController BuildDrivableCar(Vector3 pos, Color bodyColor, float maxSpeedKmh, float accelStat)
        {
            GameObject car = new GameObject("PlayerCar");
            car.transform.position = pos;
            car.tag = "Player";
            Material bm = new Material(TextureHelper.LitShader());
            hasShader = bm.shader != null;
            bodyMat = darkMat = glassMat = tyreMat = rimMat = null;
            if (hasShader)
            {
                bodyMat = bm;
                bodyMat.color = bodyColor;
                Customization.ApplyFinish(bodyMat, SaveManager.PaintFinish);
                darkMat = new Material(bodyMat.shader); darkMat.color = new Color(0.08f, 0.08f, 0.1f);
                glassMat = new Material(bodyMat.shader); glassMat.color = new Color(0.08f, 0.14f, 0.2f); glassMat.SetFloat("_Smoothness", 1f); glassMat.SetFloat("_Metallic", 0.9f);
                tyreMat = new Material(bodyMat.shader); tyreMat.color = Color.black; tyreMat.SetFloat("_Smoothness", 0.9f);
                rimMat = new Material(bodyMat.shader); rimMat.color = Customization.RimColor(SaveManager.RimStyle); rimMat.SetFloat("_Metallic", 1f); rimMat.SetFloat("_Smoothness", 0.9f);
            }

            // Main body - neeche chauda, upar halka taper jaisa
            Part(car, "Body", PrimitiveType.Cube, new Vector3(0, 0.55f, 0), new Vector3(1.9f, 0.45f, 4.4f), bodyMat);
            // Hood - aage ki taraf dhala hua
            Part(car, "Hood", PrimitiveType.Cube, new Vector3(0, 0.74f, 1.55f), new Vector3(1.82f, 0.16f, 1.15f), bodyMat, new Vector3(-10, 0, 0));
            // Front / rear bumper
            Part(car, "FrontBumper", PrimitiveType.Cube, new Vector3(0, 0.34f, 2.18f), new Vector3(1.92f, 0.26f, 0.28f), darkMat);
            Part(car, "RearBumper", PrimitiveType.Cube, new Vector3(0, 0.34f, -2.18f), new Vector3(1.92f, 0.26f, 0.28f), darkMat);
            // Side skirts
            Part(car, "SkirtL", PrimitiveType.Cube, new Vector3(-0.96f, 0.32f, 0), new Vector3(0.1f, 0.18f, 3.1f), darkMat);
            Part(car, "SkirtR", PrimitiveType.Cube, new Vector3(0.96f, 0.32f, 0), new Vector3(0.1f, 0.18f, 3.1f), darkMat);
            // Cabin (glass) + roof panel + windshield + rear window
            Part(car, "Cabin", PrimitiveType.Cube, new Vector3(0, 0.95f, -0.3f), new Vector3(1.6f, 0.45f, 2.2f), glassMat);
            Part(car, "Roof", PrimitiveType.Cube, new Vector3(0, 1.2f, -0.35f), new Vector3(1.55f, 0.08f, 1.9f), bodyMat);
            Part(car, "Windshield", PrimitiveType.Cube, new Vector3(0, 0.98f, 0.82f), new Vector3(1.5f, 0.07f, 1.0f), glassMat, new Vector3(-28, 0, 0));
            Part(car, "RearWindow", PrimitiveType.Cube, new Vector3(0, 0.98f, -1.32f), new Vector3(1.5f, 0.07f, 0.9f), glassMat, new Vector3(24, 0, 0));
            // Grille
            Part(car, "Grille", PrimitiveType.Cube, new Vector3(0, 0.5f, 2.21f), new Vector3(1.2f, 0.18f, 0.06f), darkMat);
            // Spoiler + supports (style ke hisaab se)
            GameObject spoiler = null;
            if (SaveManager.SpoilerStyle != 0)
            {
                spoiler = Part(car, "Spoiler", PrimitiveType.Cube, new Vector3(0, 1.05f, -2.1f), new Vector3(1.8f, 0.08f, 0.5f), darkMat);
                if (SaveManager.SpoilerStyle == 1) spoiler.transform.localScale = new Vector3(1.4f, 0.08f, 0.4f);
                else if (SaveManager.SpoilerStyle == 2) spoiler.transform.localScale = new Vector3(1.8f, 0.08f, 0.5f);
                else spoiler.transform.localScale = new Vector3(2.1f, 0.1f, 0.7f);
                Part(car, "SpoilerStrutL", PrimitiveType.Cube, new Vector3(-0.6f, 0.92f, -2.1f), new Vector3(0.08f, 0.22f, 0.08f), darkMat);
                Part(car, "SpoilerStrutR", PrimitiveType.Cube, new Vector3(0.6f, 0.92f, -2.1f), new Vector3(0.08f, 0.22f, 0.08f), darkMat);
            }
            // Side mirrors
            Part(car, "MirrorL", PrimitiveType.Cube, new Vector3(-1.02f, 0.98f, 0.85f), new Vector3(0.14f, 0.2f, 0.24f), darkMat);
            Part(car, "MirrorR", PrimitiveType.Cube, new Vector3(1.02f, 0.98f, 0.85f), new Vector3(0.14f, 0.2f, 0.24f), darkMat);
            // Headlights (emissive) + real spot lights
            foreach (float x in new float[] { -0.6f, 0.6f })
            {
                GameObject h = Part(car, "Headlight", PrimitiveType.Cube, new Vector3(x, 0.62f, 2.21f), new Vector3(0.5f, 0.14f, 0.06f), null);
                if (hasShader)
                {
                    Material lm = TextureHelper.Glow(Color.white, Color.white, 1.5f);
                    if (lm != null) h.GetComponent<Renderer>().material = lm;
                }
                GameObject hl = new GameObject("HeadlightLight");
                hl.transform.SetParent(car.transform, false);
                hl.transform.localPosition = new Vector3(x, 0.6f, 2.2f);
                Light l = hl.AddComponent<Light>(); l.type = LightType.Spot; l.spotAngle = 55f; l.range = 30f; l.intensity = 2f; l.enabled = false;
            }
            // Taillight strip (emissive red, poori chaudi)
            GameObject tl = Part(car, "Taillight", PrimitiveType.Cube, new Vector3(0, 0.66f, -2.21f), new Vector3(1.7f, 0.13f, 0.06f), null);
            if (hasShader)
            {
                Material tm = TextureHelper.Glow(Color.red, Color.red, 1.4f);
                if (tm != null) tl.GetComponent<Renderer>().material = tm;
            }
            // Exhaust pipes
            foreach (float x in new float[] { -0.5f, 0.5f })
                Part(car, "Exhaust", PrimitiveType.Cylinder, new Vector3(x, 0.28f, -2.28f), new Vector3(0.13f, 0.3f, 0.13f), darkMat, new Vector3(90, 0, 0));

            // Rigidbody + main collider
            Rigidbody rb = car.AddComponent<Rigidbody>();
            rb.mass = 1400f;
            BoxCollider bc = car.AddComponent<BoxCollider>();
            bc.center = new Vector3(0, 0.6f, 0); bc.size = new Vector3(1.9f, 0.9f, 4.4f);
            // Wheels: visual + collider
            CarController ctrl = car.AddComponent<CarController>();
            ctrl.maxSpeedKmh = maxSpeedKmh;
            ctrl.motorForce = 1400f + accelStat * 180f;
            Vector3[] positions = {
                new Vector3(-0.95f, 0.35f, 1.45f), new Vector3(0.95f, 0.35f, 1.45f),
                new Vector3(-0.95f, 0.35f, -1.45f), new Vector3(0.95f, 0.35f, -1.45f)
            };
            WheelCollider[] cols = new WheelCollider[4];
            Transform[] visuals = new Transform[4];
            for (int i = 0; i < 4; i++)
            {
                GameObject wcGo = new GameObject("WheelCollider_" + i);
                wcGo.transform.SetParent(car.transform, false);
                wcGo.transform.localPosition = positions[i];
                WheelCollider wc = wcGo.AddComponent<WheelCollider>();
                wc.radius = 0.35f; wc.suspensionDistance = 0.25f;
                JointSpring spring = wc.suspensionSpring;
                spring.spring = 28000f; spring.damper = 3500f; spring.targetPosition = 0.5f;
                wc.suspensionSpring = spring;
                wc.mass = 20f;
                WheelFrictionCurve fwd = wc.forwardFriction; fwd.extremumSlip = 0.4f; fwd.extremumValue = 1f; fwd.asymptoteSlip = 0.8f; fwd.asymptoteValue = 0.5f; fwd.stiffness = 1.5f; wc.forwardFriction = fwd;
                WheelFrictionCurve side = wc.sidewaysFriction; side.extremumSlip = 0.25f; side.extremumValue = 1f; side.asymptoteSlip = 0.5f; side.asymptoteValue = 0.75f; side.stiffness = 1.8f; wc.sidewaysFriction = side;
                cols[i] = wc;
                // Tyre + alag rim (rim tyre ke andar ghumta hai)
                GameObject wv = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wv.name = "WheelVisual_" + i;
                wv.transform.SetParent(car.transform, false);
                wv.transform.localPosition = positions[i];
                wv.transform.localRotation = Quaternion.Euler(0, 0, 90);
                wv.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                if (tyreMat != null) wv.GetComponent<Renderer>().material = tyreMat;
                GameObject rim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                rim.name = "Rim_" + i;
                rim.transform.SetParent(wv.transform, false);
                rim.transform.localPosition = Vector3.zero;
                rim.transform.localScale = new Vector3(0.55f, 1.12f, 0.55f);
                if (rimMat != null) rim.GetComponent<Renderer>().material = rimMat;
                visuals[i] = wv.transform;
            }
            ctrl.frontLeft = cols[0]; ctrl.frontRight = cols[1]; ctrl.rearLeft = cols[2]; ctrl.rearRight = cols[3];
            ctrl.frontLeftVisual = visuals[0]; ctrl.frontRightVisual = visuals[1]; ctrl.rearLeftVisual = visuals[2]; ctrl.rearRightVisual = visuals[3];
            car.AddComponent<EngineAudio>();
            return ctrl;
        }
    }
}
