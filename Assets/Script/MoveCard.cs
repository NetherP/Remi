using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour {

    private bool isMove = false;
    private Vector3 _moveTarget;
    private float _speed;

    public void moveTo(Vector3 target, float speed = 30.0f)
    {
        _moveTarget = target;
        _speed = speed;
        isMove = true;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _moveTarget, _speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 90), _speed * Time.deltaTime);
        }
        if (transform.position == _moveTarget)
            isMove = false;
	}

}
