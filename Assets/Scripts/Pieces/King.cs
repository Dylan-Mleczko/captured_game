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
      rb.AddForce(new Vector3(100f, 0, 0));
      rb.velocity = (new Vector3(transform.position.x - player.transform.position.x, 0.0f, transform.position.z - player.transform.position.z)).normalized * speed;
      // Debug.Log(rb.velocity.ToString());
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.name == "Player")
    {
      rb.velocity = Vector3.zero;
    }
  }

  /*::: We use "FixedUpdate" and not "Update", because for the displacements of the player, the
"FixedUpdate" is better to calculate more frames :::*/
  private void FixedUpdate()
  {
    transform.Translate(Vector3.right * 4f * Time.deltaTime * Input.GetAxis("Horizontal"));
    if (Input.GetAxis("Vertical") != 0)
    {
      transform.Translate(Vector3.forward * 3f * Time.deltaTime * Input.GetAxis("Vertical"));
    }
    else
    {
      if (Input.GetAxis("Vertical") < 0)
      {
        transform.Translate(Vector3.forward * 3f * Time.deltaTime * Input.GetAxis("Vertical"));
      }
    }
  }

}