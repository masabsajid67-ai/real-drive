using UnityEngine;
namespace RealDrive.Vehicle
{
    // Stage 2: touch + keyboard input ka ek hi jagah state. HUD buttons isko set karte hain, CarController isko padhta hai.
    public static class MobileInput
    {
        public static float Steer; // -1 left, +1 right
        public static bool Accelerator;
        public static bool Brake;
        public static bool Handbrake;
        public static bool Nitro;
        public static bool Run; // on-foot run
        public static Vector2 MoveStick; // on-foot joystick -1..1
        public static void ResetDriving()
        {
            Steer = 0f; Accelerator = false; Brake = false; Handbrake = false; Nitro = false;
        }
        public static void ResetAll()
        {
            ResetDriving(); Run = false; MoveStick = Vector2.zero;
        }
        // Keyboard (Editor / PC test) - HUD touch ke saath merge hota hai
        public static float KeyboardSteer()
        {
            float k = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) k -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) k += 1f;
            return k;
        }
        public static bool KeyboardAccel() { return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow); }
        public static bool KeyboardBrake() { return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow); }
        public static bool KeyboardHandbrake() { return Input.GetKey(KeyCode.Space); }
    }
}
