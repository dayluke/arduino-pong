using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;

public class Arduino : MonoBehaviour
{
    public GameObject[] players;
    public bool controllerActive = false;
    public int commPort = 0;
    private SerialPort serial = null;
    public float lerpSpeed = 10f;

    private void Start()
    {
        ConnectToSerial();
    }

    private void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);

        // Read this: https://support.microsoft.com/en-us/help/115831/howto-specify-serial-ports-larger-than-com9
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    private void Update()
    {
        if (controllerActive)
        {
            WriteToArduino("D"); // distance
            string distanceValue = ReadFromArduino();
            string[] distances = null;

            WriteToArduino("P"); // Potentiometer
            string potentiValue = ReadFromArduino();
            string[] potinetiValues = null;

            if (distanceValue != null)
            {
                distances = distanceValue.Split('-');
            }

            if (potentiValue != null)
            {
                potinetiValues = potentiValue.Split('-');
            }

            positionPlayers(distances, potinetiValues);

            distanceValue = null;
            potentiValue = null;
        }
    }

    private void positionPlayers(string[] zValues, string[] yValues)
    {
        int i = 0;
        foreach (GameObject player in players)
        {
            float yPos = player.transform.position.y;
            float zPos = player.transform.position.z;
            if (zValues[i] != "-1" || zValues[i] != null || int.Parse(zValues[i]) > 1000 || int.Parse(zValues[i]) < 500)
            {
                if (player.name == "P1")
                {
                    zPos = Remap(int.Parse(zValues[i]), 500, 1000, -4, 0.5f);
                }
                else
                {
                    zPos = Remap(int.Parse(zValues[i]), 500, 1000, 0.5f, 4);
                }
            }

            if (yValues[i] != "-1" || yValues[i] != null || int.Parse(yValues[i]) > 1023)
            {
                yPos = Remap(int.Parse(yValues[i]), 0, 1023, 1f, 9.5f);
            }

            Vector3 newPosition = new Vector3(player.transform.position.x, yPos, zPos);
            player.transform.position = Vector3.Lerp(player.transform.position, newPosition, lerpSpeed * Time.deltaTime);
            i++;
        }

    }

    private void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 10)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch (TimeoutException e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    // be sure to close the serial when the game ends.
    private void OnDestroy()
    {
        Debug.Log("Exiting");
        serial.Close();
    }

    // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}