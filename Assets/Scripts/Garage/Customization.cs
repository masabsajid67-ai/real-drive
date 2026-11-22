using UnityEngine;
namespace RealDrive.Garage
{
    // Stage 5: customization ka data, PlayerPrefs me save hota hai (SaveManager ke zariye).
    // Masab ka rule: garage me sirf 5 cars, zyada nahi. Upgrades free hain - koi mission / paisa nahi hai.
    public static class Customization
    {
        public static string[] RimNames = { "STANDARD", "SPORT", "CHROME" };
        public static string[] SpoilerNames = { "NONE", "SMALL", "LARGE", "GT WING" };
        public static string[] FinishNames = { "GLOSSY", "MATTE", "METALLIC" };
        public static Color RimColor(int style)
        {
            switch (style)
            {
                case 1: return new Color(0.2f, 0.2f, 0.25f);
                case 2: return new Color(0.9f, 0.9f, 0.95f);
                default: return new Color(0.5f, 0.5f, 0.55f);
            }
        }
        public static void ApplyFinish(Material bodyMat, int finish)
        {
            if (bodyMat == null) return;
            if (finish == 1) { bodyMat.SetFloat("_Smoothness", 0.25f); bodyMat.SetFloat("_Metallic", 0.1f); }
            else if (finish == 2) { bodyMat.SetFloat("_Smoothness", 0.9f); bodyMat.SetFloat("_Metallic", 0.95f); }
            else { bodyMat.SetFloat("_Smoothness", 0.85f); bodyMat.SetFloat("_Metallic", 0.6f); }
        }
        public static float EngineMultiplier(int level) { return 1f + level * 0.08f; }
        public static float BrakeMultiplier(int level) { return 1f + level * 0.15f; }
        public static float HandlingMultiplier(int level) { return 1f + level * 0.07f; }
    }
}
