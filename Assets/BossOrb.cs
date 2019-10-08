using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrb : MonoBehaviour
{

    [SerializeField] public float orbHealth;
    private BossAI bossAI;

    private void Awake()
    {
        bossAI = GetComponentInParent<BossAI>();
    }

    public void DamageOrb(float damage)
    {
        bossAI.DamageOrb(damage,transform);
    }

}
