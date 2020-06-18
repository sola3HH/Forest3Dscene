using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SimpleMove : MonoBehaviour
{
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;
    private Animator anim;

    private Transform player;

    CharacterController controller;
    float heading;
    Vector3 targetRotation;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Awake()
    {
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        //Change target rotation to a random angle.
        NewHeadingRoutine();



    }

    void Update()
    {
        //If the eular angle reach the target angle, then we should start for a new target angle.

        if (Vector3.Distance(transform.position, player.position) < 50f)
        {
            
            anim.SetBool("Walk", false);
            anim.SetBool("Run", true);
            //targetRotation = Quaternion.LookRotation(player.position - transform.position);
            //targetRotation = new Vector3(0, Quaternion.LookRotation(player.position-transform.position).eulerAngles.y, 0);
            //targetRotation =new Vector3( 0, Mathf.Atan((-player.position + transform.position).z/ (-player.position + transform.position).x), 0);
            //transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,  Time.deltaTime/15);
            transform.LookAt(player);
        }
        else
        {
            

            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
            //transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation ( targetRotation),  Time.deltaTime);
            

        }
        var forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * speed * 2);
        Debug.Log(targetRotation);
    }

    /// <summary>
    /// Repeatedly calculates a new direction to move towards.
    /// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
    /// </summary>
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// Calculates a new direction to move towards.
    /// </summary>
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
       
        targetRotation =new Vector3(0,heading,0); 
    }
}

