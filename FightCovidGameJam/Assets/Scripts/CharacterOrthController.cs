using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterOrthController : MonoBehaviour
{
    UIManager uiManager;

    NavMeshAgent agent;
    NavMeshPath path;
    public float distance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                if (IsWalkable(hit.point))
                    MoveToDestination(hit.point);
                if (hit.collider.CompareTag("Selectable") && !uiManager.isTaskMenuOpen)
                    hit.collider.gameObject.GetComponent<Interactable>().OnTap();
            }
        }
    }

    bool IsWalkable(Vector3 point)
    {
        return NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path);
    }

    void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
