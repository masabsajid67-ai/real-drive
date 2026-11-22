using UnityEngine;
using RealDrive.Vehicle;
namespace RealDrive.Player
{
    public class PlayerVehicleManager : MonoBehaviour
    {
        public CarController car;
        public GameObject playerObject;
        public PlayerOnFootController onFoot;
        public CameraController camController;
        public bool isDriving = true;
        public float enterDistance = 4.5f;
        public System.Action<bool> OnStateChanged; // true = driving

        public void Setup(CarController c, GameObject player, CameraController cam)
        {
            car = c; playerObject = player; camController = cam;
            onFoot = player.GetComponent<PlayerOnFootController>();
            StartDriving();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) ToggleEnterExit();
            if (Input.GetKeyDown(KeyCode.R) && isDriving && car != null)
            {
                car.Respawn(car.transform.position + Vector3.up * 1f, Quaternion.identity);
            }
        }
        public bool CanEnterCar()
        {
            if (isDriving || car == null || playerObject == null) return false;
            return Vector3.Distance(playerObject.transform.position, car.transform.position) < enterDistance;
        }
        public void ToggleEnterExit()
        {
            if (isDriving) ExitCar();
            else if (CanEnterCar()) EnterCar();
        }
        public void EnterCar()
        {
            if (car == null) return;
            isDriving = true;
            if (onFoot != null) onFoot.isActive = false;
            if (playerObject != null) playerObject.SetActive(false);
            car.isDrivingEnabled = true;
            car.StopCar();
            if (camController != null) { camController.followCar = true; camController.carTarget = car.transform; }
            MobileInput.ResetAll();
            OnStateChanged?.Invoke(true);
        }
        public void ExitCar()
        {
            if (car == null) return;
            isDriving = false;
            car.isDrivingEnabled = false;
            car.StopCar();
            Vector3 side = car.transform.position + car.transform.right * 2.2f + Vector3.up * 0.5f;
            if (playerObject != null)
            {
                playerObject.transform.position = side;
                playerObject.transform.rotation = car.transform.rotation;
                playerObject.SetActive(true);
            }
            if (onFoot != null) onFoot.isActive = true;
            if (camController != null) { camController.followCar = false; camController.playerTarget = playerObject.transform; }
            MobileInput.ResetAll();
            OnStateChanged?.Invoke(false);
        }
        public void StartDriving()
        {
            isDriving = true;
            if (playerObject != null) playerObject.SetActive(false);
            if (onFoot != null) onFoot.isActive = false;
            if (car != null) car.isDrivingEnabled = true;
            if (camController != null) { camController.followCar = true; camController.carTarget = car.transform; }
        }
    }
}
