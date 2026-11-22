using UnityEngine;
using RealDrive.Vehicle;
namespace RealDrive.World
{
    public class RespawnManager : MonoBehaviour
    {
        public CarController car;
        public Vector3 spawnPoint = new Vector3(0, 1f, 0);
        float upsideDownTime;
        void Update()
        {
            if (car == null) return;
            // Car gir gayi ya ulta ho gaya to 3 sec baad auto respawn
            if (car.transform.position.y < -5f)
            {
                car.Respawn(spawnPoint, Quaternion.identity);
                upsideDownTime = 0f;
                return;
            }
            if (car.transform.up.y < 0.2f)
            {
                upsideDownTime += Time.deltaTime;
                if (upsideDownTime > 3f)
                {
                    car.Respawn(spawnPoint, Quaternion.identity);
                    upsideDownTime = 0f;
                }
            }
            else upsideDownTime = 0f;
        }
    }
}
