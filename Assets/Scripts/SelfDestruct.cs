using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
    public float destroyTime = 1.0f;

	void Update () {
        destroyTime -= Time.deltaTime;
        if (PauseMenu.isPaused == true)
            return;
        if (destroyTime <= 0)
            Destroy(gameObject);
	}
}
