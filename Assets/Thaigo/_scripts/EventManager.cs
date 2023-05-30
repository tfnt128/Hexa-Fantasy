using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();

                if (instance == null)
                {
                    Debug.LogError("O objeto EventManager não está presente na cena.");
                }
            }

            return instance;
        }
    }

    public delegate void ActivateBattle(bool activate);
    public event ActivateBattle ActivatedBattle;


    public void ActivateBattleCam()
    {
        if (ActivatedBattle != null)
        {
            ActivatedBattle(true);
        }
    }
    public void DeativateBattleCam()
    {
        if (ActivatedBattle != null)
        {
            ActivatedBattle(false);
        }
    }
}
