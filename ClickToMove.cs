using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour 
{
    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Чекает лучом
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Проверяет попали ли луч
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                // Идет на места попадания
                navAgent.SetDestination(hit.point);
            }
        }
    }

}
