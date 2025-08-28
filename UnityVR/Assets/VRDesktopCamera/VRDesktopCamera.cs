using UnityEngine;

[RequireComponent(typeof(Camera)), DefaultExecutionOrder(-10000)]
public class VRDesktopCamera : MonoBehaviour{
    [Header("Desktop Camera")]
    public bool enable = true;
    public float rotationFollowSpeed = 1;
    public float positionFollowSpeed = 1;
    public float offsetFOV = -15;

    Camera localCam;
    Camera desktopCam;
    bool lastEnable = true;

    void Awake() {
        if(enable) {
            CreateCamera(GetComponent<Camera>());
            desktopCam.gameObject.SetActive(false);
            desktopCam.gameObject.SetActive(enable);
            Invoke("Flicker", 1f);
        }
    }

    void Flicker()
    {
        desktopCam.gameObject.SetActive(false);
        desktopCam.gameObject.SetActive(enable);
    }

    void Update(){
        if(enable != lastEnable) {
            desktopCam.transform.gameObject.SetActive(enable);
            lastEnable = enable;
        }

        if(desktopCam.fieldOfView != localCam.fieldOfView + offsetFOV)
            desktopCam.fieldOfView = localCam.fieldOfView + offsetFOV;

        desktopCam.transform.position = Vector3.Lerp(desktopCam.transform.position, transform.position, (Vector3.Distance(transform.position, desktopCam.transform.position)+1)*Time.deltaTime*positionFollowSpeed*10);
        desktopCam.transform.rotation = Quaternion.Lerp(desktopCam.transform.rotation, transform.rotation, Quaternion.Angle(transform.rotation, desktopCam.transform.rotation)*Time.deltaTime*rotationFollowSpeed/1.5f);
    }

    public Camera DesktopCamera() {
        return desktopCam;
    }

    void CreateCamera(Camera copyCam) {
        if(desktopCam != null)
            Destroy(desktopCam.transform.gameObject);

        localCam = copyCam;
        desktopCam = new GameObject().AddComponent<Camera>();
        desktopCam.gameObject.layer = gameObject.layer;
        desktopCam.cullingMask = localCam.cullingMask;
        desktopCam.name = "DESKTOP CAMERA";
        desktopCam.CopyFrom(copyCam);
        desktopCam.fieldOfView += offsetFOV;
        desktopCam.depth = 0;
        desktopCam.transform.parent = transform.parent;
        desktopCam.nearClipPlane = 0.01f;
        desktopCam.stereoTargetEye = StereoTargetEyeMask.None;
        desktopCam.transform.gameObject.SetActive(enable);
    }

}
