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
            WriteToArduino("D"); // Distance
            string distanceValue = ReadFromArduino();
            string[] distances = null;

            WriteToArduino("P"); // Potentiometer
            string potentiValue = ReadFromArduino();
            string[] potinetiValues = null;

            WriteToArduino("V"); // Vibration
            string otherValue = ReadFromArduino();
            string[] others = null;

            if (distanceValue != null)
            {
                distances = distanceValue.Split('-');
            }

            if (potentiValue != null)
            {
                potinetiValues = potentiValue.Split('-');
            }

            if(otherValue != null)
            {
                others = otherValue.Split('-');
            }

            if (distances.Length == 2 && potinetiValues.Length == 2 && others.Length == 2)
            {
                positionPlayers(distances, potinetiValues, others);
            }
        }
    }

    private void positionPlayers(string[] xValues, string[] yValues, string[] zValues)
    {
        int i = 0;
        foreach (GameObject player in players)
        {
            float xPos = 0f;
            if (player.name == "P1")
            {
                xPos = Remap(int.Parse(xValues[i]), 0, 1023, -4, 0.5f);
            } else
            {
                xPos = Remap(int.Parse(xValues[i]), 0, 1023, 0.5f, 4);
            }

            float yPos = Remap(int.Parse(yValues[i]), 0, 1023, 0, 11);
            float zPos = Remap(int.Parse(zValues[i]), 0, 1023, -4, 4);
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);
            player.transform.position = newPosition;
            i++;
        }

    }

    private void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 50)
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