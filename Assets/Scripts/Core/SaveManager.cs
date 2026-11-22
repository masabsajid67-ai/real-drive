using UnityEngine;
namespace RealDrive.Core
{
    public static class SaveManager
    {
        const string K_Sound = "RD_SoundVolume";
        const string K_Music = "RD_MusicVolume";
        const string K_Quality = "RD_QualityLevel";
        const string K_Car = "RD_SelectedCar";
        const string K_Control = "RD_ControlType"; // 0 = SteeringButtons, 1 = Tilt/Wheel
        const string K_CarColor = "RD_CarColor";
        const string K_Engine = "RD_EngineLevel";
        const string K_Brake = "RD_BrakeLevel";
        const string K_Handling = "RD_HandlingLevel";
        const string K_Rim = "RD_RimStyle";
        const string K_Spoiler = "RD_SpoilerStyle";
        const string K_Finish = "RD_Finish";

        public static float SoundVolume { get { return PlayerPrefs.GetFloat(K_Sound, 0.8f); } set { PlayerPrefs.SetFloat(K_Sound, Mathf.Clamp01(value)); PlayerPrefs.Save(); } }
        public static float MusicVolume { get { return PlayerPrefs.GetFloat(K_Music, 0.6f); } set { PlayerPrefs.SetFloat(K_Music, Mathf.Clamp01(value)); PlayerPrefs.Save(); } }
        public static int QualityLevel { get { return PlayerPrefs.GetInt(K_Quality, 1); } set { PlayerPrefs.SetInt(K_Quality, Mathf.Clamp(value, 0, 2)); PlayerPrefs.Save(); } }
        public static int SelectedCarIndex { get { return PlayerPrefs.GetInt(K_Car, 0); } set { PlayerPrefs.SetInt(K_Car, value); PlayerPrefs.Save(); } }
        public static int ControlType { get { return PlayerPrefs.GetInt(K_Control, 0); } set { PlayerPrefs.SetInt(K_Control, value); PlayerPrefs.Save(); } }
        public static string CarColorHex { get { return PlayerPrefs.GetString(K_CarColor, "#7CFC00"); } set { PlayerPrefs.SetString(K_CarColor, value); PlayerPrefs.Save(); } }
        public static int EngineLevel { get { return PlayerPrefs.GetInt(K_Engine, 0); } set { PlayerPrefs.SetInt(K_Engine, Mathf.Clamp(value, 0, 3)); PlayerPrefs.Save(); } }
        public static int BrakeLevel { get { return PlayerPrefs.GetInt(K_Brake, 0); } set { PlayerPrefs.SetInt(K_Brake, Mathf.Clamp(value, 0, 3)); PlayerPrefs.Save(); } }
        public static int HandlingLevel { get { return PlayerPrefs.GetInt(K_Handling, 0); } set { PlayerPrefs.SetInt(K_Handling, Mathf.Clamp(value, 0, 3)); PlayerPrefs.Save(); } }
        public static int RimStyle { get { return PlayerPrefs.GetInt(K_Rim, 0); } set { PlayerPrefs.SetInt(K_Rim, Mathf.Clamp(value, 0, 2)); PlayerPrefs.Save(); } }
        public static int SpoilerStyle { get { return PlayerPrefs.GetInt(K_Spoiler, 1); } set { PlayerPrefs.SetInt(K_Spoiler, Mathf.Clamp(value, 0, 3)); PlayerPrefs.Save(); } }
        public static int PaintFinish { get { return PlayerPrefs.GetInt(K_Finish, 0); } set { PlayerPrefs.SetInt(K_Finish, Mathf.Clamp(value, 0, 2)); PlayerPrefs.Save(); } }

        public static void ApplyQuality()
        {
            QualitySettings.SetQualityLevel(QualityLevel, true);
            Application.targetFrameRate = GameConstants.TargetFrameRate;
        }
        public static void ResetAll()
        {
            PlayerPrefs.DeleteKey(K_Sound); PlayerPrefs.DeleteKey(K_Music);
            PlayerPrefs.DeleteKey(K_Quality); PlayerPrefs.DeleteKey(K_Car);
            PlayerPrefs.DeleteKey(K_Control); PlayerPrefs.DeleteKey(K_CarColor);
            PlayerPrefs.Save();
        }
    }
}
