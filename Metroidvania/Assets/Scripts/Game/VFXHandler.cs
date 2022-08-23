using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : Singleton<VFXHandler>
{

        [SerializeField] private GameObject screenWipePrefab;
        private GameObject screenWipe;

        public void StartScreenWipe() {
            screenWipe = Instantiate(screenWipePrefab, Camera.main.transform);
            screenWipe.transform.position += new Vector3(0, 0, 10);
            screenWipe.GetComponent<Animator>().SetTrigger("StartWipe");
        }

        public void EndScreenWipe() {
            screenWipe.GetComponent<Animator>().SetTrigger("EndWipe");
            StartCoroutine(DelayDestroyWipe());
        }

        private IEnumerator DelayDestroyWipe() {
            yield return new WaitForSeconds(0.5f);
            Destroy(screenWipe);
            screenWipe = null;
        }

}
