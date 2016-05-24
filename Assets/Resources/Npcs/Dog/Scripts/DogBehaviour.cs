
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Rpg;
using Window;

public struct FarCollision
{
    public Vector2 hitPoint;
    public Vector2 direction;
    public float distance;

    public FarCollision(Vector2 p1, Vector2 p2, float d) {
        hitPoint = p1;
        direction = p2;
        distance = d;
    }
}

public class DogBehaviour : MonoBehaviour {

	// Use this for initialization
	GameObject player;

	Vector2 direction;

	public float threshold;
	public float radiusRange;
	public GameObject boundary;

	Rigidbody2D rbody;
	Animator anim;
	Span span;
	Vector3 spanPos;


	bool movingNow;

	public static bool locked = false;

	Vector3 offset = new Vector3(0f, -0.16f, 0f);
	float MaxDistanceCheck = 10.0f;

	void Start () {
		player = GameObject.Find("Player");
		direction = Vector2.up;

		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		string status = Log.GetValue("mn01dog");
		if(status != null){
			transform.position = boundary.transform.position;
		}

		movingNow = false;

		Transform gc = UnityEngine.GameObject.Find("GameController").transform;
		span = new Span(gc);

		StartCoroutine("WhatIShouldDo");
	}
	
	void AjustDepht() {
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y+(32f*0.01f) );
	}
	// Update is called once per frame
	void Update () {
		AjustDepht();
		anim.SetBool("moving", false);

		float d = Math.Abs(Vector2.Distance(player.transform.position, (transform.position)));
		if( d < radiusRange) {
			if(span.instance == null)
				span.Open("Au Au", spanPos);
			RunAway(player.transform.position);
			return;	
		}

		span.Close();

		Move();
		
	}

	IEnumerator WhatIShouldDo() {
		
		float jitter = UnityEngine.Random.Range(0.8f, 4.3f);

		while(true){

			yield return new WaitForSeconds(jitter);
			movingNow ^= true;
		}

	}

	void RunAway(Vector3 playerPosition) {

		Debug.DrawLine((transform.position+offset), playerPosition);

		Vector2 runTo = (2*(transform.position+offset)-playerPosition);

		Debug.DrawLine((transform.position+offset), runTo);

		direction = (runTo-(Vector2)(transform.position+offset)).normalized;

		MovePosition(1f);

	}

	void MovePosition(float velocity) {
		anim.SetBool("moving", true);
		anim.SetFloat("move-x", direction.x);
		anim.SetFloat("move-y", direction.y);

		rbody.MovePosition(rbody.position + direction*velocity*Time.deltaTime);
		spanPos = Camera.main.WorldToScreenPoint(transform.position);
		spanPos += new Vector3(0, 10, 0);
		span.Follow(spanPos);
	}

	void Move() {

		if(!movingNow) {
			return;
		}

		float distance = Vector3.Distance(transform.position, player.transform.position);

		FarCollision[] collisions = SoFarCollision();

		int index = GetDirectionIndex();
		float d = Vector2.Distance((Vector2)transform.position, collisions[index].hitPoint);
		if(d < threshold) {
			Vector3 aux = (Vector3) collisions[GetDirectionIndex(true)].direction;
			direction = aux;
		}

		threshold = SortDistance(collisions)[0].distance/MaxDistanceCheck + 0.2f;

		MovePosition(0.5f);
            
	}

	int GetDirectionIndex(bool random = false) {
		if(random) return UnityEngine.Random.Range(0, 4);

		if (direction == Vector2.up)    return 0;
		if (direction == Vector2.left)  return 1;
		if (direction == Vector2.right) return 2;
		if (direction == Vector2.down)  return 3;

		return UnityEngine.Random.Range(0, 4);
	}

	FarCollision[] SoFarCollision(bool sort = false) {

		RaycastHit2D hitUp = Physics2D.Raycast((transform.position+offset), Vector3.up, MaxDistanceCheck);
		RaycastHit2D hitLeft = Physics2D.Raycast((transform.position+offset), Vector3.left, MaxDistanceCheck);
		RaycastHit2D hitRight = Physics2D.Raycast((transform.position+offset), Vector3.right, MaxDistanceCheck);
		RaycastHit2D hitDown = Physics2D.Raycast((transform.position+offset), Vector3.down, MaxDistanceCheck);

		//No Detecting
		if(hitUp.point == new Vector2(0f,0f))    hitUp.point = (transform.position+offset) + (Vector3.up*MaxDistanceCheck);
		if(hitLeft.point == new Vector2(0f,0f))  hitLeft.point = (transform.position+offset) + (Vector3.left*MaxDistanceCheck);
		if(hitRight.point == new Vector2(0f,0f)) hitRight.point = (transform.position+offset) + (Vector3.right*MaxDistanceCheck);
		if(hitDown.point == new Vector2(0f,0f))  hitDown.point = (transform.position+offset) + (Vector3.down*MaxDistanceCheck);

	    Debug.DrawLine((transform.position+offset), hitUp.point);
	    Debug.DrawLine((transform.position+offset), hitLeft.point);
	    Debug.DrawLine((transform.position+offset), hitRight.point);
	    Debug.DrawLine((transform.position+offset), hitDown.point);

	    FarCollision[] distance = {
	    	new FarCollision(hitUp.point, Vector2.up, Vector2.Distance((Vector2)(transform.position+offset), hitUp.point)),
	    	new FarCollision(hitLeft.point, Vector2.left, Vector2.Distance((Vector2)(transform.position+offset), hitLeft.point)),
	    	new FarCollision(hitRight.point, Vector2.right, Vector2.Distance((Vector2)(transform.position+offset), hitRight.point)),
	    	new FarCollision(hitDown.point, Vector2.down, Vector2.Distance((Vector2)(transform.position+offset), hitDown.point))
	    };

	    if(sort) SortDistance(distance);

	    return distance;
	}

	FarCollision[] SortDistance(FarCollision[] distance){
		Array.Sort(distance, delegate(FarCollision v1, FarCollision v2) {
	    	float distance1 = Vector2.Distance((Vector2)(transform.position+offset), v1.hitPoint);
	    	float distance2 = Vector2.Distance((Vector2)(transform.position+offset), v2.hitPoint);
        	return distance2.CompareTo(distance1);
      	});

      	return distance;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject == boundary) {
			DogBehaviour.locked = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject == boundary) {
			DogBehaviour.locked = false;
		}
	}
}
