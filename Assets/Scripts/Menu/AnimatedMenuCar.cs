using UnityEngine;
namespace RealDrive.Menu
{
    public class AnimatedMenuCar : MonoBehaviour
    {
        public Transform carTransform;
        public Transform cameraTransform;
        float angle;
        Vector3 basePos;
        void Start()
        {
            if (carTransform != null) basePos = carTransform.position;
        }
        void Update()
        {
            angle += Time.deltaTime * 12f;
            if (carTransform != null)
            {
                carTransform.rotation = Quaternion.Euler(0, angle, 0);
                carTransform.position = basePos + new Vector3(0, Mathf.Sin(Time.time * 1.2f) * 0.03f, 0);
            }
            if (cameraTransform != null)
            {
                float a = Time.time * 0.08f;
                cameraTransform.position = new Vector3(Mathf.Sin(a) * 7f, 3.2f + Mathf.Sin(Time.time * 0.3f) * 0.3f, Mathf.Cos(a) * 7f);
                cameraTransform.LookAt(new Vector3(0, 0.8f, 0));
            }
        }
    }
}
