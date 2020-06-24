using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using System.Net.NetworkInformation;

namespace AutoShutDown
{
    internal static class Help
    {
        public static List<object> Getlist()
        {
            List<object> store = new List<object>();
            Process[] processlist = Process.GetProcesses();
            foreach (var process in processlist)
            {
                ListBoxItem l = new ListBoxItem();
                l.Content = process.ProcessName; //returning a list of listbox item 
                store.Add(l);
            }
            return store;
        }

        public static bool IsApplicationAlreadyRunning()
        {
            string proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
                return true;
            else
                return false;
        }


        public static TimeSpan InputFormat(double h, double m) //for making input consumable
        {
            int hi = (int) h;
            double tempmin = 60*(hi - h) + m;
            if (tempmin - 60 > 0)
            {
                hi++;
            }
            else
            {
                m = tempmin;
            }
            int mi = (int) m;
            int si = (int) ((m - mi)*60);
            TimeSpan t = new TimeSpan(hi, mi, si);
            return t;
        }
    }
    //shared object for threads to consume.
    internal static class Data
    {
        public static string pname { get; set; }
        public static double cpu { get; set; }
        public static double hp { get; set; }
        public static double mp { get; set; }
        public static double h { get; set; }
        public static double m { get; set; }
        public static int nopi { get; set; }
        public static double hn { get; set; }
        public static double mn { get; set; }
        public static double datarate { get; set; }
        public static int cpuindex { get; set; }
        public static int netindex { get; set; }
        public static int tindex { get; set; }
        public static int dunitindex { get; set; }
        public static int w1text { get; set; } //For text block on window 1
    }

     class BackEnd
    {
        Thread cpu = new Thread(() => ProcessSet(Data.cpuindex, Data.cpu, Data.hp, Data.mp, Data.pname));
        Thread net =new Thread(() => NetworkSet(Data.netindex, Data.nopi, Data.datarate, Data.dunitindex, Data.hn, Data.mn, Data.pname));
        //Creating threads for different Options 
        Thread time = new Thread(() => TimeSet(Data.tindex, Data.h, Data.m));
    
        static readonly object mutex = new object(); // for acquiring lock
        static readonly object tmutex = new object(); //for getting correct time to avoid race condition
        private static void ProcessSet(int cpuindex, double cpu, double hp, double mp, string pname)
        {
            Data.w1text = cpuindex; //Setting material for msgbox in window1.
            TimeSpan t=new TimeSpan();
            lock (tmutex)
            {
                t = Help.InputFormat(hp,mp);
            }
            Calculate(cpu,pname,t);
            TryStart(cpuindex);
            
        }
        private static void NetworkSet(int netindex, int nopi, double datarate, int dunitindex, double hn, double mn,
                                       string pname)
        {
            Data.w1text = netindex;
            TimeSpan t=new TimeSpan();
            lock (tmutex)
            {
                t = Help.InputFormat(hn, mn);
            }
            double corbyterate;//conversion of user entered datarate in byte using appropriate user selected units.
            switch (dunitindex)
            {
                case 0:
                    corbyterate = 1024*datarate;//data rate is user entered datarate not our calculated one.
                    break;
                default:
                    corbyterate = 1024*1024*datarate;
                    break;
            }
            Calculate(nopi,corbyterate,pname,t);
            TryStart(netindex);
        }

        private static void TimeSet(int tindex, double h, double m)
        {
            TimeSpan t = new TimeSpan();
            Data.w1text = tindex;
            lock (tmutex)
            {
                t = Help.InputFormat(h, m);
                //because it is static method so to avoid race condition of  acquiring race condition for getting correct input
            }
            Thread.Sleep(t);
            TryStart(tindex);//for taking control.         
        }

