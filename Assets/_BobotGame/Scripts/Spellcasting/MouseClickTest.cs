using UnityEngine;

public class MouseClickTest : MonoBehaviour
{
    [SerializeField]
    private GameObject test;

    [SerializeField]
    private GameObject test2;

    // Update is called once per frame
    void Update()
    {
        test.SetActive(Input.GetMouseButton(0));
        test2.SetActive(Input.GetButton("Fire1"));
    }
}
