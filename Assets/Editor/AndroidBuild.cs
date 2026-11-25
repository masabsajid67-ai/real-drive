#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace RealDrive.Editor
{
    public static class AndroidBuild
    {
        [MenuItem("REAL DRIVE/Build Android APK")]
        public static void BuildAPK()
        {
            // Final Stage: PC par Unity Editor se APK banane ka one-click tareeqa
            PlayerSettings.applicationIdentifier = "com.jolet.realdrive";
            PlayerSettings.bundleVersion = "1.0.14";
            PlayerSettings.Android.bundleVersionCode = 14;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
            string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/LoadingScene.unity", "Assets/Scenes/OpenWorld.unity", "Assets/Scenes/Garage.unity" };
            string path = "Builds/Android/REAL-DRIVE-v1.0.0.apk";
            System.IO.Directory.CreateDirectory("Builds/Android");
            BuildReport report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
            if (report.summary.result == BuildResult.Succeeded) Debug.Log("REAL DRIVE APK build succeeded: " + path);
            else Debug.LogError("REAL DRIVE APK build failed: " + report.summary.result);
        }
        [MenuItem("REAL DRIVE/Check Scenes In Build")]
        public static void CheckScenes()
        {
            foreach (var s in EditorBuildSettings.scenes) Debug.Log(s.path + " enabled=" + s.enabled);
        }
    }
}
#endif
