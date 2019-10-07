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

    void Update()
    {
        if (orbHealth <= 0)
        {
            bossAI.firstStage++;

            bossAI.orbTransforms.Remove(transform);
            if (bossAI.firstStage > BossAI.FirstStage.ThirdOrbDestroyed)
            {
                bossAI.bossStage = BossAI.BossStages.SecondStage;
            }

            Destroy(gameObject);
        }
    }
}
