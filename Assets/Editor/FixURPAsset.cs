using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Safety net for the pink-screen bug: if the URP asset has no renderer data,
// create a real UniversalRendererData asset with the Unity API and assign it.
// Runs when the Editor loads the project (including Unity Cloud Build import),
// so a hand-authored .asset with a bad reference can never leave the game pink.
[InitializeOnLoad]
public static class FixURPAsset
{
    static FixURPAsset()
    {
        try { EnsureRendererData(); } catch (System.Exception e) { Debug.LogWarning("[FixURPAsset] " + e.Message); }
    }

    [InitializeOnLoadMethod]
    static void EnsureRendererData()
    {
        var urp = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>("Assets/Settings/UniversalRenderPipelineAsset.asset");
        if (urp == null) return;
        bool needsFix = false;
        try
        {
            var renderers = urp.rendererDataList;
            needsFix = renderers == null || renderers.Count == 0 || renderers[0] == null;
        }
        catch { needsFix = true; }
        if (!needsFix) return;
        var data = ScriptableObject.CreateInstance<UniversalRendererData>();
        AssetDatabase.CreateAsset(data, "Assets/Settings/UniversalRendererData.asset");
        urp.SetRendererDataList(new ScriptableRendererData[] { data });
        GraphicsSettings.renderPipelineAsset = urp;
        AssetDatabase.SaveAssets();
        Debug.Log("[FixURPAsset] Created UniversalRendererData and assigned it to the URP asset.");
    }
}
