using ActiveStates;
using SpellCasting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathComponent : MonoBehaviour
{
    //jam uh shouldn't be here lol
    public static List<DeathComponent> Players = new List<DeathComponent>();

    private void OnEnable()
    {
        if (gameObject.CompareTag("Player"))
        {
            Players.Add(this);
        }

    }
    private void OnDestroy()
    {
        if (gameObject.CompareTag("Player"))
        {
            Players.Remove(this);

            if(Players.Count <= 0 && !LevelProgressionManager.resetting)
            {
                SpellCasting.UI.ConfirmPopup.Open("Game Over!", "return", "", () =>
                {
                    LevelProgressionManager.DelayedTrueReset();
                });
            }
        }
    }

    [SerializeField]
    private SpawnTable deathTable;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private EffectPooled deathEffectPooled;

    [SerializeField]
    private StateMachineLocator stateMachineLocator;

    [SerializeField]
    private float deathDelay;

    private bool _gotRektLololol;

    public void GetRektLol()
    {
        if (_gotRektLololol)
            return;
        _gotRektLololol = true;

        if (deathTable)
        {
            deathTable.SpawnObject(transform.position);
        }
        if (deathEffect)
        {
            Object.Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        if (deathEffectPooled)
        {
            EffectManager.SpawnEffect(deathEffectPooled.effectIndex, transform.position, Quaternion.identity);
        }

        if (stateMachineLocator)
        {
            stateMachineLocator.SetStates(new DedState());
        }

        //EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, Random.Range(0, 10));

        if(deathDelay > 0)
        {
            StartCoroutine(DelayedDie());
        }else
        {
            Object.Destroy(gameObject);
        }
    }

    IEnumerator DelayedDie()
    {
        yield return new WaitForSeconds(deathDelay);
        Object.Destroy(gameObject);
    }
}