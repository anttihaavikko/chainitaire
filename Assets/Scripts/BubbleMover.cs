using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMover : MonoBehaviour
{
    public Dude dude;

    public void Dodge()
    {
        dude.MirrorBubble();
    }
}