        //checks whether the thread is alive or not and then start it if its alive but does not affect the state of other threads
        public void StartAppropriate(bool p, bool n, bool t)
        {
                              
            if (p)
            {//thread are set to background so that when application exists all of them are closed. 
                cpu.IsBackground = true;
                cpu.Start();
            }
            if (n)
            {//thread are set to background so that when application exists all of them are closed. 
                net.IsBackground = true;
                net.Start();   
            }
            if (t)
            {//thread are set to background so that when application exists all of them are closed. 
                time.IsBackground = true;
             time.Start();
            }
        }
        //can only be called from main ui thread can't be called from other thread as they don't own it.
        public  void KillAppropriate(bool p, bool n, bool t)
        {
            if (p)
            {
                if (cpu.IsAlive)
                {
                    cpu.Abort();
                }
            }
            if (n)
            {
                if (net.IsAlive)
                {
                    net.Abort();
                }
            }
            if (t)
            {
                time.Abort();
            }
        }
        private static void CallAppropriate(int index)
        {
            switch (index)
            {
                case 0:
                    Process.Start("shutdown","/s /t 0");
                    break;
                case 1:

                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Suspend, true, true);
                    Application.Current.Shutdown();//for closing appication so waking after sleep or hibernate is not frustating.
                    //for application sleep found no easy way
                    break;
                case 2:
                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);//hibernate using process.start not working properly. It may require some administrative privilages. 
                    Application.Current.Shutdown();//for closing appication so waking after sleep or hibernate is not frustating.
                    break;
                case 3:
                    ExitWindowsEx(4, 0);//because process.start is not doing force log off.
                    break;
                case 4:
                    Process.Start("shutdown","-r -t 0");
                    break;
            }
        }
         //used in call appropriate.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);
        private static void TryStart(int index)
        {
            //trying to lock the thread and if the lock has been taken by someone else than exit the thread gracefully.
            if (Monitor.TryEnter(mutex, new TimeSpan(0, 0, 0)))
            {
                try
                {
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate()
                        {
                            Window1 w = new Window1();
                            w.Show();
                            Application.Current.MainWindow.Close();

                        }));
                    }
                }
                finally
                {

                    Monitor.Exit(mutex);
                }
                Thread.Sleep(22000);
                //give the user 22 seconds assuming that first two second are wasted in processing and user reaction. 
                CallAppropriate(index);
            }
        }
        private static void Calculate(double cpu,string pname,TimeSpan t)
        {
           float cpuf = (float) cpu;//narrowing beacuse performance counter next value is float
            TimeSpan temp=new TimeSpan();//temp time span for calculation
            TimeSpan processend=new TimeSpan(0,5,0);//5 minutes timespan if the process is not running
            bool flag = false;//if its true that means preivious temp was claculated in process not running case.
            PerformanceCounter pc = new PerformanceCounter("Process", "% Processor Time", pname);
            if (Process.GetProcessesByName(pname).Length != 0)//for checking if the process is running or not.
            {               
                pc.NextValue(); 
            }
                while (true)
                {
                    Stopwatch s=new Stopwatch();  //reintializing the stop watch for every while loop.
                    s.Start();    
                    //if process is not running
                    if (Process.GetProcessesByName(pname).Length == 0 && flag == false)//if earlier temp was in process running
                    {
                        temp=new TimeSpan(0,0,0);  
                        flag = true;
                        
                        
                        Thread.Sleep(1000);
                        temp += s.Elapsed;
                    }
                    else if (Process.GetProcessesByName(pname).Length == 0 && flag == true)//if earlier temp was in process not running.
                    {
                        if (TimeSpan.Compare(temp, processend) >= 0)//if process is not running for default time then exit.
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                        temp += s.Elapsed;
                    }
                        //if its running.
                    else if (Process.GetProcessesByName(pname).Length !=0 && flag == true)//if earlier process was not running.
                    {
                        if (pc.NextValue() >= cpuf)//process cpu utilisation is above the entered value.
                        {
                            flag = false;
                            temp = new TimeSpan(0,0,0);//reset the counter.
                            pc = new PerformanceCounter("Process", "% Processor Time", pname);//reset the processer time counter so that it starts calculating  % utilization from here onwards.
                            pc.NextValue();
                            Thread.Sleep(1000);
                            
                        }
                        else
                        { 
                            flag = false;
                            temp = new TimeSpan(0,0,0);//reset the counter because of earlier was  not running case.
                            pc = new PerformanceCounter("Process", "% Processor Time", pname);
                            pc.NextValue();//because it is updated once in every 1 or .5 seconds for precise answer.
                            Thread.Sleep(1000);                
                            temp += s.Elapsed;
                        }
                    }
                    else if (Process.GetProcessesByName(pname).Length != 0 && flag == false)
                    {
                        
                        if (pc.NextValue() >= cpuf)////process cpu utilisation is above the entered value.
                        {
                            temp = new TimeSpan(0,0,0);//reset the counter
                            pc = new PerformanceCounter("Process", "% Processor Time", pname);
                            pc.NextValue();
                            Thread.Sleep(1000);
                            
                        }
                        else
                        { 
                            if (TimeSpan.Compare(temp,t)>=0)//if time of such thing is greater than  user entered then break;
                            {
                                break;
                            }
                            pc = new PerformanceCounter("Process", "% Processor Time", pname);
                            pc.NextValue();
                            Thread.Sleep(1000);
                            temp += s.Elapsed;//increase the value.
                                                       
                        }
                    }
                }
        }
        private static void Calculate(int nopi, double datarate, string pname, TimeSpan t)
        {
            TimeSpan temp = new TimeSpan(0,0,0);
            List<PerformanceCounter> list=new List<PerformanceCounter>();//need list beacuse of need to get accurate counter value. 
            
            while (true)
            {
                Stopwatch s=new Stopwatch();
                PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");//in while loop so that it can immediately check if any other nic has been turned on or off during polling.
                s.Start();
                double data = 0;
                //for selecting the type of data transfer that user has selected to sniff.
                if (nopi == 0)
                {
                    foreach (var instance in performanceCounterCategory.GetInstanceNames())
                    {
                        PerformanceCounter download = new PerformanceCounter("Network Interface", "Bytes Received/sec",
                                                                             instance);
                        list.Add(download);
                        download.NextValue();//for precaution i am not trusting first next value
                    }
                    Thread.Sleep(1000);//sleeping the thread for 1 seconds so that performance counter could be updated by the system and also saves cpu clock cycles.
                    foreach (var performanceCounter in list)
                    {
                        data += performanceCounter.NextValue();
                    }

                }
                else if (nopi == 1)
                {
                    foreach (var instance in performanceCounterCategory.GetInstanceNames())
                    {
                        PerformanceCounter upload = new PerformanceCounter("Network Interface", "Bytes Sent/sec",
                                                                           instance);
                        list.Add(upload);
                        upload.NextValue();
                    }
                    Thread.Sleep(1000);
                    foreach (var performanceCounter in list)
                    {
                        data += performanceCounter.NextValue();
                    }
                }
                else if (nopi == 2)
                {
                    foreach (var instance in performanceCounterCategory.GetInstanceNames())
                    {
                        PerformanceCounter total = new PerformanceCounter("Network Interface", "Bytes Total/sec",
                                                                          instance);
                        list.Add(total);
                         total.NextValue();
                    }
                    Thread.Sleep(1000);
                    foreach (var performanceCounter in list)
                    {
                        data += performanceCounter.NextValue();
                    }
                }
                if (data>=datarate)
                {
                    temp=new TimeSpan(0,0,0);//if datarate is greater than user entered minimum value than resetting the counter. 
                }
                    //if datarate rate is less than user enterd.
                else
                {
                    temp += s.Elapsed;
                    if (TimeSpan.Compare(temp,t)>=0)//if time exceeds the user entered value.
                    {
                        break;
                    }
                }
                }
        }
    }
}