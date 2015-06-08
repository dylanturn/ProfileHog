using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using System.Timers;
using OpenHardwareMonitor.Hardware;
using Newtonsoft.Json;

//TODO: Learn how to program. Oh, and google "Ballmer Peak".
namespace ProfileHog_Console
{
    class Program
    {
        //This is a public bool just incase we want to externally shut off the waiting
        public static bool waitOnConnections = true;

        static string performanceCounter;
        static Computer thisComputer;

        static ManagementObjectSearcher processesSearcher;

        static PerformanceCounter diskReadsPerformanceCounter = new PerformanceCounter();
        static PerformanceCounter diskWritesPerformanceCounter = new PerformanceCounter();
        static PerformanceCounter diskActivePerformanceCounter = new PerformanceCounter();
        

        static void Main(string[] args)
        {
            //create the counters before we start the timer.
            InitializeCounters();

            Timer resourceTimer = new Timer(750);
            resourceTimer.Elapsed += getResourceUtilization;
            resourceTimer.Start();

            while (true)
            {
                Socket clientConnection = WaitForClient(2004);
                if (clientConnection != null) { 
                    Task clientConnectionThread = new Task(() => ClientConnection(clientConnection), TaskCreationOptions.LongRunning);
                    clientConnectionThread.Start();
                }
            }
        }
        
        private static Socket WaitForClient(int Port)
        {
            UdpClient broadcastListener = new UdpClient(Port);
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                while (waitOnConnections)
                {
                    Console.WriteLine("Waiting for devices...");
                    Console.WriteLine("Press CTRL + C to quit...");
                    byte[] bytes = broadcastListener.Receive(ref broadcastEndPoint);

                    Console.WriteLine("Connecting to {0} : {1} ...",broadcastEndPoint.ToString(),Port);
                    TcpClient newClient = new TcpClient(Encoding.ASCII.GetString(bytes, 0, bytes.Length), Port);

                    if (newClient.Connected) {
                        Console.Write("Connected!");
                        Socket connectionSocket = newClient.Client;
                        return connectionSocket;
                    }
                    else
                    {
                        Console.Write("Connnection Failed!");
                    }
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("Connection Error: " + e.ToString());
            }

            finally
            {
                broadcastListener.Close();
            }
            //Worst case senario we have to return null;
            return null;
        }

        private static void ClientConnection(Socket ClientSocket)
        {
            while (ClientSocket.Connected)
            {
                try
                {
                    byte[] input = new byte[ClientSocket.ReceiveBufferSize];
                    ClientSocket.Receive(input);
                }
                catch { ClientSocket.Close(); }

                try
                {
                    ASCIIEncoding outputASCII = new ASCIIEncoding();
                    byte[] outputBytes = outputASCII.GetBytes(performanceCounter);
                    ClientSocket.Send(outputBytes);
                }
                catch { ClientSocket.Close(); }
            }
            ClientSocket.Close();
        }

        //Is there a better way to do this? Yes. Am I going to do it that way right now? No, I'v been drinking.
        private static void InitializeCounters()
        {
            diskReadsPerformanceCounter.CategoryName = "PhysicalDisk";
            diskReadsPerformanceCounter.CounterName = "Disk Reads/sec";
            diskReadsPerformanceCounter.InstanceName = "_Total";

            diskWritesPerformanceCounter.CategoryName = "PhysicalDisk";
            diskWritesPerformanceCounter.CounterName = "Disk Writes/sec";
            diskWritesPerformanceCounter.InstanceName = "_Total";
            

            diskActivePerformanceCounter.CategoryName = "PhysicalDisk";
            diskActivePerformanceCounter.CounterName = "% Disk Time";
            diskActivePerformanceCounter.InstanceName = "_Total";
            
            diskReadsPerformanceCounter.NextValue();
            diskWritesPerformanceCounter.NextValue();
            diskActivePerformanceCounter.NextValue();

            thisComputer = new Computer();
            thisComputer.GPUEnabled = true;
            thisComputer.CPUEnabled = true;
            thisComputer.RAMEnabled = true;
            thisComputer.HDDEnabled = true;
            thisComputer.FanControllerEnabled = true;
            thisComputer.MainboardEnabled = true;
            thisComputer.Open();

            // Create a query for system environment variables only
            SelectQuery query = new SelectQuery("SELECT ProcessId FROM Win32_Process WHERE SessionId = 1");

            // Initialize an object searcher with this query
            processesSearcher = new ManagementObjectSearcher(query);
        }

