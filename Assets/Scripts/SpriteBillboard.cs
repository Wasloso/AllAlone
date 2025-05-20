using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera)
        {
            var angle = _camera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(_camera.transform.rotation.eulerAngles.x, angle, 0f);
        }
    }
}