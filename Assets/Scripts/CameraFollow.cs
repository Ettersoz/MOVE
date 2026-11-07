using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    // roughly how long in seconds the camera takes to catch up 
    // 0 = loscked to the player
    [SerializeField] private float smoothTime = 0.15f;

    // shifts the vew realitve to the plaer, e.g. (0, 1) 
    // shows a big more above the player than below.
    [SerializeField] private Vector2 offset = new Vector2(0f, 1f);

    // smoothDamp needs to remember the camera's current speed between
    // frames, so we should send a reference to it. 
    private Vector3 velocity = Vector3.zero;

    // late update runs after every update this frame, so the player has 
    // already moved. following update instead can make the camera lag a 
    // frame behind the player and look jittery.  
    void LateUpdate()
    {
        if (target == null) return;

        // where we want the camera: the players position plus our offset 
        // z stays at the camera's own Z (-10). In 2d, the camera has to 
        // sit infront of the scene on the Z axis, or it sees nothing. 

        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            1,
            transform.position.z);

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
