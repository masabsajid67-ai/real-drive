using System.Collections.Generic;
using UnityEngine;
namespace RealDrive.Data
{
    [System.Serializable]
    public class CarData
    {
        public string id;
        public string displayName;
        public string category; // Sports, Supercar, Muscle, Hypercar, Luxury
        public int topSpeedKmh;
        public float acceleration; // 0-10
        public float handling; // 0-10
        public float braking; // 0-10
        public int price;
        public int weightKg;
        public Color bodyColor;
        public CarData(string id, string name, string cat, int top, float acc, float han, float brk, int price, Color col)
        { this.id=id; displayName=name; category=cat; topSpeedKmh=top; acceleration=acc; handling=han; braking=brk; this.price=price; bodyColor=col; weightKg = 1200 + (int)(top * 2); }
        public CarData(string id, string name, string cat, int top, float acc, float han, float brk, int price, int weight, Color col)
        { this.id=id; displayName=name; category=cat; topSpeedKmh=top; acceleration=acc; handling=han; braking=brk; this.price=price; weightKg=weight; bodyColor=col; }
    }
    public static class CarCatalog
    {
        public static List<CarData> All()
        {
            return new List<CarData> {
                new CarData("rd_storm_gt","Storm GT","Sports",320,8.2f,8.0f,8.5f,0,1350,new Color(0.48f,1f,0f)),
                new CarData("rd_thunder_v8","Thunder V8","Muscle",295,7.5f,7.0f,7.5f,22000,1650,new Color(0.9f,0.05f,0.05f)),
                new CarData("rd_phantom_x","Phantom X","Supercar",340,9.0f,8.8f,9.0f,45000,1420,new Color(0.9f,0.9f,0.9f)),
                new CarData("rd_apex_one","Apex One","Hypercar",380,9.8f,9.5f,9.6f,120000,1280,new Color(1f,0.85f,0f)),
                new CarData("rd_royal_s","Royal S","Luxury",280,7.0f,7.8f,8.2f,38000,1750,new Color(0.15f,0.15f,0.2f)),
            };
        }
        public static CarData Selected()
        {
            var list = All();
            int i = Mathf.Clamp(RealDrive.Core.SaveManager.SelectedCarIndex, 0, list.Count - 1);
            return list[i];
        }
    }
}
