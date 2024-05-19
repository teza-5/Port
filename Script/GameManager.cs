using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    BoardManager boardManager;

    public bool playerTurn = true;
    public bool enemyTurn = false;

    public int Level = 1;
    private bool doingSetup;
    public Text levelText;
    public GameObject levelImage;

    public int foodPoint = 100;

   

    private List<Enemy> enemies;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();

        //MapÇê∂ê¨
        InitGame();

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

    static public void Call()
    {

        SceneManager.sceneLoaded += OnSceneLoded;

    }


    static private void OnSceneLoded(Scene next, LoadSceneMode a)
    {

        instance.Level++;

        instance.InitGame();

    }

    public void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");

        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        levelText.text = "Day:" + Level;

        levelImage.SetActive(true);

        Invoke("HideLevelImage", 2f);

        enemies.Clear();

        boardManager.SetupSecene(Level);


    }

    public void HideLevelImage()
    {

        levelImage.SetActive(false);

        doingSetup = false;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn || enemyTurn || doingSetup)
        {

            return;

        }

        StartCoroutine(MoveEnemies());

    }

    public void AddEnemy(Enemy script)
    {

        enemies.Add(script);

    }
    public void DestroyEnemyToList(Enemy script)
    {

        enemies.Remove(script);

    }

    IEnumerator MoveEnemies()
    {
        enemyTurn = true;

        yield return new WaitForSeconds(0.1f);

        if (enemies.Count == 0)
        {

            yield return new WaitForSeconds(0.1f);

        }

        for (int i = 0; i < enemies.Count; i++)
        {

            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(0.1f);

        }

        enemyTurn = false;

        playerTurn = true;

    }

    public void GameOver()
    {

        levelText.text = "GameOver";

        levelImage.SetActive(true);


        enabled = false;

    }
}