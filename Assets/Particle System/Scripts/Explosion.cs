using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public ParticleSystem FX_Explosion;
    public AudioSource SFX_Explosion;

    private float fxTimer;
    private float fxConuter;

	private void Start() {
		fxTimer = FX_Explosion.main.duration + FX_Explosion.main.startLifetime.constantMax;
		fxConuter = fxTimer;
	}

	void Update()
    {
		if (fxConuter <= 0) {
			FX_Explosion.Stop();
			SFX_Explosion.Stop();
		}
		else
			fxConuter -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Space) && fxConuter <= 0) {
			Instantiate(FX_Explosion);
			SFX_Explosion.Play();
			fxConuter = fxTimer;
        }
    }
}
