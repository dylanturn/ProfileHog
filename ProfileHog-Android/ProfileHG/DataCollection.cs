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

		public CounterList cpuCounter = new CounterList ();
		public CounterList ramCounter = new CounterList ();
		public CounterList diskReadCounter = new CounterList ();
		public CounterList diskWriteCounter = new CounterList ();
		public CounterList diskActiveCounter = new CounterList ();
		public CounterList gpuCoreCounter = new CounterList ();
		public CounterList gpuMEMCounter = new CounterList ();

		public List<Hardware> HardwareList = new List<Hardware> ();


		//public List<CounterList> HardwareList = new List<CounterList> ();
		public List<ProcessInformation> processInformation = new List<ProcessInformation>();

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

					
					//At the end of this we should have a single byte array that contains all the data, and only the data.
					//If there is no more data waiting us, then we end the loop.
					//Thread.Sleep(200);
					if (ServerConnection.Available == 0) {
						waitingOnData = false;
						//Console.WriteLine ("No more data. DataSize: " + cleanRecieve.Length);
					}
				}

				data = Encoding.ASCII.GetString (cleanRecieve);
				JsonObject jsonCounters = null;

				try{
					
					JsonValue value = JsonValue.Parse(data);
					jsonCounters = value as JsonObject;

					foreach (JsonObject hardwareObject in jsonCounters["Hardware"]) {
						
						Hardware foundHardware = new Hardware();
						foreach(Hardware thisHardware in HardwareList){
							if(thisHardware.HardwareName == (string)hardwareObject["Name"]){
								foundHardware = thisHardware;
							}
						}

						if(foundHardware.HardwareName != null){
							
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {

								Sensor foundSensor = new Sensor();
								if(foundHardware.SensorList.Count > 0){
									foreach(Sensor thisSensor in foundHardware.SensorList){
										if(thisSensor.SensorName == (string)sensorObject["Name"]){
											foundSensor = thisSensor;
										}
									}
								}

								if(foundSensor.SensorName != null){
									foundSensor.setCurrentValue(Convert.ToInt32((float?)sensorObject ["Value"]));
								} else {
									Sensor newSensor = new Sensor((string)sensorObject ["Name"], foundHardware, Sensor.SensorTypeFromString((string)sensorObject ["Type"]), Convert.ToInt32((float?)sensorObject ["Value"]));
									foundHardware.SensorList.Add(newSensor);
								}
							}

						} else {
							
							Hardware newHardware = new Hardware((string)hardwareObject["Name"],  Hardware.HardwareTypeFromString((string)hardwareObject["Type"]), null);

							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								Sensor newSensor = new Sensor((string)sensorObject ["Name"], newHardware, Sensor.SensorTypeFromString((string)sensorObject ["Type"]), Convert.ToInt32((float?)sensorObject ["Value"]));
								newHardware.SensorList.Add(newSensor);
							}
							HardwareList.Add(newHardware);
						}
					}
					
					foreach (JsonObject hardwareObject in jsonCounters["Hardware"]) {


						if (((string)hardwareObject ["Type"] == "GpuNvidia") || ((string)hardwareObject ["Type"] == "GpuAti")){
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								if (((string)sensorObject ["Type"] == "Load") && (sensorObject ["Name"] == "GPU Core")) {
									gpuCoreCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
								if (((string)sensorObject ["Type"] == "Load") && (sensorObject ["Name"] == "GPU Memory")) {
									gpuMEMCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
							}
						}

						if ((string)hardwareObject ["Type"] == "CPU") {
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								if (((string)sensorObject ["Type"] == "Load") && (sensorObject ["Name"] == "CPU Total")) {
									cpuCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
							}
						}

						if ((string)hardwareObject ["Type"] == "RAM") {
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								if (((string)sensorObject ["Type"] == "Data") && (sensorObject ["Name"] == "Used Memory")) {
									ramCounter.setCurrentValue (Convert.ToInt32 (((float?) sensorObject ["Value"])*1024)); //multiply by 1024 because the used ram is reported in GB
								}
							}
						}

						if ((string)hardwareObject ["Type"] == "DISK") {
							foreach (JsonObject sensorObject in hardwareObject["Sensors"]) {
								if (((string)sensorObject ["Type"] == "Data") && ((string)sensorObject ["Name"] == "DiskReads")) {
									diskReadCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
								if (((string)sensorObject ["Type"] == "Data") && ((string)sensorObject ["Name"] == "DiskWrites")) {
									diskWriteCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
								if (((string)sensorObject ["Type"] == "Data") && ((string)sensorObject ["Name"] == "DiskActive")) {
									diskActiveCounter.setCurrentValue (Convert.ToInt32 ((float?) sensorObject ["Value"]));
								}
							}
						}
					}

					Thread.Sleep (500);
				}
				catch(Exception e){Console.WriteLine (e.Message);}
			}

		}

		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new NotImplementedException ();
		}

	}
}

