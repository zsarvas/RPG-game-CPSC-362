using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform player;
    public float rotation = 10f;
    public float yoff = 2.28f;
    public float zoff = -5.95f;

    private GameObject playerObj;
    private Vector3 offset;

    void Start() {
        if (player == null) {
            playerObj = GameObject.Find("PlayerQuad");
            if (playerObj == null) {
                Debug.Log("Error: Camera's default object or set object not found.");
                return;
            }
            player = playerObj.GetComponent<Transform>();
        }
        offset = new Vector3(0f, yoff, zoff);
        transform.Rotate(rotation, 0, 0);
    }

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
