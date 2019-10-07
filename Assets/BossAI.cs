﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    [Header("First Stage variables")]
    [SerializeField] private float firstStageMovementSpeed;
    [SerializeField] private float firstStageBulletSpeed;

    [Header("bullet Amount variables")]
    [Space]

    [SerializeField] private int noOrbBulletAmount;
    [SerializeField] private int firstOrbBulletAmount;
    [SerializeField] private int secondOrbBulletAmount;
    [SerializeField] private int thirdOrbBulletAmount;
    [SerializeField] private int thirdOrbHomingBulletAmount;

    [Header("bullet firerate variables")]
    [Space]

    [SerializeField] private float firstThreeOrbsBulletFirerate;
    [SerializeField] private float thirdOrbBulletFirerate;
    [SerializeField] private float thirdOrbHomingBulletFirerate;

    [Header("Second Stage variables")]
    [SerializeField] private float secondStageMovementSpeed;

    [Space]
    [Space]

    [SerializeField] private int secondStageMinionSpawnAmount;
    [SerializeField] private float secondStageMinionSpawnCastTime;
    [SerializeField] private float secondStageMinionSpawnCooldown;

    [Space]
    [Space]

    [SerializeField] private float secondStageTeleportBulletSpeed;
    [SerializeField] private float secondStageTeleportHomingBulletSpeed;
    [SerializeField] private float secondStageTeleportHomingBulletDelay;

    [Space]
    [Space]

    [SerializeField] private int secondStageDownTimeBulletAmount;
    [SerializeField] private float secondStageDownTimeBulletSpeed;
    [SerializeField] private float secondStageDownTimeHomingBulletAmount;
    [SerializeField] private float secondStageDownTimeHomingBulletSpeed;

    [Header("AI Controller Variables")]

    
    [SerializeField] public List<Transform> orbTransforms = new List<Transform>();

    [SerializeField] private int bulletAngleSpread;

    [Space]
    [Space]

    [SerializeField] private GameObject bossBulletPrefab;
    [SerializeField] private GameObject bossHomingBulletPrefab;

    [Space]

    [SerializeField] private Transform playerTransform;
     private Vector2 playerPosition;

    private float firstStageBulletTimer;

    public enum BossStages
    {
        FirstStage,
        SecondStage
    }

    public enum FirstStage
    {
        NoOrbsDestroyed,
        FirstOrbDestroyed,
        SecondOrbDestroyed,
        ThirdOrbDestroyed,
    }

    public enum SecondStage
    {
        SummonMove,
        TeleportMove,
        DowntimeMove
    }

    public BossStages bossStage;
    public FirstStage firstStage;
    public SecondStage secondStage;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        bossStage = BossStages.FirstStage;
        firstStage = FirstStage.NoOrbsDestroyed;
        secondStage = SecondStage.SummonMove;

        firstStageBulletTimer = firstThreeOrbsBulletFirerate;
    }

    private void Start()
    {

    }

    private void Update()
    {
        #region Calculating AI variables

        #endregion

        switch (bossStage)
        {
            case BossStages.FirstStage:
                switch (firstStage)
                {
                    case FirstStage.NoOrbsDestroyed:

                        if (firstStageBulletTimer >= 0)
                        {
                            firstStageBulletTimer -= Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform transform in orbTransforms)
                            {
                                ShootBullets(noOrbBulletAmount, bulletAngleSpread, transform, firstThreeOrbsBulletFirerate);
                            }

                            firstStageBulletTimer = firstThreeOrbsBulletFirerate;
                        }

                        break;
                    case FirstStage.FirstOrbDestroyed:

                        if (firstStageBulletTimer >= 0)
                        {
                            firstStageBulletTimer -= Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform transform in orbTransforms)
                            {
                                ShootBullets(firstOrbBulletAmount, bulletAngleSpread, transform, firstThreeOrbsBulletFirerate);
                            }

                            firstStageBulletTimer = firstThreeOrbsBulletFirerate;
                        }

                        break;
                    case FirstStage.SecondOrbDestroyed:

                        if (firstStageBulletTimer >= 0)
                        {
                            firstStageBulletTimer -= Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform transform in orbTransforms)
                            {
                                ShootBullets(secondOrbBulletAmount, bulletAngleSpread, transform, firstThreeOrbsBulletFirerate);
                            }

                            firstStageBulletTimer = firstThreeOrbsBulletFirerate;
                        }
                        break;
                    case FirstStage.ThirdOrbDestroyed:

                        if (firstStageBulletTimer >= 0)
                        {
                            firstStageBulletTimer -= Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform transform in orbTransforms)
                            {
                                ShootBullets(thirdOrbBulletAmount, bulletAngleSpread, transform, firstThreeOrbsBulletFirerate);
                            }

                            firstStageBulletTimer = firstThreeOrbsBulletFirerate;
                        }

                        break;
                    default:
                        break;
                }
                break;
            case BossStages.SecondStage:
                switch (secondStage)
                {
                    case SecondStage.SummonMove:
                        break;
                    case SecondStage.TeleportMove:
                        break;
                    case SecondStage.DowntimeMove:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private string AllOrbsShooting(float firstStageBulletSpeed)
    {
        throw new NotImplementedException();
    }

    void ShootBullets(int bulletAmount, float angleSpread, Transform firingTransform, float bulletSpeed)
    {
        Vector2 aimingDirection = (Vector2)playerTransform.position - (Vector2)firingTransform.position;

        float angle = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;

        Vector2 firingPos = firingTransform.position;


        if (bulletAmount > 1)
        {
            float angleBulletAngleSpread = (-(angleSpread * bulletAmount) * 0.5f) - angleSpread * 0.5f;

            for (int i = 0; i < bulletAmount; i++)
            {
                angleBulletAngleSpread += angleSpread;


                angle += angleBulletAngleSpread;

                GameObject bullet = Instantiate(bossBulletPrefab, firingPos, Quaternion.identity);
                bullet.transform.rotation = Quaternion.Euler(0,0,angle);
                Debug.Log(bullet.transform.rotation);
                bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
            }

        }
        else
        {
            GameObject bullet = Instantiate(bossBulletPrefab, firingPos, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        }
    }
    
    void ShootHomingBullet(int bulletAmount, Transform firingTransform, float bulletSpeed)
    {

        Vector2 aimingDirection = (Vector2)playerTransform.position - (Vector2)firingTransform.position;

        float angle = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg;

        Vector2 firingPos = firingTransform.position;

        for (int i = 0; i < bulletAmount; i++)
        {
            Instantiate(bossHomingBulletPrefab, firingPos, Quaternion.Euler(0,0,angle));
        }
    }
}
