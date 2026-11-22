#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace RealDrive.Editor
{
    // GitHub Actions (game-ci) aur Cloud Build ke liye CI build method.
    // -executeMethod RealDrive.Editor.CIBuild.BuildAndroid se chalta hai.
    public static class CIBuild
    {
        public static void BuildAndroid()
        {
            PlayerSettings.bundleIdentifier = "com.jolet.realdrive";
            PlayerSettings.bundleVersion = "1.0.0";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/LoadingScene.unity", "Assets/Scenes/OpenWorld.unity", "Assets/Scenes/Garage.unity" };
            System.IO.Directory.CreateDirectory("build/Android");
            string path = "build/Android/REAL-DRIVE-v1.0.0.apk";
            BuildReport report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
            if (report.summary.result != BuildResult.Succeeded)
            {
                Debug.LogError("CI Android build failed: " + report.summary.result);
                EditorApplication.Exit(1);
            }
            Debug.Log("CI Android build succeeded: " + path);
        }
    }
}
#endif
