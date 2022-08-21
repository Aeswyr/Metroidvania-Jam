using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private List<LevelSpawn> spawnPoints;
    [SerializeField] private Collider2D cameraBounds;

    public LevelSpawn GetSpawn(int index) {
        return spawnPoints[index];
    }

    public Collider2D GetCameraBounds() {
        return cameraBounds;
    }
}
