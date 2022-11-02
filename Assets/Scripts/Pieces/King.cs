using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{

  [SerializeField] float speed;
  Rigidbody rb;

  void Start()
  {
    rb = gameObject.GetComponent<Rigidbody>();
  }

  void Update()
  {

  }

  void OnTriggerStay(Collider collider)
  {
    if (collider.name == "Player")
    {
      GameObject player = collider.gameObject;
      rb.AddForce(new Vector3(1f, 0, 0));
      rb.velocity = (new Vector3(transform.position.x - player.transform.position.x, 0.0f, transform.position.z - player.transform.position.z)).normalized * speed;
      Debug.Log(rb.velocity.ToString());
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.name == "Player")
    {
      rb.velocity = Vector3.zero;
    }
  }

}