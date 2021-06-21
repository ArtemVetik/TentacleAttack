using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleContainer : MonoBehaviour
{
    public event UnityAction<Obstacle> ObstacleActivated;

    private List<Obstacle> _allObstacles;

    private void Awake()
    {
        _allObstacles = FindObjectsOfType<Obstacle>().ToList();
    }

    private void OnEnable()
    {
        foreach (var obstacle in _allObstacles)
            obstacle.Activated += OnObstacleActivated;
    }

    private void OnDisable()
    {
        foreach (var obstacle in _allObstacles)
            obstacle.Activated -= OnObstacleActivated;
    }

    private void OnObstacleActivated(Obstacle obstacle)
    {
        ObstacleActivated?.Invoke(obstacle);
    }
}
