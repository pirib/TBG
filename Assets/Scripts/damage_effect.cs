using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_effect : MonoBehaviour
{

    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Done"))
            Destroy(this.gameObject);
    }


}
