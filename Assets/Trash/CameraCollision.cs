using UnityEngine;

public class CameraCollider : MonoBehaviour
{

    [SerializeField]
    private Transform TransformCamera;
    [SerializeField]
    private float clipingDistance = 1f;

    private RaycastHit hit;
    private Vector3 cameraOffset;
    public Transform transformHead;

    public Vector3 Startoffset;
    public float damping;
    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }


    private void FixedUpdate()
    {
        Vector3 movePosition = Startoffset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }


    // Update is called once per frame
    void Update()
    {
        CameraCollision();
    }



    // Not needed just keeping it in case
    private void CameraCollision()
    {
        Debug.DrawLine(transformHead.position, transform.position, Color.green);

        if (Physics.Linecast(transformHead.position, transform.position, out hit))
        {
            if (!hit.collider.gameObject.tag.Equals("Player") && hit.collider.gameObject.tag.Equals("Wall"))
            {

                TransformCamera.localPosition = Vector3.Lerp(TransformCamera.localPosition, new Vector3(
                                                        hit.point.x + clipingDistance - transformHead.position.x,
                                                        hit.point.y - clipingDistance - transformHead.position.y,
                                                        hit.point.z + clipingDistance - transformHead.position.z) + Vector3.up * 2, Time.deltaTime * 10); ;

                Debug.DrawLine(transformHead.position, new Vector3(
                                                        hit.point.x,
                                                        hit.point.y,
                                                        hit.point.z), Color.red);

            }
        }
        else
        {
            TransformCamera.localPosition = Vector3.Lerp(TransformCamera.localPosition, cameraOffset, Time.deltaTime * 10);
        }
    }
}
