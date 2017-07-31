using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public struct Level {
        public GameObject enemies;
        public int magic, deception, strength;
    };

    public Level[] levels;
    public int currentLevel = 0;

    private SkillManager skillManager;

    private GameObject player;

    // Use this for initialization
    void Start () {
        skillManager = GameObject.FindObjectOfType<SkillManager>();

        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnLevel() {
        Level lvl = levels[currentLevel];

        GameObject go = GameObject.Instantiate(lvl.enemies);
        WatchEnemies watcher = go.AddComponent<WatchEnemies>();
        watcher.levelManager = this;
    }

    public void Advance() {
        Level lvl = levels[currentLevel];
        skillManager.magicPointsToRemove = lvl.magic;
        skillManager.deceptionPointsToRemove = lvl.deception;
        skillManager.strengthPointsToRemove = lvl.strength;
        skillManager.ShowCurse();

        player.transform.position = new Vector3(0, 0, -10);
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void NextLevel() {
        currentLevel++;
        if (currentLevel == levels.Length) {
            currentLevel = 0;
        }

        SpawnLevel();
        player.transform.position = new Vector3(0, 0, -10);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovement>().MoveTo(new Vector3(0, 0, -10));
    }
}
