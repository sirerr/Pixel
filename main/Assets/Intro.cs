using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
    bool isStarting = false;

	void Update () {
        if (!this.animation.isPlaying && !isStarting)
            StartCoroutine(start());
	}

    IEnumerator start() {
        isStarting = true;
        yield return new WaitForSeconds(2.0F);
        Application.LoadLevel("Main-pixel");
    }
}
