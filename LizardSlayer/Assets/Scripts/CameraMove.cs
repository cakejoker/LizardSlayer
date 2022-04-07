using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector2 velocity;

    private float smoothTimeX;
    private float smoothTimeY;

    public GameObject player;

    public bool bounds;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    public static CameraMove instance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if(bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
