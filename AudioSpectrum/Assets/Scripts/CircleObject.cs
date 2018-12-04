using UnityEngine;
using System.Collections;

public class CircleObject : MonoBehaviour {

	// Use this for initialization
	private float Amp;
	void Start () {
		Amp = Random.Range(40.0f, 80.0f);
	}

	// Update is called once per frame
	float vel3;
	void Update () {
		float[] spectrum = new float[256];
		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Rectangular );


		for (int i = 0; i < transform.childCount; i++)
		{

			float _scaleY = transform.GetChild(i).transform.localScale.y;
			_scaleY = Mathf.SmoothDamp(_scaleY, spectrum[i] * 40.0f  - (i * transform.childCount), ref vel3, .18f);
			transform.GetChild(i).transform.localScale = new Vector3( Mathf.Abs(_scaleY), Mathf.Abs(_scaleY) , 1);
			transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(20f - ((float)i / (float)transform.childCount), 1f - ((float)i / (float)transform.childCount) , 1f, 1);

		}

	}
}
