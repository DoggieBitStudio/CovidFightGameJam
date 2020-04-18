using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterOrthController : MonoBehaviour
{
    UIManager uiManager;

    NavMeshAgent agent;
    NavMeshPath path;
    Animator animator;
    public float distance = 50f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FixedUpdate()
    {
#if UNITY_STANDALONE_WIN
        StandaloneInput();
#endif

#if UNITY_ANDROID
        PhoneInput();
#endif
    }

    bool IsWalkable(Vector3 point)
    {
        return NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path);
    }

    void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void StandaloneInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                if (IsWalkable(hit.point) && !uiManager.isTaskMenuOpen)
                    MoveToDestination(hit.point);
                if (hit.collider.CompareTag("Selectable") && !uiManager.isTaskMenuOpen && !GameManager.instance.ui_opened)
                {
                    hit.collider.gameObject.GetComponent<Interactable>().OnTap(hit.point);
                    MoveToDestination(hit.point);
                }
            }
        }
    }
    void PhoneInput()
    {
        if (Input.touchCount > 0)
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                if (IsWalkable(hit.point) && !uiManager.isTaskMenuOpen)
                    MoveToDestination(hit.point);
                if (hit.collider.CompareTag("Selectable") && !uiManager.isTaskMenuOpen)
                {
                    hit.collider.gameObject.GetComponent<Interactable>().OnTap(hit.point);
                    MoveToDestination(hit.point);
                }
            }
        }
    }
}
