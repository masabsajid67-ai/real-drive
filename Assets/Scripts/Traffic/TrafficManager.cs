using System.Collections.Generic;
using UnityEngine;
namespace RealDrive.Traffic
{
    public class TrafficManager : MonoBehaviour
    {
        public Transform playerTransform;
        public int poolSize = 14;
        public float spawnRadius = 140f;
        public float despawnRadius = 200f;
        List<TrafficCar> pool = new List<TrafficCar>();
        static readonly float[] Roads = { -180f, -120f, -60f, 0f, 60f, 120f, 180f };

        public void Setup(Transform player)
        {
            playerTransform = player;
            // Object pooling: pehle se 14 cars banao, door wali ko deactivate karo
            for (int i = 0; i < poolSize; i++)
            {
                TrafficType t = (TrafficType)(i % 6);
                Color col = TrafficCarBuilder.ColorFor(t, i);
                GameObject go = TrafficCarBuilder.Build(t, Vector3.zero, Quaternion.identity, col);
                go.transform.SetParent(transform, false);
                TrafficCar tc = go.AddComponent<TrafficCar>();
                tc.Setup(t, player);
                tc.isActiveInPool = false;
                go.SetActive(false);
                pool.Add(tc);
            }
            // Shuru me player ke aas paas spawn karo
            for (int i = 0; i < poolSize; i++) SpawnOne(pool[i], true);
        }
        void Update()
        {
            if (playerTransform == null) return;
            // Distance-based update: door wali cars ko despawn karke player ke paas dobara spawn karo
            foreach (var tc in pool)
            {
                if (!tc.isActiveInPool) continue;
                float d = Vector3.Distance(tc.transform.position, playerTransform.position);
                if (d > despawnRadius)
                {
                    SpawnOne(tc, false);
                }
            }
        }
        void SpawnOne(TrafficCar tc, bool initial)
        {
            Vector3 playerPos = playerTransform != null ? playerTransform.position : Vector3.zero;
            // Player ke aas paas koi road line chuno
            float road = Roads[Random.Range(0, Roads.Length)];
            bool alongX = Random.value < 0.5f;
            int dir = Random.value < 0.5f ? 1 : -1;
            float along = playerPos.x + Random.Range(-spawnRadius, spawnRadius);
            if (alongX) along = Mathf.Clamp(along, -180f, 180f); else along = Mathf.Clamp(playerPos.z + Random.Range(-spawnRadius, spawnRadius), -180f, 180f);
            Vector3 pos;
            Quaternion rot;
            if (alongX)
            {
                pos = new Vector3(along, 0.5f, road + (dir > 0 ? -2.2f : 2.2f));
                rot = Quaternion.Euler(0, dir > 0 ? 90 : 270, 0);
            }
            else
            {
                pos = new Vector3(road + (dir > 0 ? 2.2f : -2.2f), 0.5f, along);
                rot = Quaternion.Euler(0, dir > 0 ? 0 : 180, 0);
            }
            // Player ke bahut paas spawn na karo
            if (Vector3.Distance(pos, playerPos) < 15f) pos += new Vector3(0, 0, 30f);
            tc.transform.position = pos;
            tc.transform.rotation = rot;
            tc.movingAlongX = alongX;
            tc.direction = dir;
            tc.currentSpeed = tc.cruiseSpeed * 0.5f;
            tc.isActiveInPool = true;
            tc.gameObject.SetActive(true);
        }
        public int ActiveCount()
        {
            int c = 0;
            foreach (var tc in pool) if (tc.isActiveInPool) c++;
            return c;
        }
    }
}
