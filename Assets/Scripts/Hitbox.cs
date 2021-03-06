﻿using DanmakU;
using System.Collections;
using UnityEngine;
using EZCameraShake;

public class Hitbox : MonoBehaviour, IHitbox
{
    public Collider2D Collider2D { get; set; }
    public DanmakuCollider DanmakuCollider { get; set; }
    public IHealthPoint ParentController { get; set; }

    public float invincibleTime = 1.5f;
    public LifeUIController LifeUIController;

    private bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        ParentController = transform.parent.gameObject.GetComponent<IHealthPoint>();
        DanmakuCollider = GetComponent<DanmakuCollider>();
        Collider2D = GetComponent<Collider2D>();

        DanmakuCollider.OnDanmakuCollision += OnDanmakuCollision;
    }

    public void OnDanmakuCollision(DanmakuCollisionList danmakuCollisions)
    {
        for (int i = 0; i < danmakuCollisions.Count; i++)
        {
            //先判断是否是自机的弹幕
            if (!WhoseDanmaku.IsMyDanmaku(danmakuCollisions[i].Danmaku, (ParentController as Player).emitter))
            {
                //子弹消失
                danmakuCollisions[i].Danmaku.Destroy();
                //掉残
                Miss();
                break;//自机一帧只处理一个弹幕碰撞
            }
        }
    }

    IEnumerator InvincibleMiss()    //无敌一段时间
    {
        Collider2D.enabled = false;
        isInvincible = true;
        (ParentController as Player).audioSourceDead.PlayOneShot((ParentController as Player).sePlayerDead);
        yield return new WaitForSeconds(invincibleTime);
        Collider2D.enabled = true;
        isInvincible = false;
    }

    public void Miss()
    {
        if (isInvincible) return;
        ParentController.HP--;
        LifeUIController.SetLifeLevel(ParentController.HP);
        (ParentController as MonoBehaviour).GetComponent<Animator>().SetTrigger("miss");
        CameraShaker.Instance.ShakeOnce(10f, 4f, .2f, .2f);

        //满身疮痍！
        if (ParentController.HP <= 0)
        {
            //关闭擦弹器、判定点的渲染器、发射器
            GetComponent<SpriteRenderer>().enabled = false;
            DanmakuCollider.OnDanmakuCollision -= OnDanmakuCollision;
            (ParentController as Player).Die();
            this.enabled = false;
        } 

        StartCoroutine("InvincibleMiss");   //中弹无敌一段时间
    }
}
