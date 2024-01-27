using UnityEngine;

public class AddEdgeToChild : MonoBehaviour
{
    [SerializeField] private GameObject _edge;
    [SerializeField] private bool _edgeMeBaby;

    private void OnValidate()
    {
        if (_edgeMeBaby)
        {
            var instantiate = Instantiate(_edge, transform);
            instantiate.name = "Top Edge Collider";
        }
    }
}