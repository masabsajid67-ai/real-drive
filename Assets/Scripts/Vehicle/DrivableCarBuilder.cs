using UnityEngine;
using RealDrive.Core;
using RealDrive.Garage;
namespace RealDrive.Vehicle
{
    public static class DrivableCarBuilder
    {
        public static CarController BuildDrivableCar(Vector3 pos, Color bodyColor, float maxSpeedKmh, float accelStat)
        {
            GameObject car = new GameObject("PlayerCar");
            car.transform.position = pos;
            car.tag = "Player";
            // Body visual - MenuCarBuilder jaisa, lekin wheels alag se taake WheelCollider se sync hon
            Material bodyMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if (bodyMat.shader == null) bodyMat = new Material(Shader.Find("Standard"));
            bodyMat.color = bodyColor;
            Customization.ApplyFinish(bodyMat, SaveManager.PaintFinish);
            Material darkMat = new Material(bodyMat.shader); darkMat.color = new Color(0.08f, 0.08f, 0.1f);
            Material glassMat = new Material(bodyMat.shader); glassMat.color = new Color(0.1f, 0.2f, 0.3f); glassMat.SetFloat("_Smoothness", 1f); glassMat.SetFloat("_Metallic", 0.9f);
            Material tyreMat = new Material(bodyMat.shader); tyreMat.color = Color.black;

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
            spoiler.transform.localPosition = new Vector3(0, 1.05f, -2.1f);
            if (SaveManager.SpoilerStyle == 0) spoiler.SetActive(false);
            else if (SaveManager.SpoilerStyle == 1) spoiler.transform.localScale = new Vector3(1.4f, 0.08f, 0.4f);
            else if (SaveManager.SpoilerStyle == 2) spoiler.transform.localScale = new Vector3(1.8f, 0.08f, 0.5f);
            else spoiler.transform.localScale = new Vector3(2.1f, 0.1f, 0.7f);
            spoiler.GetComponent<Renderer>().material = darkMat;
            // headlights
            foreach (float x in new float[]{-0.6f, 0.6f})
            {
                GameObject h = GameObject.CreatePrimitive(PrimitiveType.Cube);
                h.name = "Headlight"; h.transform.SetParent(car.transform, false);
                h.transform.localPosition = new Vector3(x, 0.6f, 2.21f); h.transform.localScale = new Vector3(0.45f, 0.12f, 0.05f);
                Material lm = new Material(bodyMat.shader); lm.color = Color.white; lm.EnableKeyword("_EMISSION"); lm.SetColor("_EmissionColor", Color.white * 1.5f);
                h.GetComponent<Renderer>().material = lm;
                GameObject hl = new GameObject("HeadlightLight");
                hl.transform.SetParent(car.transform, false);
                hl.transform.localPosition = new Vector3(x, 0.6f, 2.2f);
                Light l = hl.AddComponent<Light>(); l.type = LightType.Spot; l.spotAngle = 55f; l.range = 30f; l.intensity = 2f; l.enabled = false;
            }
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
                GameObject wv = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wv.name = "WheelVisual_" + i;
                wv.transform.SetParent(car.transform, false);
                wv.transform.localPosition = positions[i];
                wv.transform.localRotation = Quaternion.Euler(0, 0, 90);
                wv.transform.localScale = new Vector3(0.7f, 0.25f, 0.7f);
                Material wm = new Material(tyreMat); wm.color = Customization.RimColor(SaveManager.RimStyle);
                // Tyre black hi rakho, rim colour ko wheel ke centre me dikhane ke liye material ko halka mix karo
                wv.GetComponent<Renderer>().material = wm;
                visuals[i] = wv.transform;
            }
            ctrl.frontLeft = cols[0]; ctrl.frontRight = cols[1]; ctrl.rearLeft = cols[2]; ctrl.rearRight = cols[3];
            ctrl.frontLeftVisual = visuals[0]; ctrl.frontRightVisual = visuals[1]; ctrl.rearLeftVisual = visuals[2]; ctrl.rearRightVisual = visuals[3];
            car.AddComponent<EngineAudio>();
            return ctrl;
        }
    }
}
