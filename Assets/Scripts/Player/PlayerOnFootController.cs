using UnityEngine;
using RealDrive.Vehicle;
namespace RealDrive.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerOnFootController : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float runSpeed = 9f;
        public bool isActive = false;
        CharacterController cc;
        float vy;
        Camera cam;
        void Awake() { cc = GetComponent<CharacterController>(); cam = Camera.main; }
        void Update()
        {
            if (!isActive) return;
            if (cam == null) cam = Camera.main;
            float h = 0f, v = 0f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
            h += MobileInput.MoveStick.x;
            v += MobileInput.MoveStick.y;
            h = Mathf.Clamp(h, -1f, 1f); v = Mathf.Clamp(v, -1f, 1f);
            bool run = MobileInput.Run || Input.GetKey(KeyCode.LeftShift);
            float speed = run ? runSpeed : walkSpeed;
            Vector3 fwd = cam != null ? cam.transform.forward : transform.forward;
            fwd.y = 0; fwd.Normalize();
            Vector3 right = cam != null ? cam.transform.right : transform.right;
            right.y = 0; right.Normalize();
            Vector3 move = (fwd * v + right * h).normalized * speed;
            if (move.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 10f);
            }
            vy += Physics.gravity.y * Time.deltaTime;
            if (cc.isGrounded && vy < 0) vy = -1f;
            move.y = vy;
            cc.Move(move * Time.deltaTime);
        }
    }
}
