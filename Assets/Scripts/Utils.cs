using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils _utilInstance;

    private Camera _camera;

    private void Start ()
    {
        if (_utilInstance == null)
        {
            _utilInstance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        _camera = Camera.main;
    }


    public bool OutOfBounds(Vector2 position)
    {
        Vector2 viewPos = _camera.WorldToViewportPoint(position);

        if (viewPos.x < 1 && viewPos.x > 0 && viewPos.y < 1 && viewPos.y > 0)
        {
            return false;
        }

        return true;
    }
}
