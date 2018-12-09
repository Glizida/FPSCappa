using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

	[SerializeField] public GameObject EnemySpawm;

	private float TimeSpawn = 0;

	public float StartTimeSpawn;

	public float MinTimeSpawn;
	// Use this for initialization
	void Start ()
	{
		Instantiate(EnemySpawm, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimeSpawn += Time.deltaTime;
		if (TimeSpawn > StartTimeSpawn)
		{		
			Instantiate(EnemySpawm, transform.position, Quaternion.identity);
			TimeSpawn = 0;
			if (StartTimeSpawn >MinTimeSpawn)
			{
				StartTimeSpawn -= 1;
			}
		}
	}
}
