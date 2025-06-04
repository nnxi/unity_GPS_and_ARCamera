using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android; //네임스페이스 추가

public class GPSModule : MonoBehaviour
{
    [Header("Setting")]
    public bool startGPSOnStart; //Start문에서 GPS를 실행할 것인지 여부
    public float desiredAccuracyInMeters; //현재 위치로부터의 최대 오차를 지정하는 변수 (정확도)
    public float updateDistanceInMeters; //특정 거리 이상 이동하면 갱신되도록 지정하는 변수 (갱신 빈도)

    [Header("Cache")]
    private LocationService locationService; //핵심 클래스

    private void Awake()
    {
        locationService = Input.location; //계속 참조할 것이므로 캐싱  <<= 이라고 되어있지만 테스트 할 때는 locationService 변수 안 씀 바로바로 input.location으로 불러올 것임
    }
    public void Start()  // 원래 private
    {
        startGPSOnStart = true;
        desiredAccuracyInMeters = 5f;
        updateDistanceInMeters = 1f;

        /* 버튼 눌러야 실행되는 식으로 바꿈
        if (startGPSOnStart) //Start문에서 GPS를 실행하고자 하면
            StartGPS(); //실행
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

    public void StartGPS(string permissionName = null) //GPS를 실행하는 함수
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) //이미 위치 권한을 획득했으면
        {
            Input.location.Stop();
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters); //서비스 시작  //
            Debug.Log("StartGPS 실행됨");
        }
        else //아직 위치 권한을 획득하지 못했으면
        {
            PermissionCallbacks callbacks = new(); //콜백 함수 생성 후
            callbacks.PermissionGranted += StartGPS; //현재 함수를 재귀로 들어오도록
            Permission.RequestUserPermission(Permission.FineLocation, callbacks); //권한 요청 후, 다시 GPS를 시작하도록 함수 실행
        }
    }

    public void StopGPS() //GPS를 정지하는 함수
    {
        Debug.Log("StopGPS 실행됨");
        Input.location.Stop(); //서비스 정지
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("앱 포커스 복귀: GPS 재시작");
            StartGPS();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("앱 포커스 복귀: GPS 재시작");
            StartGPS();
        }
    }

    public bool GetLocation(out LocationServiceStatus status, out float latitude, out float longitude, out float altitude) //위치 정보를 얻는 함수
    {
        latitude = 0f; //위도
        longitude = 0f; //경도
        altitude = 0f; //고도
        status = Input.location.status; //서비스 상태

        if (!Input.location.isEnabledByUser) //만약, 사용자가 스마트폰의 GPS 기능을 껐다면
            return false;

        switch (status)
        {
            case LocationServiceStatus.Stopped: //GPS를 시작하지 않음
            case LocationServiceStatus.Failed: //GPS 정보를 가져올 수 없음
            case LocationServiceStatus.Initializing: //GPS 기능 시작 후 초기화 중
                return false; //false를 반환해서 정상적으로 정보를 주지 못했음을 알림 (그 원인은 status에 담김)

            default: //GPS 기능이 정상적임 (Running)
                //LocationInfo locationInfo = Input.location.lastData; //마지막 GPS 정보를 담고
                latitude = Input.location.lastData.latitude; //위도 지정
                longitude = Input.location.lastData.longitude; //경도 지정
                altitude = Input.location.lastData.altitude; //고도 지정
                Debug.Log("running이어서 GetLocation() 정상실행됨");
                return true; //true를 반환해서 정상적으로 정보를 줬음을 알림
        }
    }
}