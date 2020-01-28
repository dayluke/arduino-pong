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
    private bool connected = false;

    // Use this for initialization
    void Start()
    {
        ConnectToSerial();
    }

    void ConnectToSerial()
    {
        Debug.Log("Attempting Serial: " + commPort);

        // Read this: https://support.microsoft.com/en-us/help/115831/howto-specify-serial-ports-larger-than-com9
        serial = new SerialPort("\\\\.\\COM" + commPort, 9600);
        serial.ReadTimeout = 50;
        serial.Open();
    }

    // Update is called once per frame
    void Update()
    {

        if (controllerActive)
        {
            WriteToArduino("P");                // Ask for the positions
            String value = ReadFromArduino(50); // read the positions

            if (value != null)                  // check to see if we got what we need
            {
                // EXPECTED VALUE FORMAT: "0-1023"
                string[] values = value.Split('-');     // split the values

                if (values.Length == 2)
                {
                    positionPlayers(values);
                }
            }
        }
    }

    void positionPlayers(String[] values)
    {
        int i = 0;
        foreach (GameObject player in players)
        {
            float yPos = Remap(int.Parse(values[i]), 0, 1023, 0, 11);         // scale the input. this could be done on the Arduino as well.
            Vector3 newPosition = new Vector3(player.transform.position.x,       // create a new Vector for the position
                yPos, player.transform.position.z);

            player.transform.position = newPosition;        // apply the new position
            i++;
        }

    }

    void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        serial.BaseStream.Flush();
    }

    public string ReadFromArduino(int timeout = 0)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }

    // be sure to close the serial when the game ends.
    void OnDestroy()
    {
        Debug.Log("Exiting");
        serial.Close();
    }

    // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}