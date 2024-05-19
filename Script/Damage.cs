using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int wallHp = 3;

    public Sprite dmgWall;

    private SpriteRenderer spriteRenderer;

    public int enemyHP = 5;

    private Enemy enemy;

    public AudioClip chopSound1;

    public AudioClip chopSound2;

    // Start is called before the first frame update
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemy = GetComponent<Enemy>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackDamage(int loss)
    {

        SoundManager.instance.RandomSE(chopSound1, chopSound2);

        if (gameObject.CompareTag("Wall"))
        {

            spriteRenderer.sprite = dmgWall;

            wallHp -= loss;

            if (wallHp <= 0)
            {

                gameObject.SetActive(false);

            }
        }
        else if (gameObject.CompareTag("Enemy"))
        {

            enemyHP -= loss;

            if (enemyHP <= 0)
            {

                //ƒŠƒXƒg‚©‚çíœ
                GameManager.instance.DestroyEnemyToList(enemy);

                gameObject.SetActive(false);

            }
        }

    }
}