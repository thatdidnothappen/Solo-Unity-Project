using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    PlayerController player;

    public int health = 3;
    public int maxHealth = 5;
    public bool isFollowing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
     if (isFollowing)
        agent.destination = GameObject.Find("Player").transform.position;
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "proj")
        {
            health--;
            Destroy(collision.gameObject);
        }
    }

}

