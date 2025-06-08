using Unity.XR.CoreUtils;
using UnityEngine;

public class ARButtonManager : MonoBehaviour
{
    public GameObject mainCamera, arCamera, arSession, arOrigin, GPSCanvas, arCanvas;
    void Start()
    {
        arSession.SetActive(false);
        arOrigin.SetActive(false);
        arCanvas.SetActive(false);
    }

    public void EnterARMode()
    {
        GPSCanvas.SetActive(false);
        mainCamera.gameObject.SetActive(false); // 기존 카메라 끔
        arCamera.tag = "MainCamera";            // AR 카메라에 태그 부여
        arSession.SetActive(true);
        arOrigin.SetActive(true);
        arCanvas.SetActive(true);
    }

    public void ExitARMode()
    {
        arCamera.tag = "Untagged";             // 태그 제거
        arSession.SetActive(false);
        arOrigin.SetActive(false);
        arCanvas.SetActive(false);
        GPSCanvas.SetActive(true);
        mainCamera.gameObject.SetActive(true); // 기존 카메라 다시 켬
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
