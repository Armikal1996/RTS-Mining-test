using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour
{
    [Header("Drone Stats")]
    public float speed = 2f;
    public int maximumCarryingCapacity = 5;
    public string faction;

    public int currentCarryingCapacity = 1;

    private Asteroid currentTarget;
    private Vector3 targetPosition;
    public bool returningToBase = false;
    private BaseController baseController;
    private int mineralsCarried = 0;

    private Coroutine moveRoutine;

    public void Initialize(string factionTag, BaseController baseBase)
    {
        faction = factionTag;
        baseController = baseBase;
        currentCarryingCapacity = 1;
        FindTargetAsteroid();
    }

    private void StartMovement(Vector3 destination)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveToTarget(destination));
    }

    private IEnumerator MoveToTarget(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            Vector3 direction = destination - transform.position;

            // === Avoid System start ===
            Vector3 avoidance = Vector3.zero;
            Collider2D[] nearbyDrones = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Drone"));
            foreach (var drone in nearbyDrones)
            {
                if (drone.gameObject != this.gameObject)
                {
                    Vector3 awayFromOther = transform.position - drone.transform.position;
                    avoidance += awayFromOther.normalized / awayFromOther.magnitude; // Weight by distance
                }
            }
            direction += avoidance * 0.5f; 
            // === Avoid System finish ===

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction.normalized, speed * Time.deltaTime);

            yield return null;
        }
        // === After reaching destination ===
        yield return new WaitForSeconds(0.1f); // small delay

        if (!returningToBase && currentTarget == null)
        {
            Debug.Log("asdasd");
            // if asteroid isn't there, fly to next
            currentTarget = null;
            FindTargetAsteroid();
        }
    }

    private void FindTargetAsteroid()
    {
        currentTarget = AsteroidManager.Instance.GetAvailableAsteroid(faction, baseController.transform.position);

        if (currentTarget != null)
        {
            currentTarget.Claim(faction);
            targetPosition = currentTarget.transform.position;
            StartMovement(targetPosition);
        }
        else
        {
            Invoke(nameof(FindTargetAsteroid), 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!returningToBase && other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null && asteroid == currentTarget)
            {
                mineralsCarried = asteroid.Harvest(currentCarryingCapacity, faction);

                returningToBase = true;
                targetPosition = baseController.transform.position;
                StartMovement(targetPosition);
            }
        }

        if (returningToBase && other.CompareTag("Planet"))
        {
            BaseController baseHit = other.GetComponent<BaseController>();

            if (baseHit != null && baseHit.faction.ToString() == faction)
            {
                baseHit.AddStrength(mineralsCarried);
                mineralsCarried = 0;
                returningToBase = false;

                if (currentCarryingCapacity < maximumCarryingCapacity)
                {
                    //currentCarryingCapacity++; // uncomment if you want more capacity after each harvest
                }

                FindTargetAsteroid();
            }
        }
    }
    public void SetDroneSpeed(int newSpeed)
    {
        speed = newSpeed;
    }
    private void OnDestroy()
    {
        if (baseController != null)
        {
            baseController.RemoveDrone(this);
        }
    }
}
