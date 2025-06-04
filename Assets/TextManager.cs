using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Android;


public class TextDirector : MonoBehaviour
{
    public GameObject gpsManager;
    LocationServiceStatus status;
    private float latitude, longitude, altitude, last_latitude, last_longitude, last_altitude;
    private float sec = 0f;
    private float epsilon = 0.0000001f;
    private bool b, c;

    void Start()
    {
        latitude = 0;
        longitude = 0;
        altitude = 0;
        last_altitude = 0;
        last_latitude = 0;
        last_longitude = 0;

        gpsManager.GetComponent<GPSModule>().GetLocation(out status, out latitude, out longitude, out altitude);
    }

    public void GetLocButtonClicked()
    {
        b = gpsManager.GetComponent<GPSModule>().GetLocation(out status, out latitude, out longitude, out altitude);
        Debug.Log("화면에 띄우는 정보 새로고침됨");
    }

    void Update()
    {
        sec += Time.deltaTime;
        if (sec > 100) sec = 0;

        /*
        b = gpsManager.GetComponent<GPSModule>().GetLocation(out status, out latitude, out longitude, out altitude);
        */
        if (Mathf.Abs(latitude - last_latitude) > epsilon || Mathf.Abs(longitude - last_longitude) > epsilon || Mathf.Abs(altitude - last_altitude) > epsilon)
            c = true;
        else
            c = false;

        last_altitude = altitude;
        last_latitude = latitude;
        last_longitude = longitude;

        this.GetComponent<TextMeshProUGUI>().text = "latitude = " + string.Format("{0:F7}", latitude) + "\nlongitude = " + string.Format("{0:F7}", longitude)
          + "\naltitude = " + string.Format("{0:F7}", altitude) + "\ncurrent sec = " + string.Format("{0:D1}", (int)sec) + "\nGPS status = " + gpsManager.GetComponent<GPSModule>().GetGPSstatus()
          + "\nGetLocation() status = " + b + "\npermition status = " + gpsManager.GetComponent<GPSModule>().Getpermission() + "\nGPS change? = " + c + "\ntimestamp = " + Input.location.lastData.timestamp;
    }
}

