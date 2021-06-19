using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooPlayer : MonoBehaviour
{
    private float _speed = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            enemy.ApplyDamage();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector2.left * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector2.right * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector2.up * Time.deltaTime * _speed);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector2.down * Time.deltaTime * _speed);
    }
}
