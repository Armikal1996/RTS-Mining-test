using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public static AsteroidManager Instance;
    private List<Asteroid> asteroidList = new List<Asteroid>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterAsteroid(Asteroid asteroid)
    {
        asteroidList.Add(asteroid);
    }

    public void UnregisterAsteroid(Asteroid asteroid)
    {
        asteroidList.Remove(asteroid);
    }

    public Asteroid GetAvailableAsteroid(string faction, Vector3 originPosition)
    {
        Asteroid closest = null;
        float closestDistance = float.MaxValue;

        foreach (Asteroid asteroid in asteroidList)
        {
            if (asteroid != null && asteroid.CanBeClaimedBy(faction))
            {
                float distance = Vector3.Distance(originPosition, asteroid.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = asteroid;
                }
            }
        }

        return closest;
    }

}
