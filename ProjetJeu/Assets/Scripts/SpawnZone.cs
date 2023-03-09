using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] 
    public GameObject zombi;

    [SerializeField]
    public Vector3 size;

    public bool spawner = true; //Pour tempo entre chaque spawn

    public float safeDistance; //Distacne minimum entre les bots et le joueur

    public int nombreZombie;
    
    public Transform target;

    public void Update()
    {
        if (spawner && nombreZombie > 0)
        {
            StartCoroutine(Spawn());
            nombreZombie -= 1;
        }
    }


    IEnumerator Spawn()
    {
        Vector3 position = target.position;
        spawner = false;
        GameObject zombiePrefab = Instantiate(zombi); //Spawn un zombi à un endroit aléatoire
        float x = Random.Range(transform.position.x - size.x / 2, transform.position.x + size.x / 2);
        float z = Random.Range(transform.position.z - size.z / 2, transform.position.z + size.z / 2);
        while (position.x - x < safeDistance && position.x - x > -safeDistance && position.z - z < safeDistance && position.z - z > -safeDistance)
        {
            x = Random.Range(transform.position.x - size.x / 2, transform.position.x + size.x / 2);//Créée de nouvelles coordonnées tant que le zombie est dans la Safe Zone
            z = Random.Range(transform.position.z - size.z / 2, transform.position.z + size.z / 2);
        }
        zombiePrefab.transform.position = new Vector3(x,Random.Range(transform.position.y - size.y/2, transform.position.y + size.y/2),z);
        yield return new WaitForSeconds(4);
        spawner = true;
    }

    private void OnDrawGizmos() //Affiche dans la scene d'edit la zone de spawn
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
