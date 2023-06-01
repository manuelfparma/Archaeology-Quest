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

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        if (transform.parent != null && transform.parent.CompareTag("glove")) {
            GameObject closestDigSite = null;
            float minDist = 99999;

            // find the closes dig site
            foreach (GameObject digSite in spawner.GetComponent<DigSiteSpawner>().digSiteList) {
                // radar only searches for invisible objects
                if (digSite.GetComponent<DigSite>().visible) continue;

                if (closestDigSite == null) {
                    closestDigSite = digSite;
                    minDist = Vector3.Distance(transform.position, digSite.transform.position);
                    continue;
                }
                
                float newDist = Vector3.Distance(transform.position, digSite.transform.position);

                if (newDist < minDist) {
                    closestDigSite = digSite;
                    minDist = newDist;
                }
            }

            // play a beep when getting closer
            if (!is_playing)
                StartCoroutine(playBeep(minDist));

            // if radar is close, make digSite appear
            if (minDist < DELTA_DIST)
                closestDigSite.GetComponent<DigSite>().makeVisible();
        }
    }

    IEnumerator playBeep(float dist) {
        is_playing = true;
        beep.Play();
        float time = beep.clip.length + dist / 50;
        yield return new WaitForSeconds (time);
        is_playing = false;
    }
}
