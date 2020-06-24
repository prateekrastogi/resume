using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Threading;

namespace AutoShutDown
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private System.Timers.Timer timer;
        public Window1()
        {//using time to do on another thread
            timer = new System.Timers.Timer(100);
            InitializeComponent();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);//timer event handler
            timer.Start();
            //buttons are hidden by changing window style property.
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {   
            Msgboxmsg(); //Type of action that is going to be executed display.
            progress.Value = 0;//initializing progress value
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {//sending message from another thread using dispatcher.
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                if (progress.Value < 20)
                {
                    progress.Value += 0.1;
                    timerbox.Text =((int)(20-progress.Value)).ToString();
                }
                else
                {
                    timer.Stop();
                    this.Close(); //for closing the form in case sleep or hibernate is trigeered then we don't want user to see these window.   
                }
            }));
        }
        void Msgboxmsg()
        {
            int c = Data.w1text;
            switch (c)
            {
                case 0:
                    mtypebox.Text = "Shut Down";
                    break;
                case 1:
                    mtypebox.Text = "Sleep";
                    break;
                case 2:
                    mtypebox.Text = "Hibernate";
                    break;
                case 3:
                    mtypebox.Text = "Log off";
                    break;
                case 4:
                    mtypebox.Text = "Restart";
                    break;
            }
        }
        private void ProgressBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
        //restart the application
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            //Restart the application on clicking cancel button.
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();          
        }
    }
}