using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Spectrum : MonoBehaviour {

	public GameObject cube_prefab;
	public GameObject image;
	public GameObject objectList;
	public GameObject Canvas;
	public GameObject CircleObjects;
	public LineRenderer Line;
	public Camera cam;
	int amount  = 256;
	int lineAmount = 256;
	void Start ()
	{
		Line = GameObject.Find("Line").GetComponent<LineRenderer>();
		CircleObjects = GameObject.Find("CircleObjects");
		Line.SetVertexCount(lineAmount);
		cam = Camera.main;
		for (int i = 0; i < 50; i++)
		{

			Vector3 _position = new Vector3(i * 1.4f - 10 , 0, 0) ;
			GameObject _cube = Instantiate(cube_prefab, _position, Quaternion.identity) as GameObject;
			_cube.transform.parent = objectList.transform;
		}
		for (int i = 0; i < amount; i++)
		{
			float angle = i * Mathf.PI * 2 / amount;
			Vector3 _position = new Vector3(Mathf.Cos(angle) * 450.0f, Mathf.Sin(angle) * 450.0f, 0);
			GameObject uiImage = Instantiate(image, _position, Quaternion.identity) as GameObject;
			uiImage.transform.SetParent(Canvas.transform);
			RectTransform _transform = uiImage.GetComponent<RectTransform>();
			_transform.anchoredPosition = _position;
			float div = _transform.anchoredPosition.x /  _transform.anchoredPosition.y;
			_transform.rotation = Quaternion.Euler(new Vector3(0, 0, 360 - (Mathf.Atan(div) * Mathf.Rad2Deg)));
			if (i >= amount / 2)
			{
				_transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360 - (Mathf.Atan(div) * Mathf.Rad2Deg)) - 180 ));

			}
			if (Canvas.transform.GetChild(i).gameObject.name == "BK")
			{
				Canvas.transform.GetChild(i).transform.SetSiblingIndex(Canvas.transform.childCount - 1);
			}


		}


	}
	float vel;
	float vel2;
	float[] spectrum = new float[256];
	float vel3;
	// Update is called once per frame
	void Update () {


		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Rectangular );


		for (int i = 0; i < objectList.transform.childCount; i++)
		{
			float _scaleY = objectList.transform.GetChild(i).transform.localScale.y;
			float height = (spectrum[i] * i * 1.6f  ) * 10f ;

			_scaleY = Mathf.SmoothDamp(_scaleY, height, ref vel, .08f);
			objectList.transform.GetChild(i).transform.localScale = new Vector3(1f, _scaleY , 1);
			float j = (i * Mathf.Deg2Rad) / 4f;
			float speed = ((float)i /  objectList.transform.childCount) * 3f;

			objectList.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = new Color(Mathf.Sin(j * 16f) , (spectrum[i] * 5f) /  Mathf.PingPong(Time.time * 1, 1)  , 0);
		}


		for (int i = 0; i < amount; i++)
		{
			float _scaleY = Canvas.transform.GetChild(i).transform.localScale.y;

			_scaleY = Mathf.SmoothDamp(_scaleY, spectrum[i] * Mathf.PI / 2f * i / 2f  , ref vel2, .1f);

			Canvas.transform.GetChild(i).transform.localScale = new Vector3(.05f, Mathf.Abs(_scaleY) , 1);

			float color = spectrum[i] * 110f;
			float j = (i * Mathf.Deg2Rad) / amount;
			float shade = Mathf.Sin(j * amount);
			Canvas.transform.GetChild(i).transform.GetComponent<Image>().color = new Color(color * shade, (1f - (float)i / amount)* shade, .5f * shade);

		}

		for (int i = 0; i < lineAmount; i++ )
		{
			Vector3 pos = new Vector3(i - lineAmount / 4.0f,  Mathf.Sin(spectrum[i]) * lineAmount / 2.0f - 49 , 550);

			Line.SetPosition(i, pos);
			Color one = new Color(1f, 1f, 1f, 1f);
			Color two = new Color(spectrum[i] * 255f, spectrum[i] * 255f, 1f, 1f);

			Line.SetColors(one, two);
		}


		// for (int i = 0; i < CircleObjects.transform.childCount; i++)
		// {

		// 	float _scaleY = CircleObjects.transform.GetChild(i).transform.localScale.y;
		// 	_scaleY = Mathf.SmoothDamp(_scaleY, spectrum[i] * 40.0f  - (i * CircleObjects.transform.childCount), ref vel3, .18f);
		// 	CircleObjects.transform.GetChild(i).transform.localScale = new Vector3( Mathf.Abs(_scaleY), Mathf.Abs(_scaleY) , 1);
		// 	CircleObjects.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(20f - ((float)i / (float)CircleObjects.transform.childCount), 1f - ((float)i / (float)CircleObjects.transform.childCount) , 1f, 1);

		// }



	}

	float GetAverage()
	{
		float Avg = 0;
		float sum = 0;

		for (int i = 0; i < spectrum.Length; i++)
		{
			sum += spectrum[i];
		}

		Avg = sum / spectrum.Length;
		return Avg;
	}

}
