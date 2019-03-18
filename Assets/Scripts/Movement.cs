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

    void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    public bool FindAndFollowPath(Tile destination)
    {
        StopAllCoroutines();
        var startTile = GridData.FindStartTile(transform.position);
        if(startTile == null) { return false; }
        currentPath = pathfinder.FindPath(startTile, destination);
        if(currentPath == null) { return false; }
        StartCoroutine(FollowPath(currentPath));
        return true;
    }

    private IEnumerator FollowPath(List<Tile> path)
    {
        foreach(var tile in path)
        {
            yield return StartCoroutine(SmoothMovement(tile.position));
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
    }
}
