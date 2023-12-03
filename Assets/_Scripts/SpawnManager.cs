using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float xRange = 24f;

    private float startDelay1 = 1;
    private float startDelay2 = 5;
    private float startDelay3 = 4;
    private float startDelay4 = 20;
    private float repeatDelay1;
    private float repeatDelay2;
    private float repeatDelay3;
    private float repeatDelay4;

    public GameObject leftRunningPrefabs;
    public GameObject rightRunningPrefabs;
    public GameObject[] collectiblePrefabs;
    public GameObject extraLife;

    public float upForce;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnAnimals_1()
    {
        if (playerController.isGameActive)
        {
            Vector3 spawnPosLeft_ground = new Vector3(xRange, 0.5f, -4);
            Instantiate(leftRunningPrefabs, spawnPosLeft_ground, leftRunningPrefabs.transform.rotation);
        }
    }
    
    void SpawnAnimals_2()
    {
        if (playerController.isGameActive)
        {
            Vector3 spawnPosRight_ground = new Vector3(-xRange, 0.5f, -4);
            Instantiate(rightRunningPrefabs, spawnPosRight_ground, rightRunningPrefabs.transform.rotation);
        }
    }

    void SpawnCollectibles()
    {
        if (playerController.isGameActive)
        {
            int collectibleIndex = Random.Range(0, collectiblePrefabs.Length);
            float xRangeCol_ = Random.Range(-15f, 15f);
            Vector3 spawnPosCollectibles = new Vector3(xRangeCol_, 19, -4);

            Instantiate(collectiblePrefabs[collectibleIndex], spawnPosCollectibles, collectiblePrefabs[collectibleIndex].transform.rotation);
        }
    }

    void SpawnExtraLife()
    {
        if (playerController.isGameActive)
        {
            float xRangeCol_ = Random.Range(-15f, 15f);
            Vector3 spawnPosCollectibles = new Vector3(xRangeCol_, 19, -4);

            Instantiate(extraLife, spawnPosCollectibles, extraLife.transform.rotation);
        }
    }

    public void StartGame(float difficulty)
    {
        playerController.isGameActive = true;

        playerController.backGround.gameObject.SetActive(false);
        playerController.start.gameObject.SetActive(false);
        playerController.scoreLifePanel.gameObject.SetActive(true);
        playerController.rulePanel.gameObject.SetActive(false);

        repeatDelay1 = Random.Range(3f, 9f);
        repeatDelay2 = Random.Range(6f, 12f);
        repeatDelay3 = Random.Range(1f, 3f);
        repeatDelay4 = 20f;

        InvokeRepeating("SpawnAnimals_1", startDelay1, repeatDelay1/difficulty);
        InvokeRepeating("SpawnAnimals_2", startDelay2, repeatDelay2/difficulty);
        InvokeRepeating("SpawnCollectibles", startDelay3, repeatDelay3);
        InvokeRepeating("SpawnExtraLife", startDelay4, repeatDelay4 * difficulty);

        playerController.startAudio = true;
    }

}
