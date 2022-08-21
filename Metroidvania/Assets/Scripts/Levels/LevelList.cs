using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Metroidvania/LevelList", order = 0)]
public class LevelList : ScriptableObject {
    [SerializeField] private List<GameObject> levels;

    public GameObject GetLevel(LevelType level) {
        return levels[(int)level];
    }
}

public enum LevelType {
    INNSWARD, 
}
