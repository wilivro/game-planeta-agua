using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class WheelBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public float angle;

	void Start() {
		angle = transform.localEulerAngles.z;
	}

	public void OnBeginDrag (PointerEventData eventData){}

	public void OnDrag (PointerEventData eventData)
	{
		Vector2 pos = Input.mousePosition - transform.position;
		float relativeAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		transform.localEulerAngles  = new Vector3(0f,0f,relativeAngle);
		angle = transform.localEulerAngles.z;

	}

	public void OnEndDrag (PointerEventData eventData){}
}
