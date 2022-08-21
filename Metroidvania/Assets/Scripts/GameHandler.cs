using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameHandler : Singleton<GameHandler>
{
    [Header("Scene data")]
    [SerializeField] private CinemachineVirtualCamera vCam;

    [Header("Level data")]
    [SerializeField] private LevelList levels;

    [Header("VFX")]
    [SerializeField] private GameObject screenWipe;
    
    private LevelHandler currentLevel;
    public void GotoLevel(LevelType levelType, int spawnPointID) {
        StartCoroutine(DelayLoadLevel(levelType, spawnPointID));
    }

    private IEnumerator DelayLoadLevel(LevelType levelType, int spawnPointID) {
        PlayerHandler player = FindObjectOfType<PlayerHandler>();
        player.DisableInputs();

        GameObject wipe = Instantiate(screenWipe, Camera.main.transform);
        wipe.transform.position += new Vector3(0, 0, 10);
        Animator wipeanim = wipe.GetComponent<Animator>();
        wipeanim.SetTrigger("StartWipe");

        yield return new WaitForSeconds(0.5f);

        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        
        GameObject level = levels.GetLevel(levelType);
        level = Instantiate(level, Vector3.zero, Quaternion.identity);
        currentLevel = level.GetComponent<LevelHandler>();

        wipeanim.SetTrigger("EndWipe");
        Vector3 position = currentLevel.GetSpawn(spawnPointID).GetSpawnPos();
        position.z = 0;
        player.transform.position = position;
        position.z = -10;
        vCam.ForceCameraPosition(position, Quaternion.identity);
        

        yield return new WaitForSeconds(0.5f);

        Destroy(wipe);
         player.EnableInputs();
    }
}
