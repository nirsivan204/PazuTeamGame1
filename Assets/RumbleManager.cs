using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{

    public static Gamepad[] Gamepads;
    static Gamepad pad;

    public static void Init()
    {
        Gamepads = Gamepad.all.ToArray();

    }

    public static void RumblePulse(bool isOn, int controllerIndex, float low, float high)
    {
        if (Gamepads.Length == 0)
        {
            Init();
        }
        pad = Gamepads[controllerIndex];
        if(pad != null)
        {
            pad.SetMotorSpeeds(isOn?low:0, isOn ? high:0);
        }
    }
}
