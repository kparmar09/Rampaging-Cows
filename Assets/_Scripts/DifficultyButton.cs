using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public Button difficultyButton;

    public float difficulty;

    private SpawnManager spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        difficultyButton = GetComponent<Button>();

        difficultyButton.onClick.AddListener(SetDifficulty);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty()
    {
        spawnManager.StartGame(difficulty);
    }

}
