using UnityEngine;
using RealDrive.Core;
using RealDrive.Garage;
namespace RealDrive.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [Header("Wheels")]
        public WheelCollider frontLeft;
        public WheelCollider frontRight;
        public WheelCollider rearLeft;
        public WheelCollider rearRight;
        public Transform frontLeftVisual;
        public Transform frontRightVisual;
        public Transform rearLeftVisual;
        public Transform rearRightVisual;

        [Header("Engine - selected car se set hota hai")]
        public float maxSpeedKmh = 320f;
        public float motorForce = 2200f;
        public float brakeForce = 3500f;
        public float maxSteerAngle = 28f;
        public float nitroBoost = 1.6f;

        [Header("State - HUD padhta hai")]
        public float speedKmh;
        public int gear = 1;
        public float rpm;
        public bool isDrivingEnabled = true;
        public bool headlightsOn = false;

        Rigidbody rb;
        float steerInput;
        Light[] headlights;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = 1400f;
            rb.centerOfMass = new Vector3(0, -0.4f, 0);
            rb.drag = 0.05f;
            rb.angularDrag = 0.6f;
            headlights = GetComponentsInChildren<Light>();
            SetHeadlights(false);
            // Stage 5 upgrades apply karo
            maxSpeedKmh = maxSpeedKmh * Customization.EngineMultiplier(SaveManager.EngineLevel);
            motorForce = motorForce * Customization.EngineMultiplier(SaveManager.EngineLevel);
            brakeForce = brakeForce * Customization.BrakeMultiplier(SaveManager.BrakeLevel);
            maxSteerAngle = maxSteerAngle * Customization.HandlingMultiplier(SaveManager.HandlingLevel);
        }
        void FixedUpdate()
        {
            if (!isDrivingEnabled)
            {
                ApplyBrake(1f);
                UpdateWheels();
                return;
            }
            float steerKey = MobileInput.KeyboardSteer();
            steerInput = Mathf.Clamp(MobileInput.Steer + steerKey, -1f, 1f);
            bool accel = MobileInput.Accelerator || MobileInput.KeyboardAccel();
            bool brake = MobileInput.Brake || MobileInput.KeyboardBrake();
            bool hand = MobileInput.Handbrake || MobileInput.KeyboardHandbrake();
            bool nitro = MobileInput.Nitro;

            speedKmh = rb.velocity.magnitude * 3.6f;
            float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
            // Gear 1-6 speed se
            gear = Mathf.Clamp(Mathf.FloorToInt(Mathf.Abs(forwardSpeed) / 14f) + 1, 1, 6);
            rpm = 900f + (Mathf.Abs(forwardSpeed) % 14f) / 14f * 5500f + (accel ? 600f : 0f);
            if (forwardSpeed < -0.5f) gear = -1; // reverse = R

            float steerAngle = maxSteerAngle * steerInput * Mathf.Clamp01(1f - speedKmh / 400f);
            frontLeft.steerAngle = steerAngle;
            frontRight.steerAngle = steerAngle;

            float maxSpeedMs = maxSpeedKmh / 3.6f * (nitro ? nitroBoost : 1f);
            if (accel)
            {
                if (forwardSpeed < maxSpeedMs && forwardSpeed > -1f)
                {
                    float f = motorForce * (nitro ? nitroBoost : 1f);
                    rearLeft.motorTorque = f;
                    rearRight.motorTorque = f;
                    frontLeft.motorTorque = f * 0.4f;
                    frontRight.motorTorque = f * 0.4f;
                }
                else { rearLeft.motorTorque = 0f; rearRight.motorTorque = 0f; frontLeft.motorTorque = 0f; frontRight.motorTorque = 0f; }
                frontLeft.brakeTorque = 0f; frontRight.brakeTorque = 0f;
                rearLeft.brakeTorque = 0f; rearRight.brakeTorque = 0f;
            }
            else if (brake)
            {
                // Ruki hui ya bahut slow ho to reverse, warna brake
                if (forwardSpeed > 1f)
                {
                    ApplyBrake(1f);
                    rearLeft.motorTorque = 0f; rearRight.motorTorque = 0f;
                }
                else
                {
                    float rev = -motorForce * 0.55f;
                    rearLeft.motorTorque = rev; rearRight.motorTorque = rev;
                    frontLeft.brakeTorque = 0f; frontRight.brakeTorque = 0f;
                    rearLeft.brakeTorque = 0f; rearRight.brakeTorque = 0f;
                }
            }
            else
            {
                rearLeft.motorTorque = 0f; rearRight.motorTorque = 0f;
                frontLeft.motorTorque = 0f; frontRight.motorTorque = 0f;
                frontLeft.brakeTorque = 0f; frontRight.brakeTorque = 0f;
                rearLeft.brakeTorque = 0f; rearRight.brakeTorque = 0f;
                // halki engine braking
                if (speedKmh > 5f) rb.velocity *= 0.999f;
            }
            if (hand && speedKmh > 40f)
            {
                var ca = GetComponent<RealDrive.Audio.CollisionAudio>();
                if (ca != null) ca.PlayScreech();
            }
            if (hand)
            {
                rearLeft.brakeTorque = brakeForce * 1.5f;
                rearRight.brakeTorque = brakeForce * 1.5f;
                rearLeft.motorTorque = 0f; rearRight.motorTorque = 0f;
            }
            // Car ko ulta na hone do, zyada tilt rokne ke liye
            if (rb.velocity.magnitude < 0.2f && !accel && !brake) rb.velocity = Vector3.zero;
            UpdateWheels();
        }
        void ApplyBrake(float amount)
        {
            frontLeft.brakeTorque = brakeForce * amount;
            frontRight.brakeTorque = brakeForce * amount;
            rearLeft.brakeTorque = brakeForce * amount;
            rearRight.brakeTorque = brakeForce * amount;
        }
        void UpdateWheels()
        {
            UpdateOne(frontLeft, frontLeftVisual);
            UpdateOne(frontRight, frontRightVisual);
            UpdateOne(rearLeft, rearLeftVisual);
            UpdateOne(rearRight, rearRightVisual);
        }
        void UpdateOne(WheelCollider col, Transform visual)
        {
            if (col == null || visual == null) return;
            Vector3 pos; Quaternion rot;
            col.GetWorldPose(out pos, out rot);
            visual.position = pos;
            visual.rotation = rot;
        }
        public void SetHeadlights(bool on)
        {
            headlightsOn = on;
            if (headlights != null) foreach (var l in headlights) l.enabled = on;
        }
        public void ToggleHeadlights() { SetHeadlights(!headlightsOn); }
        public void Respawn(Vector3 pos, Quaternion rot)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = pos;
            transform.rotation = rot;
            MobileInput.ResetDriving();
        }
        public void StopCar()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ApplyBrake(1f);
        }
    }
}
