using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private int[] scores = new int[2];

    void Start()
    {
        scores[0] = 0;
        scores[1] = 0;
    }

    public void PlayerScored(int player)
    {
        scores[player]++;
        Debug.LogFormat("Player 1: {0} \n[00:00:00] Player 2: {1}", scores[0], scores[1]);
    }
}
