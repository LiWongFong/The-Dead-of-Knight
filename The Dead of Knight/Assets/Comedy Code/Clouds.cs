using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField]
    private float _threshold;
    [SerializeField]
    private float _jumpAmmount;

    private float _timer = 0f;

    private void Update() {
        if (_timer >= _threshold)
        {
            if (transform.position.x >= 55)
            {
                transform.position = new Vector3(-45,18.8f,0);
            }
            transform.position += new Vector3(_jumpAmmount,0,0);

            _timer = 0;
        }

        _timer += Time.deltaTime;
    }
}
