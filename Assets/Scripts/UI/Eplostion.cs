using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eplostion : MonoBehaviour

{

    [SerializeField]   float destroyTime;

    private void Start()
    {
       Destroy(this.gameObject, destroyTime);

    }

}
