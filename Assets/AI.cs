using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    // declarações
    public Transform player;
    public Transform bulletSpawn;
    public Slider healthBar;
    public GameObject bulletPrefab;

    NavMeshAgent agent; // nav mesh do agente
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f; // vida do agente
    float rotSpeed = 5.0f; // rotação dele

    float visibleRange = 80.0f; // visibilidade do destino dele
    float shotRange = 40.0f; // força do tiro

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>(); // chamando o navmesh
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth", 5, 0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position); // colocando a barra de vida no game
        healthBar.value = (int)health; // valor da barra de vida
        healthBar.transform.position = healthBarPos + new Vector3(0, 60, 0); // posicionamento dela
    }

    void UpdateHealth()
    {
        if (health < 100) // vida total
            health++;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet") // tiro do personagem
        {
            health -= 10; // tira vida 
        }
    }

    [Task] // task de referencia para puglin
    public void PickRandomDestination() // metodo para movimentação de um ponto para o outro
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)); // movimentação do agente
        agent.SetDestination(dest); // destino do agente
        Task.current.Succeed();
    }

    [Task]
    public void MoveToDestination() // timer para medição de onde ele chega
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time); // timer de medição
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) // distancia do agente virtual
        {
            Task.current.Succeed();
        }
    }
}

