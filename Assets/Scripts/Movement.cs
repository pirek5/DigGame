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

    //state
    public Tile CurrentTile { get; private set; }

    protected virtual void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    private void Start()
    {
        List<Tile> tiles = new List<Tile>(GridData.GridDictionary.Values);
        CurrentTile = Utilities.FindClosestTile(transform.position, tiles);
    }

    public virtual void MoveToPosition(Tile destinationTile)
    {
        FindAndFollowPath(destinationTile); // TODO zaznacz nieudane wyszukanie ściezki FIndAndFollowPath to bool
    }

    public bool FindAndFollowPath(Tile destination)
    {
        StopAllCoroutines();
        currentPath = pathfinder.FindPath(destination);
        if(currentPath == null) { return false; }
        StartCoroutine(FollowPath(currentPath));
        return true;
    }

    protected virtual IEnumerator FollowPath(List<Tile> path)
    {
        foreach(var tile in path)
        {
            yield return StartCoroutine(SmoothMovement(tile.Position));
            CurrentTile = tile;
        }
    }

    protected IEnumerator SmoothMovement(Vector3 destination)
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
