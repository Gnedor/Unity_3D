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

        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
    } 
}
