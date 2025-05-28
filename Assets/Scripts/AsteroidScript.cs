using System.Collections.Generic;
using UnityEngine;
using static BaseController;

public class Asteroid : MonoBehaviour
{
    public int totalMinerals;

    // Tracks if already sent a drone to asteroid
    private HashSet<string> claimedByFactions = new HashSet<string>();

    public bool CanBeClaimedBy(string faction)
    {
        return !claimedByFactions.Contains(faction);
    }

    public void Claim(string faction)
    {
        claimedByFactions.Add(faction);
    }

    public int Harvest(int amount, string faction)
    {
        int harvested = Mathf.Min(amount, totalMinerals);
        totalMinerals -= harvested;

        if (totalMinerals <= 0)
        {
            Destroy(gameObject, 0.5f); // destroy after 0.5 sec
        }
        else
        {
            // Allow this faction to harvest it again later
            claimedByFactions.Remove(faction);
        }

        return harvested;
    }
    private void OnDestroy()
    {
        if (AsteroidManager.Instance != null)
        {
            AsteroidManager.Instance.UnregisterAsteroid(this);
        }
    }
}
