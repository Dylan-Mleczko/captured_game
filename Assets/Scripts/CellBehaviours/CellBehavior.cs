using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehavior : MonoBehaviour
{

  public GameObject[] walls; // 0 - A, 1 - B, 2 - C, 3 - D 

  public void UpdateCell(bool[] status)
  {
    for (int i = 0; i < status.Length; i++)
    {
      walls[i].SetActive(status[i]);
    }
  }


}