using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    static readonly Dictionary<float, WaitForSeconds> WaitForSeconds = new Dictionary<float, WaitForSeconds>();

    // Returns a WaitForSeconds object of specified duration from the dictionary.
    // Creates a new one if there is not already one with that duration.
    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (WaitForSeconds.TryGetValue(seconds, out var forSeconds)) return forSeconds;

        WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);
        WaitForSeconds.Add(seconds, waitForSeconds);

        return WaitForSeconds[seconds];
    }

    // Shuffles a given array and returns it.
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }


    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (!component) component = gameObject.AddComponent<T>();

        return component;
    }
}
