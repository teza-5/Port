using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isMoving = false;

    private BoxCollider2D boxCollider2;
    public LayerMask blockingLayer;

    public int attackDamage = 1;
    private Animator animator;

    private int foodPoint;
    public Text foodText;
    private int pointsPerFood = 10;
    private int pointsPerSoda = 20;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;



    // Start is called before the first frame update
    void Start()
    {

        boxCollider2 = GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();

        foodPoint = GameManager.instance.foodPoint;

        foodText.text = "Food : " + foodPoint;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playerTurn)
        {

            return;

        }

        int horizontal = (int)Input.GetAxisRaw("Horizontal");

        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;

            if (horizontal == 1)
            {

                transform.localScale = new Vector3(1, 1, 1);

            }
            else if (horizontal == -1)
            {

                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        else if (vertical != 0)
        {

            horizontal = 0;

        }



        if (horizontal != 0 || vertical != 0)
        {

            ATMove(horizontal, vertical);

        }
    }

    public void ATMove(int x, int y)
    {
        foodPoint--;
        foodText.text = "Food : " + foodPoint;

        RaycastHit2D hit;

        bool canMove = Move(x, y, out hit);

        if (hit.transform == null)
        {
            GameManager.instance.playerTurn = false;
            return;
        }

        Damage hitComponent = hit.transform.GetComponent<Damage>();

        if (!canMove && hit.transform != null && hitComponent != null)
        {
            //�U���p�̊֐�
            OnCantMove(hitComponent);
        }
        CheckFood();
        GameManager.instance.playerTurn = false;
    }


    public bool Move(int x, int y, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);

        boxCollider2.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider2.enabled = true;

        if (!isMoving && hit.transform == null)
        {
            SoundManager.instance.RandomSE(moveSound1, moveSound2);
            //�ړ��p
            StartCoroutine(Movement(end));

            return true;
        }

        return false;
    }

    IEnumerator Movement(Vector3 end)
    {
        isMoving = true;

        float remainingDistance = (transform.position - end).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {

            transform.position = Vector3.MoveTowards(this.gameObject.transform.position, end, 10f * Time.deltaTime);

            remainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;

        }
        transform.position = end;

        isMoving = false;

        CheckFood();

    }

    void OnCantMove(Damage hit)
    {

        hit.AttackDamage(attackDamage);

        animator.SetTrigger("Attack");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {

            foodPoint += pointsPerFood;

            foodText.text = "Food : " + foodPoint;

            SoundManager.instance.RandomSE(eatSound1, eatSound2);


            collision.gameObject.SetActive(false);

        }
        else if (collision.tag == "Soda")
        {

            foodPoint += pointsPerSoda;

            foodText.text = "Food : " + foodPoint;

            SoundManager.instance.RandomSE(drinkSound1, drinkSound2);


            collision.gameObject.SetActive(false);

        }
        else if (collision.tag == "Exit")
        {

            Invoke("Restart", 1f);

            enabled = false;

        }
    }

    public void Restart()
    {

        SceneManager.LoadScene(0);

    }


    private void CheckFood()
    {
        if (foodPoint <= 0)
        {

            SoundManager.instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();

        }
    }

    private void OnDisable()
    {

        GameManager.instance.foodPoint = foodPoint;

    }

    public void EnemyAttack(int loss)
    {

        animator.SetTrigger("Hit");

        foodPoint -= loss;
        foodText.text = "-" + loss + "Food:" + foodPoint;

        CheckFood();

    }
}