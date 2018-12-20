using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils _utilInstance;

    private Camera _camera;
    private float halfX, halfY;

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

        Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        halfX = -world.x;
        halfY = -world.y;

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
