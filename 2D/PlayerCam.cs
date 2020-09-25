using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    public Transform player;
    private Vector3 offset = new Vector3(0f, 0f, -15f);


    void LateUpdate() {
        transform.position = player.position + offset;
    }
}
