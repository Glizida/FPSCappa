using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class PlayerControler : MonoBehaviour
{


	
	[SerializeField] public Text textAmmo;
	[SerializeField] public Text textAmmomax;
	[SerializeField] public Text ScoreInt;
	
	[SerializeField]
	public ButtonFixed fireButton;
	public float rotationSpeed = 1.5f;
	[SerializeField] public Text gTex;
	[SerializeField] public Button gButton;
	public float hp = 100;
	public float gravity;
	public float speed;
	private float maxCameraRotation = 30;
	[SerializeField]public Joystick joystick;
	[SerializeField] public Joystick rotationJoystick;
	[SerializeField] public ButtonFixed changeWeapon;
	private CharacterController chControler;
	[SerializeField] public BaseWeapon curentWeapon;
	[SerializeField] public BaseWeapon[] inventory;
	public bool isDead = false;
	private float rotationX = 0;
	private int idcurentWeapon = 0;
	private float switchWeapon = 1;
	private float switchWeaponDeltaTime = 0;
	private bool swithBool;
	public float Score;
	public float reactx, reacty;
	public Texture2D crosshair;
	private AudioSource ad;

	private float[] maxScore = new float[5]; 


	void Start()
	{
		ad = GetComponent<AudioSource>();
		chControler = GetComponent<CharacterController>();
		maxScore[0] = PlayerPrefs.GetFloat("0",0);
		maxScore[1] = PlayerPrefs.GetFloat("1",1);
		maxScore[2] = PlayerPrefs.GetFloat("2",2);
		maxScore[3] = PlayerPrefs.GetFloat("3",3);
		maxScore[4] = PlayerPrefs.GetFloat("4",4);
		ad.Play();
	}

	private void OnGUI()
	{
		textAmmo.text = curentWeapon.ammo.ToString();
		textAmmomax.text = curentWeapon.curentAmmo.ToString();
		GUI.DrawTexture(new Rect(Screen.width/2 - reactx/2, Screen.height/2 - reacty/2, reactx, reacty), crosshair);
		ScoreInt.text = Score.ToString();

	}

	public void AddDamage(float damage)
	{
		Debug.Log("AddDamage Player");
		if (isDead) return;
		hp -= damage;
		if (hp <= 0)
		{
			Array.Sort(maxScore);
			if (Score > maxScore[4])
			{
				maxScore[0] = Score;
				PlayerPrefs.SetFloat("0",maxScore[0]);
				PlayerPrefs.SetFloat("1",maxScore[1]);
				PlayerPrefs.SetFloat("2",maxScore[2]);
				PlayerPrefs.SetFloat("3",maxScore[3]);
				PlayerPrefs.SetFloat("4",maxScore[4]);
				PlayerPrefs.Save();
				
			}
			isDead = true;
		}
	}

	public void AddScore(float score)
	{
		Score += score;
		Debug.Log(Score);
	}
	
	void Update()
	{
		if (hp > 0)
		{
				if (joystick.Vertical > 0)
				{
					Vector3 move = transform.forward;
					float tempSpeed = speed * joystick.Vertical;
					chControler.Move(move * tempSpeed * Time.deltaTime);
				}

				if (joystick.Vertical < 0)
				{
					Vector3 move = -transform.forward;
					float tempSpeed = speed * -joystick.Vertical;
					chControler.Move(move * tempSpeed * Time.deltaTime);
				}

				if (joystick.Horizontal > 0)
				{
					Vector3 move = transform.right;
					float tempSpeed = speed * joystick.Horizontal;
					chControler.Move(move * tempSpeed * Time.deltaTime);
				}

				if (joystick.Horizontal < 0)
				{

					Vector3 move = -transform.right;
					float tempSpeed = speed * -joystick.Horizontal;
					chControler.Move(move * tempSpeed * Time.deltaTime);
				}

				if (rotationJoystick.Horizontal > 0 || rotationJoystick.Horizontal < 0)
				{
					transform.Rotate(0f, rotationSpeed * rotationJoystick.Horizontal, 0f);
				}

				if (rotationJoystick.Vertical > 0 || rotationJoystick.Vertical < 0)
				{
					rotationX -= rotationJoystick.Vertical * rotationSpeed;
					rotationX = Mathf.Clamp(rotationX, -maxCameraRotation, maxCameraRotation);
					Camera.main.transform.localEulerAngles = new Vector3(rotationX, 0, 0);

				}

				Vector3 grav = Vector3.down;
				chControler.Move(grav * gravity * Time.deltaTime);

				if (fireButton.Pressed)
				{
					if (!curentWeapon.audioSource.isPlaying)
					{
						curentWeapon.audioSource.Play();
					}

					curentWeapon.Fire();
				}

				if (switchWeaponDeltaTime > switchWeapon)
				{
					swithBool = true;
				}
				else
				{
					switchWeaponDeltaTime += Time.deltaTime;

				}



				if (changeWeapon.Pressed && !curentWeapon.isReload)
				{
					if (swithBool)
					{

						curentWeapon.enabled = false;
						curentWeapon.gameObject.SetActive(false);
						if (idcurentWeapon == inventory.Length - 1)
						{
							idcurentWeapon = 0;
							curentWeapon = inventory[idcurentWeapon];
							curentWeapon.gameObject.SetActive(true);
							curentWeapon.enabled = true;
							switchWeaponDeltaTime = 0f;
						}
						else
						{

							idcurentWeapon++;
							curentWeapon = inventory[idcurentWeapon];
							curentWeapon.gameObject.SetActive(true);
							curentWeapon.enabled = true;
							switchWeaponDeltaTime = 0f;
						}
					}

					swithBool = false;
					switchWeaponDeltaTime = 0f;
				}
		}	
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ammo")
		{
			switch (curentWeapon._TypeAmmo)
			{
					case BaseWeapon.TypeAmmo.Ak74:
							curentWeapon.AddAmmo(other.gameObject.GetComponent<AmmoBox>().ammoAK47);
						break;
					case BaseWeapon.TypeAmmo.Glock:
						curentWeapon.AddAmmo(other.gameObject.GetComponent<AmmoBox>().ammoGlok);
						break;
					case BaseWeapon.TypeAmmo.M4A1:
						curentWeapon.AddAmmo(other.gameObject.GetComponent<AmmoBox>().ammoM4A1);
						break;
			}
			Destroy(other.gameObject);
		}
	}
}
