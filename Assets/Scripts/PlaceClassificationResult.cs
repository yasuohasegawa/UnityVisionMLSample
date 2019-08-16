using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceClassificationResult : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;

                    string res = VisionML.GetInstance().m_identifier;
                    if (!String.IsNullOrEmpty(res))
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);

                        Quaternion targetRot = Quaternion.LookRotation(-Camera.main.transform.up, -Camera.main.transform.forward);
                        Vector3 euler = targetRot.eulerAngles;
                        euler.x = 0f;
                        euler.y = euler.y - 180f;
                        euler.z = 0f;

                        spawnedObject.transform.rotation = Quaternion.Euler(euler);

                        // set up the result to the Textfield
                        spawnedObject.GetComponent<ARLabel>().m_text.text = res;
                    }
                }
            }
        }
    }
}
