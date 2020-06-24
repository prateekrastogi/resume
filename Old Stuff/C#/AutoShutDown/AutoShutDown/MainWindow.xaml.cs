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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AutoShutDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Forms.NotifyIcon n = new System.Windows.Forms.NotifyIcon();
            string path = AppDomain.CurrentDomain.BaseDirectory; //Make sure we are in applciation base directory
            n.Icon = new System.Drawing.Icon(path + "icon.ico");
            n.Visible = true;
            n.DoubleClick +=
                delegate(object sender, EventArgs args)
                    {
                        this.Show();
                        this.WindowState = WindowState.Normal;
                    };
            
        }
         BackEnd  b = new BackEnd();
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_5(object sender, TextChangedEventArgs e)
        {
        }

        /*Error handling and input attachment to the data object class "Data" for "Process" selection action*/
        private int magicdata; //for roll back in reset

        private void SelectClick(object sender, RoutedEventArgs e)
        {
            int magic = list.SelectedIndex;
            magicdata = magic;

            if (magic == -1)
            {
                MessageBox.Show("Please select the process", "Error");
            }
            else
            {
                ListBoxItem l = (ListBoxItem) list.SelectedItem;
                string name = (string) l.Content;
                if (name == System.Diagnostics.Process.GetCurrentProcess().ProcessName)
                {
                    MessageBox.Show(
                        "You are selecting the PC Down Application itself. Please Select any other process.",
                        "Error"); //checking whether the user is selecting application itself. 
                }
                else
                {
                    l.IsEnabled = false;
                    l.Background = Brushes.Orange;
                    l.FontWeight = FontWeights.Bold;
                    select_btn.IsEnabled = false;
                    Data.pname =name;//very frustating mistake was done earlier here.
                }
            }
        }

        #region Set Button Clicks

        //Event handler for cpu consumption based set attempt and setting "Data Objects" field
        private void PsetClick(object sender, RoutedEventArgs e)
        {
            if (select_btn.IsEnabled)
            {
                MessageBox.Show("Select the process first", "Error");
                goto Exit;
            }
            double d;
            string s = pp.Text.ToString();
            double mp;
            string min = pm.Text.ToString();
            double hp;
            string hr = ph.Text.ToString();
            if (!double.TryParse(s, out d))
            {
                MessageBox.Show("Enter Valid decimal number in CPU % consumption field", "Error");
                pp.Clear();
                goto Exit;
            }
            else if (d >= 100 || d <= 0)
            {
                MessageBox.Show("CPU percentage consumption can be between 0 and 100 only.", "Error");
                pp.Clear();
                goto Exit;
            }
          
            if (hr == "" && min == "")
            {
                MessageBox.Show("Both hours and minutes field can't be empty", "Error");
                goto Exit;
            }
            else if (hr == "")
            {
                hr = "0";
            }
            else if (min == "")
            {
                min = "0";
            }

            if (!double.TryParse(hr, out hp))
            {
                MessageBox.Show("Enter Valid decimal number in hours field", "Error");
                ph.Clear();
                goto Exit;
            }
            else if (hp < 0)
            {
                MessageBox.Show("Enter Valid hours", "Error");
                ph.Clear();
                goto Exit;
            }
            else if(hp>240)
            {
                if (hp >= 35064)
                {
                    MessageBox.Show("Can't set time greater than 4 years.", "Very High");
                    ph.Clear();
                    goto Exit;
                }
                else if (MessageBoxResult.No == MessageBox.Show("You've set the value of hours too much.Are you sure?", "Very High", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    ph.Clear();
                    goto Exit;
                }
            }

            if (!double.TryParse(min, out mp))
            {
                MessageBox.Show("Enter Valid decimal number in minutes field", "Error");
                pm.Clear();
                goto Exit;
            }
            else if (mp >= 60 || mp < 0)
            {
                MessageBox.Show("Enter Valid minutes", "Error");
                pm.Clear();
                goto Exit;
            }

            if (mp == 0 && hp == 0)
            {
                MessageBox.Show("Both hours and minutes can't be zero", "Error");
                ph.Clear();
                pm.Clear();
                goto Exit;
            }
            else
            {
                pset_btn.IsEnabled = false;
                pp.IsEnabled = false;
                pm.IsEnabled = false;
                ph.IsEnabled = false;
                Data.mp = mp;
                Data.hp = hp;
                Data.cpu = d;
                Data.cpuindex = cpulist.SelectedIndex;
                cpulist.IsEnabled = false;
            }
            Exit:
            ;
        }

        //Network Set Button Click event handling and "Data Objects" field Settlement
        private void NSetClick(object sender, RoutedEventArgs e)
        {
            double d;
            string s = drbox.Text.ToString();
            double mp;
            string min = nmbox.Text.ToString();
            double hp;
            string hr = nhbox.Text.ToString();
            if (!double.TryParse(s, out d))
            {
                MessageBox.Show("Enter Valid decimal number in Data field", "Error");
                drbox.Clear();
                goto Exit;
            }
            else if (d <= 0)
            {
                MessageBox.Show("Data rate can only be greater than 0.", "Error");
                drbox.Clear();
                goto Exit;
            }
            else if(d>=9999999999)
            {
                MessageBox.Show("Data rate can not be that much high.", "Error");
            }
            if (hr == "" && min == "")
            {
                MessageBox.Show("Both hours and minutes field can't be empty", "Error");
                goto Exit;
            }
            else if (hr == "")
            {
                hr = "0";
            }
            else if (min == "")
            {
                min = "0";
            }

            if (!double.TryParse(hr, out hp))
            {
                MessageBox.Show("Enter Valid decimal number in hours field", "Error");
                nhbox.Clear();
                goto Exit;
            }
            else if (hp < 0)
            {
                MessageBox.Show("Enter Valid hours", "Error");
                nhbox.Clear();
                goto Exit;
            }
            else if (hp > 240)
            {
                if (hp >= 35064)
                {
                    MessageBox.Show("Can't set time greater than 4 years.", "Very High");
                   nhbox.Clear();
                    goto Exit;
                }
                else if (MessageBoxResult.No == MessageBox.Show("You've set the value of hours too much.Are you sure?", "Very High", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    nhbox.Clear();
                    goto Exit;
                }
            }

            if (!double.TryParse(min, out mp))
            {
                MessageBox.Show("Enter Valid decimal number in minutes field", "Error");
                nmbox.Clear();
                goto Exit;
            }
            else if (mp >= 60 || mp < 0)
            {
                MessageBox.Show("Enter Valid minutes", "Error");
                nmbox.Clear();
                goto Exit;
            }

            if (mp == 0 && hp == 0)
            {
                MessageBox.Show("Both hours and minutes can't be zero", "Error");
                nhbox.Clear();
                nmbox.Clear();
                goto Exit;
            }
            else
            {
                nset_btn.IsEnabled = false;
                drbox.IsEnabled = false;
                nmbox.IsEnabled = false;
                nhbox.IsEnabled = false;
                Data.mn = mp;
                Data.hn = hp;
                Data.datarate = d;
                Data.netindex = netlist.SelectedIndex;
                Data.nopi = Dd_list.SelectedIndex;
                Data.dunitindex = uindex.SelectedIndex;
                netlist.IsEnabled = false;
                Dd_list.IsEnabled = false;
                uindex.IsEnabled = false;
            }
            Exit:
            ;
        }

        /*Error handling and input attachment to data object*/
        /*Error handling and input attachment to the data object class "data" for "Time" initiated action */

        private void SsetClick(object sender, RoutedEventArgs e)
        {
            double mp;
            string min = sm.Text.ToString();
            double hp;
            string hr = sh.Text.ToString();
            if (hr == "" && min == "")
            {
                MessageBox.Show("Both hours and minutes field can't be empty", "Error");
                goto Exit;
            }
            else if (hr == "")
            {
                hr = "0";
            }
            else if (min == "")
            {
                min = "0";
            }
            if (!double.TryParse(hr, out hp))
            {
                MessageBox.Show("Enter Valid decimal number in hours field", "Error");
                sh.Clear();
                goto Exit;
            }
            else if (hp < 0)
            {
                MessageBox.Show("Enter Valid hours", "Error");
                sh.Clear();
                goto Exit;
            }
            else if (hp > 240)
            {
                if (hp>=35064)
                {
                    MessageBox.Show("Can't set time greater than 4 years.", "Very High");
                    sh.Clear();
                    goto Exit;
                }
                else if (MessageBoxResult.No == MessageBox.Show("You've set the value of hours too much.Are you sure?", "Very High", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    sh.Clear();
                    goto Exit;
                }
            }

            if (!double.TryParse(min, out mp))
            {
                MessageBox.Show("Enter Valid decimal number in minutes field", "Error");
                sm.Clear();
                goto Exit;
            }
            else if (mp >= 60 || mp < 0)
            {
                MessageBox.Show("Enter Valid minutes", "Error");
                sm.Clear();
                goto Exit;
            }

            if (mp == 0 && hp == 0)
            {
                MessageBox.Show("Both hours and minutes can't be zero", "Error");
                sh.Clear();
                sm.Clear();
                goto Exit;
            }
            else
            {
                Data.h = hp;
                Data.m = mp;
                sset_btn.IsEnabled = false;
                sm.IsEnabled = false;
                sh.IsEnabled = false;
                Data.tindex = timeindex.SelectedIndex;
                timeindex.IsEnabled = false;
            }
            Exit:
            ;
        }

        #endregion//Very dirty and inelegant coding style is used here. 

        //Handle of "Start" button click and calling appropriate methods by startappropriate().  
        private void StartClick(object sender, RoutedEventArgs e)
        {
            //Handling all 8 cases possible here so that we don't have to manipulate data object to avoid its field default values inteference in our program
            //When none is set
            if (pset_btn.IsEnabled && sset_btn.IsEnabled && nset_btn.IsEnabled)
            {
                MessageBox.Show("Select atleast one of the options.", "Error"); //If none is set
            }
                // when some or are buttons are set
            else{
                if (!pset_btn.IsEnabled)
            {
               b.StartAppropriate(true, false, false);
                start_btn.IsEnabled = false;//Processer set
            }
            if (!sset_btn.IsEnabled)
            {
               b.StartAppropriate(false, false, true); //Time Set     
                start_btn.IsEnabled = false;  
            }
            if (! nset_btn.IsEnabled)
            {
                b.StartAppropriate(false, true, false); //Network Set 
                start_btn.IsEnabled = false;
            }
        }

    }

        private void Loadlist(object sender, RoutedEventArgs e)
        {
            bool check = Help.IsApplicationAlreadyRunning();
            if (check) //check if another instance of the application is running or not 
            {
                this.WindowState = WindowState.Minimized;

                Application.Current.Shutdown();
            }
            else
            {
                list.ItemsSource = Help.Getlist();
            }
            
        }

        private void sm_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_6(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_7(object sender, TextChangedEventArgs e)
        {
        }

        private void AbtClick(object sender, RoutedEventArgs e)
        {
            Window2 w=new Window2();
            w.ShowDialog();

        }

        private void TextBox_TextChanged_8(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged_9(object sender, TextChangedEventArgs e)
        {
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            if (start_btn.IsEnabled)
            {
                Reset();
            }
            else
            {
               b.KillAppropriate(true,true,true);
                b=new BackEnd();//re intializes the object as we can't restart any thread in c#.  
                start_btn.IsEnabled = true;
                Reset();
            }
        }
        //For Rolling Back Changes.
         public void Reset()
        {
//if the selected item changes then to identify the original item for rolling back changes
            list.SelectedIndex = magicdata;
            ListBoxItem l = (ListBoxItem) list.SelectedItem;
            l.IsEnabled = true;
            l.Background = Brushes.White;
            l.FontWeight = FontWeights.Normal;
            select_btn.IsEnabled = true;
            list.SelectedIndex = -1;
            pset_btn.IsEnabled = true;
            nset_btn.IsEnabled = true;
            sset_btn.IsEnabled = true;
            drbox.Clear();
             drbox.IsEnabled = true;
             nhbox.Clear();
             nhbox.IsEnabled = true;
             nmbox.Clear();
             nmbox.IsEnabled = true;
             pp.Clear();
             pp.IsEnabled = true;
             ph.Clear();
             ph.IsEnabled = true;
             pm.Clear();
             pm.IsEnabled = true;
             sm.Clear();
             sm.IsEnabled = true;
             sh.Clear();
             sh.IsEnabled = true;
             cpulist.IsEnabled = true;
            cpulist.SelectedIndex = 0;
             netlist.IsEnabled = true;
            netlist.SelectedIndex = 0;
             timeindex.IsEnabled = true;
            timeindex.SelectedIndex = 0;
             uindex.IsEnabled = true;
            uindex.SelectedIndex = 0;
             Dd_list.IsEnabled = true;
            Dd_list.SelectedIndex = 0;
        }
    }
}