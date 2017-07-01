using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	public float carSpeed;
	public float maxPos = 2.2f;
	public UIManager ui;
	public AudioManager am;


	private bool currentPlateformAndroid = false;
	private	Vector3 position;
	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D> ();

		#if UNITY_ANDROID
		currentPlateformAndroid = true;

		#else
		currentPlateformAndroid = false;
		#endif

	}

	void Start ()
	{
		if (currentPlateformAndroid == true) {
			print ("Android");
		} else
			print ("Windo");
		
		position = transform.position;
		am.carSound.Play ();
	}


	void Update ()
	{
		if (currentPlateformAndroid == true) {
			//Android Specified Code
			TouchMove();
//			AccelerometerMove();
			
		} else {
			position.x +=Input.GetAxis ("Horizontal") * carSpeed * Time.deltaTime;
			position.x = Mathf.Clamp (position.x,-2.2f,2.2f);
			transform.position = position;
		
		}
		position = transform.position;
		position.x = Mathf.Clamp (position.x,-2.2f,2.2f);
		transform.position = position;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Enemy Car")
		{
			Destroy (gameObject);
			ui.GameOver ();
			am.carSound.Stop ();
		}
	}


	void TouchMove()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);	

			float middlie = Screen.width / 2;

			if (touch.position.x < middlie && touch.phase == TouchPhase.Began) {
				MoveLeft ();
			} else if (touch.position.x > middlie && touch.phase == TouchPhase.Began) {
				MoveRight ();
			} 
		}

		else {
			SetVelocityZero ();
		}
	}


	public void AccelerometerMove()
	{
		float x = Input.acceleration.x;

		if(x < -0.1f)
		{
			MoveLeft ();
		}
		else if(x > 0.1f)
		{
			MoveRight ();	
		}
	}

	public void MoveLeft()
	{
		rb.velocity = new Vector2 (-carSpeed,0);
	}

	public void MoveRight()
	{
		rb.velocity = new Vector2 (carSpeed,0);
	}

	public void SetVelocityZero()
	{
		rb.velocity = Vector2.zero;
	}
}
