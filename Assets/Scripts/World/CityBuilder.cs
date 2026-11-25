using UnityEngine;
using RealDrive.Utils;
namespace RealDrive.World
{
    // Stage 3: bada connected city, 5 districts, koi mission nahi, sirf free roam.
    // City 420 x 420 m, grid roads har 60 m par, sab districts ek hi scene me connected hain.
    // Build 13 realism: procedural textures (asphalt, grass, concrete, facades),
    // zebra crossings, road edge lines, behtar trees, textured petrol station + billboards.
    public static class CityBuilder
    {
        static Material asphaltX, asphaltZ, concrete, grass, glass, glass2, houseMat, shopMat, white, yellow, red, treeTrunk, treeLeaf, lampMat;
        static Material sidewalkX, sidewalkZ, roofMat, canopyMat, billDrive, billPetrol, billMall;
        static Transform cityRoot;

        static void InitMaterials()
        {
            Shader s = Shader.Find("Standard");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
            if (s == null) s = Shader.Find("Unlit/Color");
            // Textured materials (tiling = texture kitni baar repeat ho)
            asphaltX = TextureHelper.Mat(new Color(0.12f, 0.12f, 0.14f), 0f, 0.4f, "Textures/asphalt", 40, 1);
            asphaltZ = TextureHelper.Mat(new Color(0.12f, 0.12f, 0.14f), 0f, 0.4f, "Textures/asphalt", 1, 40);
            concrete = TextureHelper.Mat(new Color(0.55f, 0.55f, 0.58f), 0f, 0.5f, "Textures/concrete", 8, 8);
            grass = TextureHelper.Mat(new Color(0.25f, 0.5f, 0.25f), 0f, 0.6f, "Textures/grass", 45, 45);
            sidewalkX = TextureHelper.Mat(new Color(0.62f, 0.6f, 0.56f), 0f, 0.5f, "Textures/sidewalk", 40, 1);
            sidewalkZ = TextureHelper.Mat(new Color(0.62f, 0.6f, 0.56f), 0f, 0.5f, "Textures/sidewalk", 1, 40);
            roofMat = TextureHelper.Mat(new Color(0.5f, 0.5f, 0.54f), 0f, 0.5f, "Textures/roof", 2, 2);
            canopyMat = TextureHelper.Mat(Color.white, 0f, 0.5f, "Textures/petrol_canopy", 2, 1);
            billDrive = TextureHelper.Mat(Color.white, 0f, 0.4f, "Textures/billboard_drive", 1, 1);
            billPetrol = TextureHelper.Mat(Color.white, 0f, 0.4f, "Textures/billboard_petrol", 1, 1);
            billMall = TextureHelper.Mat(Color.white, 0f, 0.4f, "Textures/billboard_mall", 1, 1);
            houseMat = TextureHelper.Mat(Color.white, 0f, 0.5f, "Textures/facade_brick", 1, 1);
            shopMat = TextureHelper.Mat(Color.white, 0f, 0.6f, "Textures/facade_shop", 2, 1);
            // Solid colours (agar texture load na ho to bhi kaam kare)
            glass = Mat(s, new Color(0.35f, 0.55f, 0.75f), 0.7f, 0.9f);
            glass2 = Mat(s, new Color(0.55f, 0.6f, 0.7f), 0.6f, 0.8f);
            white = Mat(s, Color.white);
            yellow = Mat(s, new Color(1f, 0.8f, 0f));
            red = Mat(s, new Color(0.9f, 0.15f, 0.15f));
            treeTrunk = Mat(s, new Color(0.3f, 0.2f, 0.1f));
            treeLeaf = Mat(s, new Color(0.15f, 0.5f, 0.2f));
            lampMat = Mat(s, new Color(0.1f, 0.1f, 0.12f));
            // Texture wale materials agar null nikle (shader nahi mila) to solid wale use hon
            if (asphaltX == null) asphaltX = Mat(s, new Color(0.12f, 0.12f, 0.14f));
            if (asphaltZ == null) asphaltZ = asphaltX;
            if (concrete == null) concrete = Mat(s, new Color(0.55f, 0.55f, 0.58f));
            if (grass == null) grass = Mat(s, new Color(0.25f, 0.5f, 0.25f));
            if (sidewalkX == null) sidewalkX = concrete;
            if (sidewalkZ == null) sidewalkZ = concrete;
            if (roofMat == null) roofMat = concrete;
            if (canopyMat == null) canopyMat = white;
            if (houseMat == null) houseMat = Mat(s, new Color(0.85f, 0.75f, 0.6f));
            if (shopMat == null) shopMat = Mat(s, new Color(0.9f, 0.55f, 0.3f));
        }
        static Material Mat(Shader s, Color c, float metallic = 0f, float smooth = 0.5f)
        {
            if (s == null) return null;
            Material m = new Material(s);
            m.color = c; m.SetFloat("_Metallic", metallic); m.SetFloat("_Smoothness", smooth);
            return m;
        }
        // Facade texture wala material — building ki height ke hisaab se windows ki tiling
        static Material FacadeMat(string tex, float height, float brightness)
        {
            Material m = TextureHelper.Mat(new Color(brightness, brightness, brightness), 0.6f, 0.85f, tex, 1, Mathf.Max(0.7f, height / 30f));
            if (m != null) return m;
            return (tex == "Textures/facade_glass2") ? glass2 : glass;
        }
        static GameObject Box(string name, Vector3 pos, Vector3 scale, Material mat, bool collider = true)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(cityRoot, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            if (mat != null) go.GetComponent<Renderer>().material = mat;
            go.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            if (!collider) Object.Destroy(go.GetComponent<BoxCollider>());
            return go;
        }
        static void Tree(Vector3 pos)
        {
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "TreeTrunk"; trunk.transform.SetParent(cityRoot, false);
            trunk.transform.position = pos + new Vector3(0, 1.5f, 0); trunk.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
            if (treeTrunk != null) trunk.GetComponent<Renderer>().material = treeTrunk;
            // Foliage: ek bada + do chhote spheres, ghana ped lage
            Vector3[] tops = { new Vector3(0, 4.4f, 0), new Vector3(1.1f, 3.6f, 0.4f), new Vector3(-1f, 3.7f, -0.5f) };
            float[] sizes = { 3.2f, 2.1f, 2f };
            for (int i = 0; i < tops.Length; i++)
            {
                GameObject top = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                top.name = "TreeTop"; top.transform.SetParent(cityRoot, false);
                top.transform.position = pos + tops[i]; top.transform.localScale = new Vector3(sizes[i], sizes[i] * 0.9f, sizes[i]);
                if (treeLeaf != null) top.GetComponent<Renderer>().material = treeLeaf;
            }
        }
        static void Lamp(Vector3 pos)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "LampPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 2.5f, 0); pole.transform.localScale = new Vector3(0.2f, 5f, 0.2f);
            if (lampMat != null) pole.GetComponent<Renderer>().material = lampMat;
            // Arm - road ki taraf jhuka hua
            GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arm.name = "LampArm"; arm.transform.SetParent(cityRoot, false);
            arm.transform.position = pos + new Vector3(0.9f, 4.9f, 0); arm.transform.localScale = new Vector3(0.12f, 2f, 0.12f);
            arm.transform.rotation = Quaternion.Euler(0, 0, 90);
            if (lampMat != null) arm.GetComponent<Renderer>().material = lampMat;
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "LampHead"; head.transform.SetParent(cityRoot, false);
            head.transform.position = pos + new Vector3(1.8f, 4.9f, 0); head.transform.localScale = new Vector3(0.7f, 0.4f, 0.7f);
            Shader hs = head.GetComponent<Renderer>().material != null ? head.GetComponent<Renderer>().material.shader : null;
            Material lm = hs != null ? new Material(hs) : null;
            if (lm != null)
            {
                lm.color = Color.white; lm.EnableKeyword("_EMISSION"); lm.SetColor("_EmissionColor", Color.white * 0.8f);
                head.GetComponent<Renderer>().material = lm;
            }
        }
        static void TrafficLight(Vector3 pos)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "TrafficLightPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 2f, 0); pole.transform.localScale = new Vector3(0.25f, 4f, 0.25f);
            if (lampMat != null) pole.GetComponent<Renderer>().material = lampMat;
            Box("TrafficLightBox", pos + new Vector3(0, 4.2f, 0), new Vector3(0.6f, 1.4f, 0.6f), lampMat);
            // red / yellow / green chhoti spheres - abhi green on hai, Stage 4 me cycle ayega
            Color[] cols = { Color.red, Color.yellow, Color.green };
            for (int i = 0; i < 3; i++)
            {
                GameObject l = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                l.name = "TL_" + i; l.transform.SetParent(cityRoot, false);
                l.transform.position = pos + new Vector3(0.35f, 4.7f - i * 0.45f, 0); l.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                Shader ls = l.GetComponent<Renderer>().material != null ? l.GetComponent<Renderer>().material.shader : null;
                if (ls == null) continue;
                Material m = new Material(ls);
                m.color = cols[i];
                if (i == 2) { m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", Color.green * 1.2f); }
                l.GetComponent<Renderer>().material = m;
            }
        }
        static void Zebra(Vector3 centre, bool alongX)
        {
            // Intersection par safed zebra crossing
            for (int i = -3; i <= 3; i++)
            {
                if (alongX) Box("Zebra", centre + new Vector3(i * 1.4f, 0, 0), new Vector3(0.8f, 0.02f, 9), white, false);
                else Box("Zebra", centre + new Vector3(0, 0, i * 1.4f), new Vector3(9, 0.02f, 0.8f), white, false);
            }
        }
        static void PetrolStation(Vector3 pos, string name)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(cityRoot, false); root.transform.position = pos;
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "Roof"; roof.transform.SetParent(root.transform, false);
            roof.transform.localPosition = new Vector3(0, 4.5f, 0); roof.transform.localScale = new Vector3(16, 0.6f, 12);
            if (canopyMat != null) roof.GetComponent<Renderer>().material = canopyMat;
            GameObject stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripe.name = "RoofStripe"; stripe.transform.SetParent(root.transform, false);
            stripe.transform.localPosition = new Vector3(0, 4.2f, 6.1f); stripe.transform.localScale = new Vector3(16, 0.5f, 0.1f);
            if (red != null) stripe.GetComponent<Renderer>().material = red;
            for (int i = 0; i < 2; i++)
            {
                GameObject pump = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pump.name = "Pump_" + i; pump.transform.SetParent(root.transform, false);
                pump.transform.localPosition = new Vector3(-4f + i * 8f, 1f, 0); pump.transform.localScale = new Vector3(1.4f, 2f, 1f);
                if (red != null) pump.GetComponent<Renderer>().material = red;
                // Pump ka dark front panel
                GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                panel.name = "PumpPanel_" + i; panel.transform.SetParent(root.transform, false);
                panel.transform.localPosition = new Vector3(-4f + i * 8f, 1.2f, 0.55f); panel.transform.localScale = new Vector3(0.9f, 1f, 0.1f);
                if (lampMat != null) panel.GetComponent<Renderer>().material = lampMat;
            }
            GameObject sign = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sign.name = "Sign"; sign.transform.SetParent(root.transform, false);
            sign.transform.localPosition = new Vector3(0, 7f, 0); sign.transform.localScale = new Vector3(6, 2f, 0.5f);
            if (billPetrol != null) sign.GetComponent<Renderer>().material = billPetrol;
            else if (yellow != null) sign.GetComponent<Renderer>().material = yellow;
        }
        static void Billboard(Vector3 pos, Material faceMat)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "BillboardPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 4f, 0); pole.transform.localScale = new Vector3(0.4f, 8f, 0.4f);
            if (lampMat != null) pole.GetComponent<Renderer>().material = lampMat;
            Box("Billboard", pos + new Vector3(0, 9f, 0), new Vector3(12, 5, 0.5f), faceMat);
        }

        public static void BuildCity()
        {
            InitMaterials();
            GameObject root = new GameObject("City");
            cityRoot = root.transform;
            // Ground - bada grass base, texture wali ghaas
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "CityGround"; ground.transform.SetParent(cityRoot, false);
            ground.transform.localScale = new Vector3(45, 1, 45);
            if (grass != null) ground.GetComponent<Renderer>().material = grass;
            // Grid roads: har 60 m par, -180 se 180 tak
            for (int i = -3; i <= 3; i++)
            {
                float p = i * 60f;
                Box("Road_X_" + i, new Vector3(0, 0.05f, p), new Vector3(420, 0.1f, 10), asphaltX, false);
                Box("Road_Z_" + i, new Vector3(p, 0.05f, 0), new Vector3(10, 0.1f, 420), asphaltZ, false);
                // Road edge lines - safed kinare
                Box("Edge_Xa_" + i, new Vector3(0, 0.12f, p - 4.6f), new Vector3(420, 0.02f, 0.25f), white, false);
                Box("Edge_Xb_" + i, new Vector3(0, 0.12f, p + 4.6f), new Vector3(420, 0.02f, 0.25f), white, false);
                Box("Edge_Za_" + i, new Vector3(p - 4.6f, 0.12f, 0), new Vector3(0.25f, 0.02f, 420), white, false);
                Box("Edge_Zb_" + i, new Vector3(p + 4.6f, 0.12f, 0), new Vector3(0.25f, 0.02f, 420), white, false);
                // centre line - peeli dashed line ke liye chhote boxes
                for (int k = -3; k <= 3; k++)
                {
                    Box("Line_X_" + i + "_" + k, new Vector3(k * 60f, 0.12f, p), new Vector3(4, 0.02f, 0.4f), yellow, false);
                    Box("Line_Z_" + i + "_" + k, new Vector3(p, 0.12f, k * 60f), new Vector3(0.4f, 0.02f, 4), yellow, false);
                }
            }
            // Sidewalks - roads ke dono taraf halki concrete patti
            for (int i = -3; i <= 3; i++)
            {
                float p = i * 60f;
                Box("Walk_X_" + i, new Vector3(0, 0.08f, p + 6.5f), new Vector3(420, 0.06f, 3), sidewalkX, false);
                Box("Walk_Z_" + i, new Vector3(p + 6.5f, 0.08f, 0), new Vector3(3, 0.06f, 420), sidewalkZ, false);
            }
            BuildDowntown();
            BuildResidential();
            BuildCommercial();
            BuildSuburban();
            BuildHighway();
            // Traffic lights - 4 main intersections par + zebra crossings
            TrafficLight(new Vector3(6, 0, 6));
            TrafficLight(new Vector3(-6, 0, 66));
            TrafficLight(new Vector3(66, 0, -6));
            TrafficLight(new Vector3(-66, 0, -66));
            Zebra(new Vector3(0, 0.12f, 0), true);
            Zebra(new Vector3(0, 0.12f, 60), false);
            Zebra(new Vector3(60, 0.12f, 0), true);
            Zebra(new Vector3(-60, 0.12f, -60), false);
            // Lamps - har road par
            for (int i = -2; i <= 2; i++) Lamp(new Vector3(i * 60f + 6f, 0, -30f + i * 20f));
            for (int i = -2; i <= 2; i++) Lamp(new Vector3(-30f + i * 20f, 0, i * 60f + 6f));
            // Petrol stations - 2
            PetrolStation(new Vector3(-90, 0, 30), "PetrolStation_Downtown");
            PetrolStation(new Vector3(90, 0, -30), "PetrolStation_Commercial");
            // Billboards - asli likhai wali textures
            Billboard(new Vector3(-120, 0, -120), billDrive);
            Billboard(new Vector3(120, 0, 120), billMall);
            Billboard(new Vector3(0, 0, 180), billPetrol);
        }
        static void BuildDowntown()
        {
            // District 1: Modern downtown, centre (-105, -105), skyscrapers - khidki wali facades
            for (int ix = 0; ix < 3; ix++)
            {
                for (int iz = 0; iz < 4; iz++)
                {
                    float x = -150f + ix * 35f;
                    float z = -150f + iz * 35f;
                    float h = 22f + ((ix * 7 + iz * 11) % 4) * 9f;
                    float bright = 0.85f + ((ix + iz) % 3) * 0.08f;
                    Material m = FacadeMat((ix + iz) % 2 == 0 ? "Textures/facade_glass" : "Textures/facade_glass2", h, bright);
                    Box("Skyscraper_" + ix + "_" + iz, new Vector3(x, h / 2f, z), new Vector3(16, h, 16), m);
                    // roof + AC unit + antenna
                    Box("Roof_" + ix + "_" + iz, new Vector3(x, h + 1f, z), new Vector3(10, 2, 10), roofMat);
                    Box("AC_" + ix + "_" + iz, new Vector3(x - 2f, h + 2.4f, z), new Vector3(3, 0.8f, 3), concrete);
                    GameObject ant = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    ant.name = "Antenna_" + ix + "_" + iz; ant.transform.SetParent(cityRoot, false);
                    ant.transform.position = new Vector3(x + 3f, h + 4f, z); ant.transform.localScale = new Vector3(0.15f, 6f, 0.15f);
                    if (lampMat != null) ant.GetComponent<Renderer>().material = lampMat;
                }
            }
            Box("OfficePlaza", new Vector3(-105, 0.3f, -105), new Vector3(50, 0.6f, 50), concrete, false);
        }
        static void BuildResidential()
        {
            // District 2: Residential, centre (105, -105), ghar, garden, garage, driveway
            for (int i = 0; i < 12; i++)
            {
                float x = 75f + (i % 4) * 28f;
                float z = -150f + (i / 4) * 32f;
                // house base - eenton wali facade, khidkiyan texture mein
                Box("House_" + i, new Vector3(x, 2f, z), new Vector3(10, 4, 8), houseMat);
                // roof - upar chhota cube, halka tilt nahi, simple
                Box("HouseRoof_" + i, new Vector3(x, 4.8f, z), new Vector3(11, 1.2f, 9), red);
                // chimney
                Box("Chimney_" + i, new Vector3(x + 3f, 5.8f, z - 2f), new Vector3(1, 1.6f, 1), concrete);
                // door - saamne dark
                Box("Door_" + i, new Vector3(x, 1.1f, z + 4.1f), new Vector3(1.4f, 2.2f, 0.15f), lampMat);
                // garage
                Box("Garage_" + i, new Vector3(x + 8f, 1.5f, z), new Vector3(5, 3, 6), concrete);
                // driveway
                Box("Driveway_" + i, new Vector3(x + 4f, 0.12f, z + 8f), new Vector3(5, 0.08f, 8), concrete, false);
                // garden
                Box("Garden_" + i, new Vector3(x - 8f, 0.12f, z), new Vector3(6, 0.08f, 8), grass, false);
                Tree(new Vector3(x - 8f, 0, z + 6f));
            }
        }
        static void BuildCommercial()
        {
            // District 3: Commercial, centre (105, 105), shops, restaurants, parking
            for (int i = 0; i < 8; i++)
            {
                float x = 75f + (i % 4) * 30f;
                float z = 75f + (i / 4) * 35f;
                // shop - sheeshe wali dukaan, sign band texture mein
                Box("Shop_" + i, new Vector3(x, 2.5f, z), new Vector3(18, 5, 12), shopMat);
                // sign board
                Box("ShopSign_" + i, new Vector3(x, 5.8f, z + 6.2f), new Vector3(14, 1.5f, 0.4f), yellow);
                // awning - dukaan ke upar chhajja
                Box("Awning_" + i, new Vector3(x, 4.2f, z + 6.4f), new Vector3(16, 0.25f, 2.5f), red);
                // bench
                Box("Bench_" + i, new Vector3(x + 10f, 0.5f, z + 8f), new Vector3(3, 1, 1), treeTrunk);
            }
            // Parking area
            Box("ParkingLot", new Vector3(105, 0.1f, 150), new Vector3(80, 0.08f, 30), concrete, false);
            for (int i = 0; i < 8; i++)
            {
                Box("ParkLine_" + i, new Vector3(75f + i * 9f, 0.16f, 150), new Vector3(0.3f, 0.02f, 8), white, false);
            }
            // Restaurant - bada sign
            Box("Restaurant", new Vector3(150, 3f, 105), new Vector3(20, 6, 14), red);
        }
        static void BuildSuburban()
        {
            // District 5: Suburban, centre (-105, 105), park, trees, wide streets, chhote ghar
            Box("Park", new Vector3(-105, 0.12f, 105), new Vector3(70, 0.08f, 70), grass, false);
            for (int i = 0; i < 18; i++)
            {
                float x = -135f + (i % 6) * 12f;
                float z = 75f + (i / 6) * 22f;
                Tree(new Vector3(x, 0, z));
            }
            for (int i = 0; i < 6; i++)
            {
                Box("BenchPark_" + i, new Vector3(-130f + i * 10f, 0.5f, 105), new Vector3(3, 1, 1), treeTrunk);
            }
            for (int i = 0; i < 6; i++)
            {
                float x = -150f + i * 20f;
                Box("SubHouse_" + i, new Vector3(x, 2f, 155), new Vector3(10, 4, 8), houseMat);
                Box("SubRoof_" + i, new Vector3(x, 4.8f, 155), new Vector3(11, 1.2f, 9), red);
                Tree(new Vector3(x + 8f, 0, 155));
            }
        }
        static void BuildHighway()
        {
            // District 4: Highway, north edge z = 195, elevated road + bridge over main road
            Box("Highway", new Vector3(0, 8f, 195), new Vector3(420, 1f, 14), asphaltX);
            // pillars
            for (int i = -6; i <= 6; i++)
            {
                Box("HighwayPillar_" + i, new Vector3(i * 30f, 4f, 195), new Vector3(2, 8, 2), concrete);
            }
            // Bridge - highway se downtown ki taraf ramp jaisa overpass, x=0 par, z 60 se 120 tak, upar
            Box("Bridge", new Vector3(0, 6f, 90), new Vector3(12, 1f, 70), asphaltZ);
            for (int i = 0; i < 4; i++)
            {
                Box("BridgePillar_" + i, new Vector3(0, 3f, 65f + i * 18f), new Vector3(2, 6, 2), concrete);
            }
            // Highway centre line
            for (int k = -6; k <= 6; k++)
            {
                Box("HighwayLine_" + k, new Vector3(k * 30f, 8.6f, 195), new Vector3(8, 0.05f, 0.4f), yellow, false);
            }
        }
    }
}
