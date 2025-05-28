using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    public enum Faction { Red, Blue }
    public Faction faction;

    [Header("Base Stats")]
    public int strength = 0;
    public int droneCount = 0;

    [Header("Drone Settings")]
    [SerializeField] private GameObject dronePrefab;
    public float droneSpawnDelay = 0.3f;
    [SerializeField] private Transform droneSpawnPoint;


    private List<DroneController> activeDrones = new List<DroneController>();
    private GameplayUIHandler uiHandler;
    private int dronesToSpawn = 2;
    private int droneSpeed = 2;

    void Awake()
    {
        uiHandler = FindFirstObjectByType<GameplayUIHandler>();
        if (droneSpawnPoint == null)
            droneSpawnPoint = this.transform;
    }
   
    public void SetDroneSettings(int newTargetCount, int newSpeed)
    {
        dronesToSpawn = newTargetCount;
        droneSpeed = newSpeed;

        // update speed 
        foreach (var drone in activeDrones)
        {
            Debug.Log(activeDrones.Count);
            if (drone != null)
                drone.SetDroneSpeed(droneSpeed);
        }

        AdjustDroneCount();
    }
    private void AdjustDroneCount()
    {
        int difference = dronesToSpawn - activeDrones.Count;
        if (difference > 0)
        {
            // spawn more drones
            for (int i = 0; i < difference; i++)
            {
                Vector3 randomOffset = Random.insideUnitCircle * 0.2f;

                var droneObj = Instantiate(dronePrefab, droneSpawnPoint.position + new Vector3(randomOffset.x, 0, randomOffset.y), Quaternion.identity);
                var drone = droneObj.GetComponent<DroneController>();

                drone.SetDroneSpeed(droneSpeed);
                drone.Initialize(faction.ToString(), this);

                activeDrones.Add(drone);
            }
        }
        else if (difference < 0)
        {
            // destroy farthest drone first
            for (int i = 0; i < -difference; i++)
            {
                DroneController farthestDrone = GetFarthestDrone();
                Debug.Log(farthestDrone + " : fartherst drone");
                if (farthestDrone != null)
                {
                    activeDrones.Remove(farthestDrone);
                    Destroy(farthestDrone.gameObject);
                    Debug.Log("drone destroyed");
                }
            }
        }
        dronesToSpawn = activeDrones.Count;
    }
    private DroneController GetFarthestDrone()
    {
        if (activeDrones.Count == 0) return null;

        DroneController farthest = null;
        float maxDistance = 0f;

        foreach (var drone in activeDrones)
        {
            if (drone == null) continue;

            float dist = Vector3.Distance(transform.position, drone.transform.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthest = drone;
            }
        }

        return farthest;
    }

    public void AddStrength(int amount)
    {
        strength += amount;
        uiHandler.UpdateBaseStrength();
    }
    public void RemoveDrone(DroneController drone)
    {
        if (activeDrones.Contains(drone))
            activeDrones.Remove(drone);
    }
}
