using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo este script existe porque hay un "jittering" en la camara al moverse
//Pero hay muchas soluciones distintas sugeridas para resolver esto distinto de como se hace aca
//Por nombrar algunas: Usar LateUpdate(), "add Physics.SyncTransforms(); after changing transform"
//o "replace transform.rotation with GetComponent<Rigidbody>().rotation"

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    private void Awake()
    {
        gameObject.transform.parent = null;
    }
    private void Update()
    {
        transform.position = cameraPosition.position;
    }

}
