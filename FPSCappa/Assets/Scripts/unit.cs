using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{

	public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics.Raycast(transform.position,player.transform.position))
		{
			Debug.DrawLine(transform.position,player.transform.position);
		}
	}
}
