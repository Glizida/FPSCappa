using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public float curentHp = 100f;
	public float damage = 5f;
	private GameObject player;
	public Animator animator;
	private NavMeshAgent navMeshAgent;
	private bool isDead = false;
	private PlayerControler pc;
	public bool isAttack = false;
	private int IgnoreLayer = ~(1 << 10);
	[SerializeField] public BoxCollider boxCollider;
	[SerializeField] public GameObject AmmoSpawm;
	public float score;
	
	
	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = gameObject.GetComponent<Animator>();
		player = GameObject.FindWithTag("Player");
		pc = player.GetComponent<PlayerControler>();
		boxCollider.enabled = false;
	}

	private void Update()
	{
		if (!isDead && pc.hp > 0)
		{
			if (!isAttack)
			{
				navMeshAgent.enabled = true;
				navMeshAgent.SetDestination(player.transform.position);
				animator.SetFloat("Speed", 1);
				
			}
		}
		
	}

	public void AddDamage(float damage)
	{
		if (isDead) return;
		
		curentHp -= damage;
		if (curentHp < 0)
		{
			navMeshAgent.enabled = false;
			isDead = true;
			animator.SetBool("Dead",true);
			if (Random.value < 0.2)
			{
				Instantiate(AmmoSpawm,transform.position, Quaternion.identity);		
			}
			pc.AddScore(score);
			Destroy(gameObject,3);
		}
	}

	

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			isAttack = true;
			animator.SetBool("IsAtack", true);
			navMeshAgent.enabled = false;
		}
	}

	public void EndAnimAttack()
	{
		
		animator.SetBool("IsAtack", false);
		isAttack = false;
	}

	public void StartAttack()
	{
		boxCollider.enabled = true;
	}

	public void EndAttack()
	{
		boxCollider.enabled = false;
	}
	
	
}
