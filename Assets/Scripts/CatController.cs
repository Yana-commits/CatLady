using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatController : CreatureCollector<Cat>
{
    private List<int> freeCats = new List<int>();

    public Action<int> NotLookForCat;
    public Action OnGameOver;
    public override void Initialise()
    {
        base.Initialise();

        for(var i =0; i< creatures.Count; i++)
        {
            creatures[i].Id = i;
            creatures[i].SetCat();
            creatures[i].OnPickedUp += CheckCat;
        }
    }

    public Cat FindFreeCat()
    {
        var freeCat = creatures.Where(x => x.isPicked == false).FirstOrDefault();

        if (freeCat != null)
           freeCats.Add(freeCat.Id);

        return freeCat;
    }
    public void CheckCat(int id, int enemyId)
    {
        var freeCat = creatures.Where(x => x.isPicked == false).FirstOrDefault();

        if (freeCat == null)
        {
            OnGameOver?.Invoke();
            Debug.Log("GameOver");
        }
        else
        {
            freeCats.Contains(id);
            freeCats.Remove(id);
            NotLookForCat?.Invoke(enemyId);
        }
    }
}
