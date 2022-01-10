using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Outputs a bool value if the loop is lasting too much, so it is considered an infinite loop
    /// </summary>
    /// <param name="seconds">Threshold time to check if it is on an infinite loop</param>
    /// <param name="shouldExit">Should the application exit from the loop or not</param>
    public static void ExitFromInfiniteLoopAfterSeconds(float seconds, out bool shouldExit)
    {
        shouldExit = Time.timeSinceLevelLoad >= seconds;
    }
}
