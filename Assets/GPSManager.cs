using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android; //���ӽ����̽� �߰�

public class GPSModule : MonoBehaviour
{
    [Header("Setting")]
    public bool startGPSOnStart; //Start������ GPS�� ������ ������ ����
    public float desiredAccuracyInMeters; //���� ��ġ�κ����� �ִ� ������ �����ϴ� ���� (��Ȯ��)
    public float updateDistanceInMeters; //Ư�� �Ÿ� �̻� �̵��ϸ� ���ŵǵ��� �����ϴ� ���� (���� ��)

    [Header("Cache")]
    private LocationService locationService; //�ٽ� Ŭ����

    private void Awake()
    {
        locationService = Input.location; //��� ������ ���̹Ƿ� ĳ��  <<= �̶�� �Ǿ������� �׽�Ʈ �� ���� locationService ���� �� �� �ٷιٷ� input.location���� �ҷ��� ����
    }
    public void Start()  // ���� private
    {
        startGPSOnStart = true;
        desiredAccuracyInMeters = 5f;
        updateDistanceInMeters = 1f;

        /* ��ư ������ ����Ǵ� ������ �ٲ�
        if (startGPSOnStart) //Start������ GPS�� �����ϰ��� �ϸ�
            StartGPS(); //����
        */
    }

    public LocationServiceStatus GetGPSstatus()
    {
        return Input.location.status;
    }

    public bool Getpermission()
    {
        return Permission.HasUserAuthorizedPermission(Permission.FineLocation);
    }

    public void StartGPS(string permissionName = null) //GPS�� �����ϴ� �Լ�
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) //�̹� ��ġ ������ ȹ��������
        {
            Input.location.Stop();
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters); //���� ����  //
            Debug.Log("StartGPS �����");
        }
        else //���� ��ġ ������ ȹ������ ��������
        {
            PermissionCallbacks callbacks = new(); //�ݹ� �Լ� ���� ��
            callbacks.PermissionGranted += StartGPS; //���� �Լ��� ��ͷ� ��������
            Permission.RequestUserPermission(Permission.FineLocation, callbacks); //���� ��û ��, �ٽ� GPS�� �����ϵ��� �Լ� ����
        }
    }

    public void StopGPS() //GPS�� �����ϴ� �Լ�
    {
        Debug.Log("StopGPS �����");
        Input.location.Stop(); //���� ����
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("�� ��Ŀ�� ����: GPS �����");
            StartGPS();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("�� ��Ŀ�� ����: GPS �����");
            StartGPS();
        }
    }

    public bool GetLocation(out LocationServiceStatus status, out float latitude, out float longitude, out float altitude) //��ġ ������ ��� �Լ�
    {
        latitude = 0f; //����
        longitude = 0f; //�浵
        altitude = 0f; //��
        status = Input.location.status; //���� ����

        if (!Input.location.isEnabledByUser) //����, ����ڰ� ����Ʈ���� GPS ����� ���ٸ�
            return false;

        switch (status)
        {
            case LocationServiceStatus.Stopped: //GPS�� �������� ����
            case LocationServiceStatus.Failed: //GPS ������ ������ �� ����
            case LocationServiceStatus.Initializing: //GPS ��� ���� �� �ʱ�ȭ ��
                return false; //false�� ��ȯ�ؼ� ���������� ������ ���� �������� �˸� (�� ������ status�� ���)

            default: //GPS ����� �������� (Running)
                //LocationInfo locationInfo = Input.location.lastData; //������ GPS ������ ���
                latitude = Input.location.lastData.latitude; //���� ����
                longitude = Input.location.lastData.longitude; //�浵 ����
                altitude = Input.location.lastData.altitude; //�� ����
                Debug.Log("running�̾ GetLocation() ��������");
                return true; //true�� ��ȯ�ؼ� ���������� ������ ������ �˸�
        }
    }
}