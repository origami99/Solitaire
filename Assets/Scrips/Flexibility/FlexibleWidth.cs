using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleWidth : MonoBehaviour
{
    //[SerializeField] private float leftOffset;
    //[SerializeField] private float rightOffset;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private float minUnit = 2f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        float y = this.transform.position.y;

        Vector3 leftEdge = _camera.ScreenToWorldPoint(new Vector3(0, y));
        Vector3 rightEdge = _camera.ScreenToWorldPoint(new Vector3(Screen.width, y));

        float screenWidth = rightEdge.x - leftEdge.x;

        float unit = screenWidth / (objects.Count + 1);
        unit = Mathf.Max(minUnit, unit);

        for (int i = 0; i < objects.Count; i++)
        {
            GameObject obj = objects[i];

            float x = leftEdge.x + unit * (i + 1);

            obj.transform.position = new Vector3(x, obj.transform.position.y, obj.transform.position.z);
        }
    }
}
