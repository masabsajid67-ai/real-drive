using UnityEngine;
namespace RealDrive.Traffic
{
    public class TrafficCar : MonoBehaviour
    {
        public TrafficType type;
        public float cruiseSpeed = 12f;
        public float currentSpeed;
        public bool movingAlongX = false; // false = Z road, true = X road
        public int direction = 1; // +1 ya -1
        public bool isActiveInPool = false;
        Transform playerCar;
        AudioSource hornSource;
        float blockedTime;
        float hornCooldown;
        static readonly float[] RoadLines = { -180f, -120f, -60f, 0f, 60f, 120f, 180f };
        static readonly float LaneOffset = 2.2f;

        public void Setup(TrafficType t, Transform player)
        {
            type = t;
            cruiseSpeed = TrafficCarBuilder.SpeedFor(t);
            playerCar = player;
            hornSource = gameObject.AddComponent<AudioSource>();
            hornSource.playOnAwake = false; hornSource.volume = 0.4f;
            hornSource.clip = CreateHornClip();
        }
        void Update()
        {
            if (!isActiveInPool) return;
            // Aage wali gaari / player se safe distance
            float targetSpeed = cruiseSpeed;
            if (IsBlockedAhead(out float distAhead))
            {
                if (distAhead < 6f) targetSpeed = 0f;
                else if (distAhead < 12f) targetSpeed = cruiseSpeed * 0.35f;
                blockedTime += Time.deltaTime;
                if (blockedTime > 3f && hornCooldown <= 0f)
                {
                    if (hornSource != null && hornSource.clip != null) hornSource.Play();
                    hornCooldown = 4f; blockedTime = 0f;
                }
            }
            else blockedTime = 0f;
            if (hornCooldown > 0f) hornCooldown -= Time.deltaTime;
            // Traffic light: intersection ke paas ho aur red/yellow ho to ruko
            if (NearIntersection(out float distToInter) && distToInter < 12f && distToInter > 2f)
            {
                if (TrafficLightSystem.MustStop(movingAlongX)) targetSpeed = 0f;
            }
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 6f * Time.deltaTime);
            // Aage badho
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
            // Intersection par aao to kabhi kabhi mud jao (left/right/straight)
            if (NearIntersection(out float d2) && d2 < 1.5f && currentSpeed > 0.5f)
            {
                TryTurn();
            }
        }
        bool NearIntersection(out float dist)
        {
            dist = 999f;
            float along = movingAlongX ? transform.position.x : transform.position.z;
            float nearest = 999f;
            foreach (float line in RoadLines)
            {
                float dd = Mathf.Abs(along - line);
                // Sirf aage wala intersection dekho
                float ahead = (line - along) * direction;
                if (ahead > 0 && ahead < nearest) nearest = ahead;
            }
            dist = nearest;
            return nearest < 20f;
        }
        void TryTurn()
        {
            // Har intersection par 30% chance mudne ka, warna seedha
            if (Random.value > 0.3f) return;
            // Snap to grid line taake lane se bahar na jaye
            float along = movingAlongX ? transform.position.x : transform.position.z;
            float snapped = Mathf.Round(along / 60f) * 60f;
            Vector3 p = transform.position;
            if (movingAlongX) p.x = snapped; else p.z = snapped;
            transform.position = p;
            bool turnLeft = Random.value < 0.5f;
            // Nayi direction: X se Z ya Z se X
            if (movingAlongX)
            {
                movingAlongX = false;
                direction = turnLeft ? -1 : 1;
                // Lane offset: right-hand traffic ke liye
                p = transform.position;
                p.x += direction > 0 ? LaneOffset : -LaneOffset;
                transform.position = p;
                transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
            }
            else
            {
                movingAlongX = true;
                direction = turnLeft ? 1 : -1;
                p = transform.position;
                p.z += direction > 0 ? -LaneOffset : LaneOffset;
                transform.position = p;
                transform.rotation = Quaternion.Euler(0, direction > 0 ? 90 : 270, 0);
            }
        }
        bool IsBlockedAhead(out float dist)
        {
            dist = 999f;
            RaycastHit hit;
            // Aage 14 m tak dekho, sirf Traffic aur Player layer - yahan tag se check karte hain
            Vector3 origin = transform.position + Vector3.up * 1f + transform.forward * 3.5f;
            if (Physics.Raycast(origin, transform.forward, out hit, 14f))
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    string tag = hit.collider.gameObject.tag;
                    if (tag == "Traffic" || tag == "Player")
                    {
                        dist = hit.distance;
                        return true;
                    }
                }
            }
            // Player ke paas ho to bhi check (raycast miss ho to)
            if (playerCar != null)
            {
                Vector3 toPlayer = playerCar.position - transform.position;
                if (toPlayer.magnitude < 10f && Vector3.Dot(toPlayer.normalized, transform.forward) > 0.7f)
                {
                    dist = toPlayer.magnitude;
                    return true;
                }
            }
            return false;
        }
        AudioClip CreateHornClip()
        {
            int rate = 22050; int len = rate / 3;
            AudioClip clip = AudioClip.Create("Horn", len, 1, rate, false);
            float[] d = new float[len];
            for (int i = 0; i < len; i++)
            {
                float t = (float)i / rate;
                d[i] = (Mathf.Sin(2f * Mathf.PI * 370f * t) * 0.4f + Mathf.Sin(2f * Mathf.PI * 466f * t) * 0.3f) * 0.5f;
            }
            clip.SetData(d, 0);
            return clip;
        }
    }
}
