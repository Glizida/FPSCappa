using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
	public enum TypeAmmo
	{
		Glock = 1,
		Ak74 = 2,
		M4A1 = 3,
		}

	public TypeAmmo _TypeAmmo;
	private Camera camera;
	public float damage = 20f; //Урон за 1 выстрел
	public float range = 200f; // Дальность стрельбы
	public float shootSpeed = 0.5f; //Скорострельность
	public float shootTimeDelta = 0f;
	public int maxAmmo = 360; //Макс обойма
	public int curentAmmo; //Количество потронов
	public int maxAmmoCurent = 30; //Максимальное количество потронов в обойме
	public int ammo = 0; //Количество потронов в обойме
	public bool isReload = false;
	public float reloadTime = 3f;
	[SerializeField]
	public GameObject particle;
	protected ParticleSystem gunParticle;
	public AudioSource audioSource;

	public void Fire()
	{
		if (!isReload)
		{
			if ((ammo > 0) && (shootTimeDelta > shootSpeed))
			{
		
				gunParticle.Stop();
				gunParticle.Simulate(1);
				gunParticle.Play();
				Vector3 point = new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2, 0f);
				Ray ray = camera.ScreenPointToRay(point);
				Vector3 temp = ray.direction;
				ray.direction = temp;
				RaycastHit hit;
				gunParticle.Stop();
				
				if (Physics.Raycast(ray, out hit))
				{
					Debug.DrawLine(ray.origin,hit.point,Color.red,10);
					if (hit.collider.gameObject.tag == "Enemy")
					{
						
						hit.collider.gameObject.GetComponent<Enemy>().AddDamage(damage);

					}
				} 

				shootTimeDelta = 0f;
				ammo -= 1;
				
			} else if (ammo == 0 && !isReload) StartCoroutine(Reload());

			
		} 
			
	}

	private void Awake()
	{
		gunParticle = particle.GetComponent<ParticleSystem>();
		camera = Camera.main;
		gunParticle.Stop();
		audioSource = GetComponent<AudioSource>();

	}

	public void AddBullets(int count)
	{
		if ((curentAmmo + count) >= maxAmmo) curentAmmo = maxAmmo;
		else curentAmmo += count;
	}

	private void Update()
	{
		if (shootTimeDelta < shootSpeed)
		{
			shootTimeDelta += Time.deltaTime;
		}
		
	}

	public void StartReload()
	{
		if (!isReload)
		{
			StartCoroutine(Reload());
		}
		
	}
	public IEnumerator Reload()
	{
		if (ammo > 0 && ammo < maxAmmoCurent && curentAmmo > 0)
		{
			int needbullet = maxAmmoCurent - ammo;
			if (curentAmmo >= needbullet)
			{
				isReload = true;
				yield return new WaitForSeconds(reloadTime);
				ammo += needbullet;
				curentAmmo -= needbullet;
			}
			else
			{
				isReload = true;
				yield return new WaitForSeconds(reloadTime);
				ammo += curentAmmo;
				curentAmmo = 0;
			}
		}
		if (curentAmmo > 0 && curentAmmo >= maxAmmoCurent && ammo == 0)
		{
			isReload = true;
			yield return new WaitForSeconds(reloadTime);
			ammo = maxAmmoCurent;
			curentAmmo -= maxAmmoCurent;
		} else if (curentAmmo > 0 && curentAmmo < maxAmmoCurent && ammo == 0)
		{
			isReload = true;
			yield return new WaitForSeconds(reloadTime);
			ammo = curentAmmo;
			curentAmmo -= curentAmmo;
		}
		
		isReload = false;
	}

	public void AddAmmo(int ammoo)
	{
		if (curentAmmo + ammoo < maxAmmo)
		{
			curentAmmo += ammoo;
		}
		else
		{
			curentAmmo = maxAmmo;
		}
	}
}
