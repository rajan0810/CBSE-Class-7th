using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public Transform parent;
    public Vector3 offset;
    // Start is called once blic Vector3 ofefore the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        offset = transform.position - parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position + offset;
    }
}
