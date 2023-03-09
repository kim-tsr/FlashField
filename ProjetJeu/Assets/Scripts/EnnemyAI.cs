using System.Collections;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{
    public Transform target; 
    public UnityEngine.AI.NavMeshAgent bot;
    public float range = 3f; //Porté des dégats
    public float distance;
    public float damage;
    public float life = 100f;
    public bool canShoot = true;//Sert à la coroutine
    public bool canMoov = true;

    public void Start()
    {
        bot = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Update()
    {
        if (life <= 0)
        {
            gameObject.SetActive(false);
        }
        if(canMoov)
            bot.destination = target.position; //Suit le joueur
        
        distance = Vector3.Distance(target.position, transform.position);
        if (distance < range && canShoot)
        {
            StartCoroutine(Hit());
        }
    }

    //Coroutine
    IEnumerator Hit()
    {
        target.GetComponent<PlayerLife>().life -= damage; //Applique des dégats si le joueur est dans la range
        canShoot = false;
        canMoov = false;
        bot.destination = transform.position;
        yield return new WaitForSeconds(2);//Tempo pour pas spam
        canMoov = true;
        canShoot = true;
    }
}