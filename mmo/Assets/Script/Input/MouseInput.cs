using UnityEngine;
using System.Collections;

public static class MouseInput{
    public static bool mouseCatchFlag { get; set; }

    static MouseInput()
    {
        mouseCatchFlag = false;
    }
}
