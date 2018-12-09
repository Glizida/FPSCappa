using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

	[SerializeField] public Text ScoreList1;
	[SerializeField] public Text ScoreList2;
	[SerializeField] public Text ScoreList3;
	[SerializeField] public Text ScoreList4;
	[SerializeField] public Text ScoreList5;
	private AudioSource audioSource;
	
		
	// Use this for initialization
	void Start () {
		
		ScoreList1.text = PlayerPrefs.GetFloat("0",0).ToString();
		ScoreList2.text = PlayerPrefs.GetFloat("1",0).ToString();
		ScoreList3.text = PlayerPrefs.GetFloat("2",0).ToString();
		ScoreList4.text = PlayerPrefs.GetFloat("3",0).ToString();
		ScoreList5.text = PlayerPrefs.GetFloat("4",0).ToString();

		audioSource = GetComponent<AudioSource>();
		audioSource.Play();
	}
	
	public void PlayPressed()
	{
		SceneManager.LoadScene("Level");
	}
	
	public void ExitPressed()
	{
		Application.Quit();
		Debug.Log("Exit pressed!");
	}
}