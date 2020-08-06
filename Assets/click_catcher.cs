using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click_catcher : MonoBehaviour
{
    // This one catches all the clicks to turn things off (hover info, active skill target, etc.)
    private void OnMouseDown()
    {

        // Stop actively targeting
        SkillTarget.instance.stop_targeting();

    }

}
