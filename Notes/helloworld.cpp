using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSdk.Base;
using GSdk.Cortex;

public class GloveTest : MonoBehaviour
{
private IBoardCentral central = null;
private IBoardPeripheral peripheral = null;
bool initialized = false;

// Use this for initialization
void Start()
{
var boardFactory = new BoardFactory();
try
{
central = boardFactory.MakeBoardCentral();
}
catch (System.Exception err)
{
System.Console.WriteLine(err.Message);
}

central.DiscoverPeripherals();

}

// Update is called once per frame
void Update()
{
if (central != null)
{
if (central.Peripherals != null)
{
if (!initialized && central.Peripherals.Length > 0)
{

foreach (var periph in central.Peripherals) // this assumes only 1 glove is connected.
{ // edit to handle multiple gloves
peripheral = periph;
peripheral.StreamReceived += Peripheral_StreamReceived;
peripheral.Start();
peripheral.StreamTimeslotsWrite(new StreamTimeslots()
{
SensorsState = 6
});

}
initialized = true;
}
}
}

}

private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e)
{
if (e.StreamType == BoardStreamType.SensorsState)
{
var args = e as BoardSensorsStateEventArgs;
var value = FloatsToString(args.Value);
Debug.Log(“Received sensors state: ” + value);
}
}

private static string FloatsToString(float[] value)
{
string result = “”;
var index = 0;
foreach (var element in value)
{
if (index != 0)
{
result += “, “;
}
result += element.ToString();
index += 1;
}
return result;
}

public void Stop()
{
if (central != null)
{
central = null;
}
if (peripheral != null)
{
if (peripheral.Status != BoardStatus.Disconnected)
{
peripheral.Stop();
}
peripheral = null;
}
}

private void OnDestroy()
{
Stop();
}
}

}