using UnityEngine;
namespace RealDrive.World
{
    // Stage 3: bada connected city, 5 districts, koi mission nahi, sirf free roam.
    // City 420 x 420 m, grid roads har 60 m par, sab districts ek hi scene me connected hain.
    public static class CityBuilder
    {
        static Material asphalt, concrete, grass, glass, glass2, houseMat, shopMat, white, yellow, red, treeTrunk, treeLeaf, lampMat;
        static Transform cityRoot;

        static void InitMaterials()
        {
            Shader s = Shader.Find("Standard");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
            if (s == null) s = Shader.Find("Unlit/Color");
            asphalt = Mat(s, new Color(0.12f, 0.12f, 0.14f));
            concrete = Mat(s, new Color(0.55f, 0.55f, 0.58f));
            grass = Mat(s, new Color(0.25f, 0.5f, 0.25f));
            glass = Mat(s, new Color(0.35f, 0.55f, 0.75f), 0.7f, 0.9f);
            glass2 = Mat(s, new Color(0.55f, 0.6f, 0.7f), 0.6f, 0.8f);
            houseMat = Mat(s, new Color(0.85f, 0.75f, 0.6f));
            shopMat = Mat(s, new Color(0.9f, 0.55f, 0.3f));
            white = Mat(s, Color.white);
            yellow = Mat(s, new Color(1f, 0.8f, 0f));
            red = Mat(s, new Color(0.9f, 0.15f, 0.15f));
            treeTrunk = Mat(s, new Color(0.3f, 0.2f, 0.1f));
            treeLeaf = Mat(s, new Color(0.15f, 0.5f, 0.2f));
            lampMat = Mat(s, new Color(0.1f, 0.1f, 0.12f));
        }
        static Material Mat(Shader s, Color c, float metallic = 0f, float smooth = 0.5f)
        {
            if (s == null) return null;
            Material m = new Material(s);
            m.color = c; m.SetFloat("_Metallic", metallic); m.SetFloat("_Smoothness", smooth);
            return m;
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
            trunk.GetComponent<Renderer>().material = treeTrunk;
            GameObject top = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            top.name = "TreeTop"; top.transform.SetParent(cityRoot, false);
            top.transform.position = pos + new Vector3(0, 4.2f, 0); top.transform.localScale = new Vector3(3f, 3f, 3f);
            top.GetComponent<Renderer>().material = treeLeaf;
        }
        static void Lamp(Vector3 pos)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "LampPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 2.5f, 0); pole.transform.localScale = new Vector3(0.2f, 5f, 0.2f);
            pole.GetComponent<Renderer>().material = lampMat;
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "LampHead"; head.transform.SetParent(cityRoot, false);
            head.transform.position = pos + new Vector3(0, 5f, 0); head.transform.localScale = new Vector3(0.7f, 0.4f, 0.7f);
            Material lm = new Material(head.GetComponent<Renderer>().material.shader);
            lm.color = Color.white; lm.EnableKeyword("_EMISSION"); lm.SetColor("_EmissionColor", Color.white * 0.8f);
            head.GetComponent<Renderer>().material = lm;
        }
        static void TrafficLight(Vector3 pos)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "TrafficLightPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 2f, 0); pole.transform.localScale = new Vector3(0.25f, 4f, 0.25f);
            pole.GetComponent<Renderer>().material = lampMat;
            GameObject box = Box("TrafficLightBox", pos + new Vector3(0, 4.2f, 0), new Vector3(0.6f, 1.4f, 0.6f), lampMat);
            // red / yellow / green chhoti spheres - abhi green on hai, Stage 4 me cycle ayega
            Color[] cols = { Color.red, Color.yellow, Color.green };
            for (int i = 0; i < 3; i++)
            {
                GameObject l = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                l.name = "TL_" + i; l.transform.SetParent(cityRoot, false);
                l.transform.position = pos + new Vector3(0.35f, 4.7f - i * 0.45f, 0); l.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                Material m = new Material(l.GetComponent<Renderer>().material.shader);
                m.color = cols[i];
                if (i == 2) { m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", Color.green * 1.2f); }
                l.GetComponent<Renderer>().material = m;
            }
        }
        static void PetrolStation(Vector3 pos, string name)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(cityRoot, false); root.transform.position = pos;
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roof.name = "Roof"; roof.transform.SetParent(root.transform, false);
            roof.transform.localPosition = new Vector3(0, 4.5f, 0); roof.transform.localScale = new Vector3(16, 0.6f, 12);
            roof.GetComponent<Renderer>().material = white;
            GameObject stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripe.name = "RoofStripe"; stripe.transform.SetParent(root.transform, false);
            stripe.transform.localPosition = new Vector3(0, 4.2f, 6.1f); stripe.transform.localScale = new Vector3(16, 0.5f, 0.1f);
            stripe.GetComponent<Renderer>().material = red;
            for (int i = 0; i < 2; i++)
            {
                GameObject pump = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pump.name = "Pump_" + i; pump.transform.SetParent(root.transform, false);
                pump.transform.localPosition = new Vector3(-4f + i * 8f, 1f, 0); pump.transform.localScale = new Vector3(1.4f, 2f, 1f);
                pump.GetComponent<Renderer>().material = red;
            }
            GameObject sign = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sign.name = "Sign"; sign.transform.SetParent(root.transform, false);
            sign.transform.localPosition = new Vector3(0, 7f, 0); sign.transform.localScale = new Vector3(6, 2f, 0.5f);
            sign.GetComponent<Renderer>().material = yellow;
        }
        static void Billboard(Vector3 pos, Color c)
        {
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "BillboardPole"; pole.transform.SetParent(cityRoot, false);
            pole.transform.position = pos + new Vector3(0, 4f, 0); pole.transform.localScale = new Vector3(0.4f, 8f, 0.4f);
            pole.GetComponent<Renderer>().material = lampMat;
            Box("Billboard", pos + new Vector3(0, 9f, 0), new Vector3(12, 5, 0.5f), Mat(pole.GetComponent<Renderer>().material.shader, c));
        }

        public static void BuildCity()
        {
            InitMaterials();
            GameObject root = new GameObject("City");
            cityRoot = root.transform;
            // Ground - bada grass base
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "CityGround"; ground.transform.SetParent(cityRoot, false);
            ground.transform.localScale = new Vector3(45, 1, 45);
            if (grass != null) ground.GetComponent<Renderer>().material = grass;
            // Grid roads: har 60 m par, -180 se 180 tak
            for (int i = -3; i <= 3; i++)
            {
                float p = i * 60f;
                Box("Road_X_" + i, new Vector3(0, 0.05f, p), new Vector3(420, 0.1f, 10), asphalt, false);
                Box("Road_Z_" + i, new Vector3(p, 0.05f, 0), new Vector3(10, 0.1f, 420), asphalt, false);
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
                Box("Walk_X_" + i, new Vector3(0, 0.08f, p + 6.5f), new Vector3(420, 0.06f, 3), concrete, false);
                Box("Walk_Z_" + i, new Vector3(p + 6.5f, 0.08f, 0), new Vector3(3, 0.06f, 420), concrete, false);
            }
            BuildDowntown();
            BuildResidential();
            BuildCommercial();
            BuildSuburban();
            BuildHighway();
            // Traffic lights - 4 main intersections par
            TrafficLight(new Vector3(6, 0, 6));
            TrafficLight(new Vector3(-6, 0, 66));
            TrafficLight(new Vector3(66, 0, -6));
            TrafficLight(new Vector3(-66, 0, -66));
            // Lamps - har road par
            for (int i = -2; i <= 2; i++) Lamp(new Vector3(i * 60f + 6f, 0, -30f + i * 20f));
            // Petrol stations - 2
            PetrolStation(new Vector3(-90, 0, 30), "PetrolStation_Downtown");
            PetrolStation(new Vector3(90, 0, -30), "PetrolStation_Commercial");
            // Billboards
            Billboard(new Vector3(-120, 0, -120), new Color(1f, 0.6f, 0f));
            Billboard(new Vector3(120, 0, 120), new Color(0.2f, 0.6f, 1f));
            Billboard(new Vector3(0, 0, 180), new Color(0.9f, 0.2f, 0.5f));
        }
        static void BuildDowntown()
        {
            // District 1: Modern downtown, centre (-105, -105), skyscrapers
            for (int ix = 0; ix < 3; ix++)
            {
                for (int iz = 0; iz < 4; iz++)
                {
                    float x = -150f + ix * 35f;
                    float z = -150f + iz * 35f;
                    float h = 22f + ((ix * 7 + iz * 11) % 4) * 9f;
                    Material m = (ix + iz) % 2 == 0 ? glass : glass2;
                    Box("Skyscraper_" + ix + "_" + iz, new Vector3(x, h / 2f, z), new Vector3(16, h, 16), m);
                    // roof box
                    Box("Roof_" + ix + "_" + iz, new Vector3(x, h + 1f, z), new Vector3(10, 2, 10), concrete);
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
                // house base
                Box("House_" + i, new Vector3(x, 2f, z), new Vector3(10, 4, 8), houseMat);
                // roof - upar chhota cube, halka tilt nahi, simple
                GameObject roof = Box("HouseRoof_" + i, new Vector3(x, 4.8f, z), new Vector3(11, 1.2f, 9), red);
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
            Color[] shopCols = { new Color(0.9f, 0.5f, 0.2f), new Color(0.2f, 0.7f, 0.9f), new Color(0.9f, 0.2f, 0.4f), new Color(0.3f, 0.8f, 0.4f) };
            for (int i = 0; i < 8; i++)
            {
                float x = 75f + (i % 4) * 30f;
                float z = 75f + (i / 4) * 35f;
                // shop box - material seedha banao taake shader null na ho
                Shader ss = Shader.Find("Standard"); if (ss == null) ss = Shader.Find("Universal Render Pipeline/Lit"); if (ss == null) ss = Shader.Find("Unlit/Color");
                Material mm = new Material(ss); mm.color = shopCols[i % 4];
                Box("Shop_" + i, new Vector3(x, 2.5f, z), new Vector3(18, 5, 12), mm);
                // sign board
                Box("ShopSign_" + i, new Vector3(x, 5.8f, z + 6.2f), new Vector3(14, 1.5f, 0.4f), yellow);
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
                Tree(new Vector3(x + 8f, 0, 155));
            }
        }
        static void BuildHighway()
        {
            // District 4: Highway, north edge z = 195, elevated road + bridge over main road
            Box("Highway", new Vector3(0, 8f, 195), new Vector3(420, 1f, 14), asphalt);
            // pillars
            for (int i = -6; i <= 6; i++)
            {
                Box("HighwayPillar_" + i, new Vector3(i * 30f, 4f, 195), new Vector3(2, 8, 2), concrete);
            }
            // Bridge - highway se downtown ki taraf ramp jaisa overpass, x=0 par, z 60 se 120 tak, upar
            Box("Bridge", new Vector3(0, 6f, 90), new Vector3(12, 1f, 70), asphalt);
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
