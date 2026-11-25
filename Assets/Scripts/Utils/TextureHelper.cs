using UnityEngine;
using System.Collections.Generic;
namespace RealDrive.Utils
{
    // Build 13 realism: procedural textures (Assets/Resources/Textures/*.png)
    // ko runtime materials par lagane ka shared helper. Shader order kabhi
    // mat badalna: Standard pehle, phir URP/Lit, phir Unlit/Color.
    public static class TextureHelper
    {
        static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();

        public static Shader LitShader()
        {
            Shader s = Shader.Find("Standard");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
            if (s == null) s = Shader.Find("Unlit/Color");
            return s;
        }

        // "Textures/asphalt" jaisa naam do (extension nahi). Nahi mila to null.
        public static Texture2D Load(string name)
        {
            if (cache.TryGetValue(name, out Texture2D t)) return t;
            t = Resources.Load<Texture2D>(name);
            if (t != null)
            {
                t.wrapMode = TextureWrapMode.Repeat;
                cache[name] = t;
            }
            return t;
        }

        // Rang + texture + tiling (tileX/tileY) wala material. Shader null ho to null.
        public static Material Mat(Color c, float metallic, float smooth, string texName, float tileX, float tileY)
        {
            Shader s = LitShader();
            if (s == null) return null;
            Material m = new Material(s);
            m.color = c;
            m.SetFloat("_Metallic", metallic);
            m.SetFloat("_Smoothness", smooth);
            if (!string.IsNullOrEmpty(texName))
            {
                Texture2D tex = Load(texName);
                if (tex != null)
                {
                    m.mainTexture = tex;
                    m.mainTextureScale = new Vector2(tileX, tileY);
                }
            }
            return m;
        }

        public static Material Mat(Color c, string texName, float tileX, float tileY)
        {
            return Mat(c, 0f, 0.5f, texName, tileX, tileY);
        }

        // Emissive (headlight / taillight / lamp) material.
        public static Material Glow(Color c, Color emit, float strength)
        {
            Shader s = LitShader();
            if (s == null) return null;
            Material m = new Material(s);
            m.color = c;
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", emit * strength);
            return m;
        }
    }
}
