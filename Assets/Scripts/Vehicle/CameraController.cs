using UnityEngine;
namespace RealDrive.Vehicle
{
    public class CameraController : MonoBehaviour
    {
        public enum CamMode { Chase, CloseRear, Cockpit, Front }
        public CamMode mode = CamMode.Chase;
        public Transform carTarget;
        public Transform playerTarget;
        public bool followCar = true;
        Camera cam;
        Vector3 vel;
        void Awake() { cam = GetComponent<Camera>(); if (cam == null) cam = Camera.main; }
        public void NextCamera()
        {
            mode = (CamMode)(((int)mode + 1) % 4);
        }
        void LateUpdate()
        {
            if (cam == null) return;
            Transform target = followCar ? carTarget : playerTarget;
            if (target == null) return;
            if (followCar)
            {
                Vector3 offset = Vector3.zero; Vector3 look = target.position + Vector3.up * 1.2f;
                switch (mode)
                {
                    case CamMode.Chase: offset = target.TransformDirection(new Vector3(0, 3f, -8f)); break;
                    case CamMode.CloseRear: offset = target.TransformDirection(new Vector3(0, 1.8f, -4.5f)); break;
                    case CamMode.Cockpit: offset = target.TransformDirection(new Vector3(0, 1.25f, 0.4f)); look = target.position + target.forward * 10f + Vector3.up * 0.8f; break;
                    case CamMode.Front: offset = target.TransformDirection(new Vector3(0, 1.6f, 7.5f)); look = target.position + Vector3.up * 1f; break;
                }
                // Cockpit me smooth nahi, seedha attach
                if (mode == CamMode.Cockpit) { cam.transform.position = offset; cam.transform.rotation = target.rotation; }
                else { cam.transform.position = Vector3.SmoothDamp(cam.transform.position, offset, ref vel, 0.12f); cam.transform.LookAt(look); }
            }
            else
            {
                Vector3 offset = target.TransformDirection(new Vector3(0, 2.6f, -4.5f));
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, offset, ref vel, 0.12f);
                cam.transform.LookAt(target.position + Vector3.up * 1.4f);
            }
        }
    }
}
