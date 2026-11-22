using UnityEngine;
namespace RealDrive.World
{
    public class MinimapController : MonoBehaviour
    {
        public static MinimapController Instance { get; private set; }
        public RenderTexture MapTexture { get; private set; }
        public Transform followTarget;
        Camera mapCam;
        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            MapTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            MapTexture.name = "MinimapRT";
            GameObject go = new GameObject("MinimapCamera");
            go.transform.SetParent(transform, false);
            mapCam = go.AddComponent<Camera>();
            mapCam.targetTexture = MapTexture;
            mapCam.orthographic = true;
            mapCam.orthographicSize = 90f;
            mapCam.nearClipPlane = 1f;
            mapCam.farClipPlane = 300f;
            mapCam.clearFlags = CameraClearFlags.SolidColor;
            mapCam.backgroundColor = new Color(0.2f, 0.35f, 0.2f);
            mapCam.cullingMask = -1;
            mapCam.enabled = true;
        }
        void LateUpdate()
        {
            if (followTarget == null || mapCam == null) return;
            mapCam.transform.position = followTarget.position + new Vector3(0, 120f, 0);
            mapCam.transform.rotation = Quaternion.Euler(90f, 0, 0);
        }
        void OnDestroy()
        {
            if (MapTexture != null) MapTexture.Release();
        }
    }
}
