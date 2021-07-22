using System.Linq;
using UnityEngine;
using SplineMesh;

public class TentacleWithBoniesBuilder : MonoBehaviour
{
    [SerializeField] private Transform _segmentsParent;
    [SerializeField] private TentacleWithBone _tentacle;

    private Spline _spline;

    private void OnEnable()
    {
        GlobalEventStorage.GameOvering += BuildTentacle;
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameOvering -= BuildTentacle;
    }

    private void Start()
    {
        _spline = GetComponentInParent<Spline>();
    }

    private void BuildTentacle(bool isWin)
    {
        if (!isWin)
        {
            GameObject[] segments = _segmentsParent.GetComponentsInChildren<TentacleSegment>().Select(segment => segment.gameObject).ToArray();
            var positions = _spline.nodes.Select(node => node.Position).ToArray();

            if (segments.Length <= _tentacle.BoneCount)
                GameObjectsSetActive(false, segments);
            else if (segments.Length > _tentacle.BoneCount)
            {
                var orderSegments = segments.Reverse().ToArray();

                GameObjectsSetActive(false, orderSegments.Take(_tentacle.BoneCount).ToArray());
            }

            _tentacle.ShowTentacle(_spline);
        }
    }

    private void GameObjectsSetActive(bool isActive, params GameObject[] gameObjects)
    {
        foreach (var gameObject in gameObjects)
            gameObject.SetActive(isActive);
    }
}
