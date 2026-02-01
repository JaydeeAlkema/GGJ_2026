using UnityEngine;

public class SpawnTitle : MonoBehaviour
{
    [SerializeField] GameObject Title;
    private void Start() => StartCoroutine(SpawnObjectIn(12f));  

    System.Collections.IEnumerator SpawnObjectIn(float seconds) {
        yield return new WaitForSeconds(seconds);
        Title.SetActive(true);
    }
}
