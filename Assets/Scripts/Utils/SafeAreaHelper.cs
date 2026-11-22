using UnityEngine;
namespace RealDrive.Utils
{
    public class SafeAreaHelper : MonoBehaviour
    {
        void Start()
        {
            // Stage 1: CanvasScaler already handles most phones.
            // Notch / punch-hole ke liye Canvas ko SafeArea ke andar rakhna Stage 6 polish me add hoga.
            // Yahan sirf landscape orientation enforce karte hain.
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
