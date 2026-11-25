#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RealDrive.Editor
{
    // Build 11 pink fix — shader inclusion keeper.
    //
    // Masla: saare materials runtime par Shader.Find("Standard") se bante hain,
    // lekin project mein ek bhi .mat asset nahi aur GraphicsSettings mein
    // Always Included Shaders khaali hai. Is liye build mein Standard / Unlit /
    // URP Lit shaders strip ho jate hain, Shader.Find null deta hai aur
    // new Material(null) wala material magenta (pink) render hota hai.
    // Builds 9 aur 10 ka Standard-first fallback is liye kaam nahi kiya.
    //
    // Ye preprocessor build se PEHLE (Editor mein, jahan Shader.Find kaam
    // karta hai) har zaroori shader ke liye ek asli .mat asset
    // Assets/Resources/ShaderKeeper/ mein bana kar save karta hai, taake
    // shader build mein include ho aur runtime Shader.Find usay dhoond sake.
    // Unity khud sahi shader reference serialize karta hai — hand-written
    // .mat YAML ya guessed fileID ki zaroorat nahi.
    public class ShaderKeeperBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        static readonly string[] NeededShaders = new[]
        {
            "Standard",
            "Unlit/Color",
            "Universal Render Pipeline/Lit",
            "UI/Default",
            "Sprites/Default",
        };

        public void OnPreprocessBuild(BuildReport report)
        {
            const string folder = "Assets/Resources/ShaderKeeper";
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets/Resources", "ShaderKeeper");

            foreach (string shaderName in NeededShaders)
            {
                Shader s = Shader.Find(shaderName);
                if (s == null)
                {
                    Debug.LogWarning("[ShaderKeeper] Editor mein shader nahi mila: " + shaderName);
                    continue;
                }
                string fileName = shaderName.Replace("/", "_").Replace(" ", "_") + ".mat";
                string path = folder + "/" + fileName;
                Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (existing == null)
                {
                    Material m = new Material(s);
                    m.name = "ShaderKeeper_" + shaderName;
                    AssetDatabase.CreateAsset(m, path);
                    Debug.Log("[ShaderKeeper] Material asset banaya: " + path);
                }
                else if (existing.shader != s)
                {
                    existing.shader = s;
                    EditorUtility.SetDirty(existing);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif
