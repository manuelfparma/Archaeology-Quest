using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBeep : MonoBehaviour
{
    public bool isOn = false;

    public GameObject spawner;

    private AudioSource beep;

    private bool is_playing = false;

    private void Start() {
        beep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        if (transform.parent != null && transform.parent.CompareTag("glove")) {
            Vector3 closest = new Vector3(9999, 9999, 9999);
            float minDist = Vector3.Distance(transform.position, closest);

            // find the closes dig site
            foreach (GameObject digSite in spawner.GetComponent<DigSiteSpawner>().digSiteList) {
                float newDist = Vector3.Distance(transform.position, digSite.transform.position);
                if (newDist < minDist) {
                    closest = digSite.transform.position;
                    minDist = newDist;
                }
            }

            // play a beep when getting closer
            if (!is_playing)
                StartCoroutine(playBeep(minDist));
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
