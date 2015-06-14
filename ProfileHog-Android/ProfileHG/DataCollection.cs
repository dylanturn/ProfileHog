using System;
using System.Text;
using Android.App;
using Android.Content;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Json;
using System.Collections.Generic;

namespace ProfileHG
{
	[Service]
	[IntentFilter(new String[]{"com.StudioTurnbull.DataCollection"})]
	public class DataCollection : Service
	{
		public DataCollection (){}

		public string data;
		int Port = 2004;

		public List<Hardware> HardwareList = new List<Hardware> ();

		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);
			Console.WriteLine ("SimpleService", "SimpleService started");

			Task connectToServer = new Task( () => StartCollection ());
			connectToServer.Start ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			Console.WriteLine ("SimpleService", "SimpleService stopped");      
		}

		private Socket ConnectToServer(TcpListener newListner){
			bool isConnected = false;
			Console.WriteLine("Creating Broadcast Socket");
			Socket broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
			broadcastSocket.EnableBroadcast = true;
			IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, Port);
			string hostName = Dns.GetHostName();
			string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
			byte[] sendBuffer = Encoding.ASCII.GetBytes(myIP);

			while (!isConnected){
				Console.WriteLine("SEnding Broadcast from " + myIP.ToString());
				broadcastSocket.SendTo(sendBuffer, broadcastEndPoint);

				if(newListner.Pending()){
					Console.WriteLine("Connected to server....");
					Socket serverConnection = newListner.AcceptSocket();
					Console.Write("Connected!");
					if(serverConnection.Connected){
						isConnected = true;
					}
					return serverConnection;
				}

				Thread.Sleep(1000);

			}
			return null;
		}

		private void StartCollection (){
			try{
				Console.WriteLine("Waiting for the server connection...");
				TcpListener newListner = new TcpListener(IPAddress.Any,Port);
				LingerOption linger = new LingerOption(true,5);

				newListner.Start();
				//TcpClient client = newListner.AcceptTcpClient();
				Socket serverConnection = ConnectToServer(newListner);

				Console.WriteLine ("Starting the monitoring thread.");
				Task getResourceTask = new Task( () => getResourceUtilization(serverConnection),TaskCreationOptions.LongRunning);
				getResourceTask.Start();
			}
			catch(Exception error){
				Console.WriteLine (error.Message);
			}
		}

		private void getResourceUtilization(Socket ServerConnection)
		{
			while (true) {

				//This sends a byte to the PC to let it know we are ready for fresh data.
				ServerConnection.Send (Encoding.ASCII.GetBytes ("1"));

				//This is the byte array that will ultimatly contain all the JSON
				byte[] cleanRecieve = null;

				//not sure if I need this.
				int available = ServerConnection.Available;

				bool waitingOnData = true;

				//This loop will keep getting checking the server connection for data.
				//Since the default MTU is 1500 bytes and the data recieved from
				//the server is generally a minimum of 1800 bytes we need to check atleast twice to make sure
				//all the data has been recieved.
				while(waitingOnData){
					//This is the byte array that we will be recieving on.
					//I set it to 9000 just incase the user has jumbo frames enabled.
					byte[] rawRecieve = new byte[9000];

					ServerConnection.Receive (rawRecieve);

					//what we are doing is going backwards through the raw recieve byte array
					//we are doing this to try figure out where the actual data ends.
					int i = rawRecieve.Length - 1;
					while (rawRecieve [i] == 0) {
						--i;
					}

					//The first time through cleanRecieve will be null, we need to give it an initial size and value.
					//That value will be the propper length of rawRecieve.
					if (cleanRecieve == null) {
						cleanRecieve = new byte[i + 1];
						Array.Copy(rawRecieve, cleanRecieve, i+1);
					}

					//If its not the first packet we need to append it to our first packet
					else{
						//Create a new temporary byte array for storing cleanRecieve
						byte[] tempRecieve = cleanRecieve;
						//Resize cleanRecieve so it can accept more data
						cleanRecieve = new byte[cleanRecieve.Length + (i+1)];
						//copy tempRecieve back to its home in cleanRecieve
							Array.Copy(tempRecieve,cleanRecieve,tempRecieve.Length);
						//Copy rawRecieve, starting at 0, into cleanrecieve, starting at the next null space.
							Array.Copy(rawRecieve, 0, cleanRecieve, tempRecieve.Length, i + 1);
					}
					
					// At the end of this we should have a single byte array that contains all the data, and only the data.
					// If there is no more data waiting us, then we end the loop.
					// We also sleep the thread to make sure that any packets that are on their way have time to arrive.
					// This is hacking as hell.  I have some ideas on how to fix it but I'll do it later.
					Thread.Sleep(75);
					if (ServerConnection.Available == 0) {
						waitingOnData = false;
					}
				}

				data = Encoding.ASCII.GetString (cleanRecieve);
				JsonObject jsonCounters = null;

				try{
					
					JsonValue value = JsonValue.Parse(data);
					jsonCounters = value as JsonObject;
					// Iterate through the retrieved hardware objects so we can add the information to our dataset.
					foreach (JsonObject hardwareObject in jsonCounters["Hardware"]) {

						// Iterate through the dataset to see if we can find the hardware object.
						Hardware foundHardware = new Hardware();
						foreach(Hardware thisHardware in HardwareList){

							if(thisHardware.HardwareName == (string)hardwareObject["Name"]){
								foundHardware = thisHardware;
							}

						}

						// We found the piece of hardware in the collection.  We now need to look at its sensors
						if(foundHardware.HardwareName != null){
							
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {

								// Iterate through the hardware objects sensor list.
								Sensor foundSensor = new Sensor();
								if(foundHardware.SensorList.Count > 0){
									foreach(Sensor thisSensor in foundHardware.SensorList){
										if((thisSensor.SensorName == (string)sensorObject["Name"]) && (thisSensor.SensorType == Sensor.SensorTypeFromString((string)sensorObject ["Type"]))){
											foundSensor = thisSensor;
										}
									}
								}

								// If we find the sensor we just go ahead and update the value.
								if(foundSensor.SensorName != null){
									Console.WriteLine("Found: {0} Type: {1} Value: {2}", foundSensor.SensorName,(string)sensorObject ["Type"], (float?)sensorObject ["Value"]);
									foundSensor.setCurrentValue(Convert.ToDouble((float?)sensorObject ["Value"]));
								}

								// The sensor wasn't found.  This means we need to add it.
								else {
									Sensor newSensor = new Sensor((string)sensorObject ["Name"], foundHardware, Sensor.SensorTypeFromString((string)sensorObject ["Type"]), Convert.ToDouble((float?)sensorObject ["Value"]));
									foundHardware.SensorList.Add(newSensor);
								}
							}

						} 
						// If we get to this else statement it means this piece of hardware doesnt exist in the collection.  We will now need to add it and its sensors.
						else {
							Hardware newHardware = new Hardware((string)hardwareObject["Name"],  Hardware.HardwareTypeFromString((string)hardwareObject["Type"]), null);

							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								Sensor newSensor = new Sensor((string)sensorObject ["Name"], newHardware, Sensor.SensorTypeFromString((string)sensorObject ["Type"]), Convert.ToDouble((float?)sensorObject ["Value"]));
								newHardware.SensorList.Add(newSensor);
							}

							HardwareList.Add(newHardware);
						}
					}

					Thread.Sleep (500);
				}
				// Just incase somthing goes wrong.
				catch(Exception e){Console.WriteLine (e.Message);}
			}

		}

		// I forgot what this was for...
		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new NotImplementedException ();
		}

	}
}

