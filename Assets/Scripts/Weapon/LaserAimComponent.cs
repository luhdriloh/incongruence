using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{

	private void Start ()
    {
		
	}
	
	private void Update ()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get mouse position then rotate sniper dude
        Vector3 direction = target - transform.position;

    }
}
