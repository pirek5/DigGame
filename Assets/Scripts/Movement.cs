using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
public class Movement : MonoBehaviour
{
    //config
    [SerializeField] private float movementSpeed = 1f; //units per second

    //cached
    private Pathfinder pathfinder;
    private List<Tile> currentPath;

    //boolean flag
    private bool nextStep; 

    void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    public void FindAndFollowPath(Tile destination)
    {
        var closestTile = GridData.FindClosestTile(transform.position);
        currentPath = pathfinder.FindPath(closestTile, destination);
        StartCoroutine(FollowPath(currentPath));
    }

    private IEnumerator FollowPath(List<Tile> path)
    {
        foreach(var tile in path)
        {
            //nextStep = false;
            yield return StartCoroutine(SmoothMovement(tile.position));
            //SmoothMovement(tile.position);
            //while (!nextStep)
            //{
              //  yield return null;
            //}
        }
    }

    private IEnumerator SmoothMovement(Vector3 destination)
    {
        float distance = Vector3.Distance(transform.position, destination);
        var currentPosition = transform.position;
        var fractionOfJourney = 0f;
        while (fractionOfJourney < 1)
        {
            fractionOfJourney += Time.deltaTime * movementSpeed / distance;
            transform.position = Vector3.Lerp(currentPosition, destination, fractionOfJourney);
            yield return null;
        }
        nextStep = true;
    }
}
