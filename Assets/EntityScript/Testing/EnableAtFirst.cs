using System.Collections.Generic;
using UnityEngine;

public class EnableAtFirst : MonoBehaviour
{
    public List<GameObject> gameObjects;
    private void Awake()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(true);
        }
    }
}
