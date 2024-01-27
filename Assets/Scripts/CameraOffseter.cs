using UnityEngine;

public class CameraOffseter : MonoBehaviour
{
    private float yOffset;

    public Chaser chaser;
    public Transform player;

    
    private void Start()
    {
        yOffset = transform.position.y - player.transform.position.y;
    }

    private void Update()
    {
        //transform.localPosition = Vector3.zero;
        var transform1 = transform;
        transform1.rotation = Quaternion.identity;
        var currentPosition = transform1.position;
        //float targetX = Mathf.Clamp(transform.position.x, XMin, XMax);
        //float targetY = Mathf.Clamp(player.transform.position.y, Mathf.Max(YMin, chaser.transform.position.y + 7), YMax);
        transform1.position = new Vector3(currentPosition.x,
            Mathf.Max(player.transform.position.y, chaser.transform.position.y + yOffset+2) + yOffset, -10);
    }
}
