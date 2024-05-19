using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject soundManager;


    public void Awake()
    {
        //�Q�[���}�l�[�W���[�Ăяo��
        if (GameManager.instance == null)
        {

            Instantiate(gameManager);

        }
        //�T�E���h�}�l�[�W���[�Ăяo��
        if (SoundManager.instance == null)
        {

            Instantiate(soundManager);

        }

    }
}