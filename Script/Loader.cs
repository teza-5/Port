using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject soundManager;


    public void Awake()
    {
        //ゲームマネージャー呼び出し
        if (GameManager.instance == null)
        {

            Instantiate(gameManager);

        }
        //サウンドマネージャー呼び出し
        if (SoundManager.instance == null)
        {

            Instantiate(soundManager);

        }

    }
}