using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBeep : MonoBehaviour
{
    public bool isOn = false;

    public GameObject spawner;

    private AudioSource beep;

    private bool is_playing = false;

    private float DELTA_DIST = 5;

    private void Start() {
        beep = GetComponent<AudioSource>();
    }

    private float calculateDistance(Vector3 digsitePos) {
        Vector2 aux1 = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 aux2 = new Vector2(digsitePos.x, digsitePos.z);
        return Vector2.Distance(aux1, aux2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        if (transform.parent != null && transform.parent.CompareTag("glove")) {
            GameObject closestDigSite = null;
            float minDist = 99999;
            bool anyVisible = false;

            // find the closes dig site
            foreach (GameObject digSite in spawner.GetComponent<DigSiteSpawner>().digSiteList) {
                // radar only searches for invisible objects
                if (digSite.GetComponent<DigSite>().visible) continue;

                anyVisible = true;

                if (closestDigSite == null) {
                    closestDigSite = digSite;
                    minDist = calculateDistance(digSite.transform.position);
                    continue;
                }
                
                float newDist = calculateDistance(digSite.transform.position);

                if (newDist < minDist) {
                    closestDigSite = digSite;
                    minDist = newDist;
                }
            }

            // play a beep when getting closer
            if (anyVisible && !is_playing)
                StartCoroutine(playBeep(minDist));

            // if radar is close, make digSite appear
            if (minDist < DELTA_DIST)
                closestDigSite.GetComponent<DigSite>().makeVisible();
        }
    }

    IEnumerator playBeep(float dist) {
        is_playing = true;
        beep.Play();
        float time = 0;
        if (dist > DELTA_DIST * 6) {
            time = beep.clip.length * 10;
        } else if (dist > DELTA_DIST * 2) {
            time = beep.clip.length * 5;
        } else {
            time = beep.clip.length;
        }
        yield return new WaitForSeconds (time);
        is_playing = false;
    }
}
