using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{

  [SerializeField] float range;
  [SerializeField] float speed;
  int direction = 1;

  void Start()
  {

  }

  void Update()
  {
    Move();
  }

  public void UpdateRange(float range)
  {
    this.range = range;

  }

  void Move()
  {
    if (transform.localPosition.z > range)
    {
      direction = -1;
    }
    else if (transform.localPosition.z < -range)
    {
      direction = 1;
    }
    transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);
  }

}