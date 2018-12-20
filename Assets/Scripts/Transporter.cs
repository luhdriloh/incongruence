using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    private Vector2 _locationToGetTransportedTo;

    public void SetTransporterCoordinates(Vector2 location)
    {
        _locationToGetTransportedTo = location;
    }

    public Vector2 GetTransporterCoordinates()
    {
        return _locationToGetTransportedTo;
    }
}
