using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Camera _mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = GameObject.Find("First Person Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCamera.transform.position;
        cameraPosition.y = transform.position.y;

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(cameraPosition - transform.position);
        targetRotation *= Quaternion.Euler(0f, 180f, 0f); // Flip the sprite

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
    }
}
