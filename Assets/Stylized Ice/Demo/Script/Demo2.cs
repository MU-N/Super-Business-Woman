using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2 : MonoBehaviour 
{
	public float speed = 10;

	private void Update()
	{
		transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
	}
}
