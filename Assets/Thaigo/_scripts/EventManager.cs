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

    public delegate void ActivateBattle(bool activate, int id);
    public event ActivateBattle ActivatedBattle;


    public void ActivateBattleCam(int id)
    {
        if (ActivatedBattle != null)
        {
            ActivatedBattle(true, id);
        }
    }
    public void DeativateBattleCam(int id)
    {
        if (ActivatedBattle != null)
        {
            ActivatedBattle(false, id);
        }
    }
}