        private static void getResourceUtilization(Object source, ElapsedEventArgs e)
        {
            ResourceCollection resourceCollection = new ResourceCollection();
            
            IHardware[] computerHardwareList = thisComputer.Hardware;
            foreach (IHardware computerHardware in computerHardwareList)
            {
                computerHardware.Update();

                foreach (IHardware subHardware in computerHardware.SubHardware)
                    subHardware.Update();

                JSONHardware newHardware = new JSONHardware();
                newHardware.Name = computerHardware.Name;
                newHardware.Type = computerHardware.HardwareType.ToString();
                foreach (ISensor thisSensor in computerHardware.Sensors)
                {
                    JSONSensor newSensor = new JSONSensor();
                    newSensor.Name = thisSensor.Name;
                    newSensor.Type = thisSensor.SensorType.ToString();
                    newSensor.Value = thisSensor.Value;
                    newHardware.Sensors.Add(newSensor);
                }

                resourceCollection.Hardware.Add(newHardware);
                
            }

            //resourceCollection.Prosesses = GetProcesses();

            JSONHardware newDisk = new JSONHardware();
            newDisk.Name = "Disk";
            newDisk.Type = "DISK";

            newDisk.Sensors.Add(getDiskReads());
            newDisk.Sensors.Add(getDiskWrites());
            newDisk.Sensors.Add(getDiskActive());

            resourceCollection.Hardware.Add(newDisk);

            performanceCounter = JsonConvert.SerializeObject(resourceCollection);
        }

        private static JSONSensor getDiskReads()
        {
            JSONSensor readsSensor = new JSONSensor();
            readsSensor.Name = "DiskReads";
            readsSensor.Type = "Data";
            readsSensor.Value = (float?)diskReadsPerformanceCounter.NextValue();
            return readsSensor;
        }

        private static JSONSensor getDiskWrites()
        {
            JSONSensor readsSensor = new JSONSensor();
            readsSensor.Name = "DiskWrites";
            readsSensor.Type = "Data";
            readsSensor.Value = (float?)diskWritesPerformanceCounter.NextValue();
            return readsSensor;
        }

        private static JSONSensor getDiskActive()
        {
            JSONSensor readsSensor = new JSONSensor();
            readsSensor.Name = "DiskActive";
            readsSensor.Type = "Data";
            readsSensor.Value = (float?)diskActivePerformanceCounter.NextValue();
            return readsSensor;
        }

        private static List<ProcessInformation> GetProcesses()
        {
            List<ProcessInformation> processInformation = new List<ProcessInformation>();
            
            ManagementObjectCollection ProcessCollection = processesSearcher.Get();

            foreach(ManagementObject processObject in ProcessCollection)
            {
                try
                {
                    ProcessInformation thisProcessInformation = new ProcessInformation();
                    Process thisProcess = Process.GetProcessById(Convert.ToInt32(processObject["ProcessId"]));
                    thisProcessInformation.Id = (UInt32)processObject["ProcessId"];
                    thisProcessInformation.Name = thisProcess.ProcessName;



                    //thisProcessInformation.CpuUsed = Convert.ToInt32(thisProcess.TotalProcessorTime.Milliseconds);
                    thisProcessInformation.RamUsed = Convert.ToInt32(thisProcess.PrivateMemorySize64);


                    //Console.WriteLine(thisProcess.ProcessName);
                    processInformation.Add(thisProcessInformation);
                }
                catch { }

            }

            return processInformation;
        }
    }
}
