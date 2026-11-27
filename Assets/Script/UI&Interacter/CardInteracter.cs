using System;
using UnityEngine;

public class CardInteracter : MonoBehaviour
{
    public static event Action<int> CardInteracted;
    public int CardID = -1; // sau nay tao cac khoa cam trung lap cho moi cardID và check sau.
    private void Awake()
    {
        if (CardID == -1)
        {
            Debug.LogWarning("CardID not valid");
        }
        c = 1;
    }
    private int c; // testing here
    public void OnInteract()
    {
        c--;
        CardInteracted?.Invoke(CardID);
        if(c == 0) transform.parent.gameObject.SetActive(false);
        Debug.Log("CardInteracted");
    }
}
