using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _respawnsPrefabs;
    [SerializeField]
    private List<float> _delays;
    // Start is called before the first frame update
    private int numRespawns;
    private List<GameObject> respawnInstances;

    private bool isHordeBeat = false;
    void Start()
    {
        numRespawns = _respawnsPrefabs.Count;
        respawnInstances = new List<GameObject>();
        int counter = 0;
        foreach(GameObject respawnPrefab in _respawnsPrefabs) {
            StartCoroutine(StartRespawn(respawnPrefab, _delays[counter]));
            counter++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (numRespawns > respawnInstances.Count) {
            return;
        }

        isHordeBeat = true;
        foreach (GameObject respawnInstance in respawnInstances)
        {
            isHordeBeat &= respawnInstance == null;
        }

        if (isHordeBeat) {
            Debug.Log("Horde beat!");
            Destroy(gameObject);
        }
    }

    protected IEnumerator StartRespawn(GameObject respawn, float delay) {
        Debug.Log("RESPWAWN STARTED");
        Debug.Log("delay: " + delay);
        yield return new WaitForSeconds(delay);
        GameObject respawnInstance = Instantiate(respawn);
        respawnInstances.Add(respawnInstance);
    }
}
