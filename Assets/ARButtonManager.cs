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
        mainCamera.gameObject.SetActive(false); // ���� ī�޶� ��
        arCamera.tag = "MainCamera";            // AR ī�޶� �±� �ο�
        arSession.SetActive(true);
        arOrigin.SetActive(true);
        arCanvas.SetActive(true);
    }

    public void ExitARMode()
    {
        arCamera.tag = "Untagged";             // �±� ����
        arSession.SetActive(false);
        arOrigin.SetActive(false);
        arCanvas.SetActive(false);
        GPSCanvas.SetActive(true);
        mainCamera.gameObject.SetActive(true); // ���� ī�޶� �ٽ� ��
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
