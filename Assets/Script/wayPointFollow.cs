using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPointFollow : MonoBehaviour
{
    public Transform wayPoint1, wayPoint2;
    public int speed;
    Vector2 targetPos;


    void Start()
    {
        targetPos = wayPoint2.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, wayPoint1.position) < 0.1f)
            targetPos = wayPoint2.position;

        if (Vector2.Distance(transform.position, wayPoint2.position) < 0.1f)
            targetPos = wayPoint1.position;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    void OnDrawGizmosLine()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wayPoint1.position, wayPoint2.position);
        Debug.Log("Enter in gizmos");
    }
}
