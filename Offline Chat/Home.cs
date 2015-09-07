using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace Offline_Chat
{
    public partial class Home : Form
    {
        //string connectionstring = "Data Source=HULK\\SQLEXPRESS;database=Offline_Chat;uid=avneet;pwd=root";
        string connectionstring = "Data Source=192.168.65.44,1433;database=Offline_Chat;uid=avneet;pwd=root";
        Hashtable table = new Hashtable();
        Hashtable table_obj = new Hashtable();
        Socket sck;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Offline_Chat.user_info tem = new user_info();
       

        public Home()
        {
            InitializeComponent();
            username.Text = Form1.user_name.ToString();
            Control.CheckForIllegalCrossThreadCalls = false;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeOwnerDrawnListBox1();
            InitializeOwnerDrawnListBox2();
            listBox2.SelectedIndex = -1;
            listBox1.SelectedIndex = -1;
            listBox1.SelectedIndexChanged += new EventHandler(listBox_SelectedIndexChanged);
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var myDictionary = new Dictionary<KeyValuePair<string, string>, Offline_Chat.Chat_info>();
            

            
            timer.Interval = 5000;
            timer.Enabled = true;
            
            timer.Tick += new System.EventHandler(OnTimerEvent);
          
            //AddElements();
            try
            {
                if (Form1.user_name.ToString().Trim() != null || Form1.login_id.ToString().Trim() != null)
                {

                    if (Form1.role_id.Equals("Student"))
                    {
                        string query = "select roll_no ,password ,name,email,mobile from Student where roll_no ='" + Form1.login_id + "'";
                        SqlConnection con = new SqlConnection(connectionstring);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(query, con);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            try
                            {
                                //MessageBox.Show("hello");
                                tem.Info(ds.Tables[0].Rows[0][2].ToString(), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][4].ToString(), ds.Tables[0].Rows[0][3].ToString(), ds.Tables[0].Rows[0][0].ToString(), Form1.role_id.ToString().Trim());
                                MessageBox.Show("Welcome to Offline Chat " + tem.name);
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show(ee.Message);
                            }
                        }
                    }
                    else if (Form1.role_id.Equals("Instructor"))
                    {
                        string query = "select user_id ,password , name,email_id, mobile_no  from Instructor where user_id ='" + Form1.login_id + "'";
                        SqlConnection con = new SqlConnection(connectionstring);
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(query, con);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            try
                            {
                                Offline_Chat.user_info.Current.Info(ds.Tables[0].Rows[0][2].ToString(), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][4].ToString(), ds.Tables[0].Rows[0][3].ToString(), ds.Tables[0].Rows[0][0].ToString(), Form1.role_id.ToString().Trim());
                                MessageBox.Show(tem.name);
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show(ee.Message);
                            }

                        }
                    }
                }
               // Offline_Chat.user_info tem = new user_info();
               // MessageBox.Show(tem.name);
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            

        }
        //var myDictionary = new Dictionary<KeyValuePair<int, int>, List<string>>(); 
        private string getLocalIp()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily==AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";

        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Form1.login_id.ToString().Trim()!=null || Form1.login_id.ToString().Trim()!=null)
             send_message();
        }
        private async void send_message()
        {
            string result = await send_message_to_user();

        }
        private Task<string> send_message_to_user()
        {
            return Task.Factory.StartNew(() => open_friend_chat());
        }
        private string open_friend_chat()
        {
            try
            {
                string reciever = listBox1.SelectedItem.ToString();
                char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\r', '=','>','\n','-', };
                string[] lines = reciever.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                //string[] words = reciever.Split(delimiterChars);
                int i = 0;
                string reciever_user_name = null; string reciever_login = null;
                foreach (string s in lines)
                {
                    if (i == 0)
                        reciever_user_name = s;
                    if (i == 2)
                        reciever_login = s;
                    i++;

                }
                string[] words = reciever_login.Split(delimiterChars);
                i=0;
                string reciever_login_id=null;
                foreach(string s in words)
                {
                    if ( s !=" " && s!="ID" && s!=null)
                    {
                        reciever_login_id = s;
                        //MessageBox.Show(s);
                        i++;
                    }
                    
                }
                //MessageBox.Show(reciever_user_name);
                //MessageBox.Show(reciever_login_id); 
                string find_ip_address_reciever = "select ip_address from Student_status where roll_no = '"+reciever_login_id+"'";
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(find_ip_address_reciever,con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                string reciever_ip_address = ds.Tables[0].Rows[0][0].ToString().Trim();
                string sender_ip_address = getLocalIp();
                string sender_role = Form1.role_id.ToString();
                string sender_user_name = Form1.user_name;
                string sender_login_id = Form1.login_id;
                string reciever_role = "Student";
                reciever_role = reciever_role.Trim();
                string sender_port = "80" ;
                string reciever_port = "81" ;
                Chat_info info;
                /*var myDictionary = new Dictionary<KeyValuePair<string, string>, Offline_Chat.Chat_info>();
                //Dictionary<Tuple<string, string>, List<string>> dict = new Dictionary<Tuple<string, string>, List<string>>(sender_login_id, reciever_login_id);
                Tuple<string,string>  pair= new Tuple<string,string>(sender_login_id,reciever_login_id);
                KeyValuePair<string, string> pairs = new KeyValuePair<string, string>(sender_login_id,reciever_login_id);
                Chat_info info;
                if (myDictionary[pairs]!=null)
                {
                    info = myDictionary[pairs];
                    
                }
                else
                {
                    info = new Offline_Chat.Chat_info(sender_ip_address, reciever_ip_address, sender_port, reciever_port, sender_user_name, reciever_user_name, sender_login_id, reciever_login_id, sender_role, reciever_role);
                    myDictionary.Add(pairs, info);
                }*/

                //string exist = myDictionary.Add(new KeyValuePair<string, string>(sender_login_id, reciever_login_id),"sender");
                //KeyValuePair<string, string> pairs = new KeyValuePair<string, string>(sender_login_id,reciever_login_id);
                bool flag=false;
                foreach(DictionaryEntry d in table_obj)
                {
                    if(d.Key.ToString()==reciever_login_id)
                    {
                        info = (Offline_Chat.Chat_info)d.Value;
                        flag = true;
                        break;
                    }
                }
                if(flag==false)
                {
                    info = new Offline_Chat.Chat_info(sender_ip_address, reciever_ip_address, sender_port, reciever_port, sender_user_name, reciever_user_name, sender_login_id, reciever_login_id, sender_role, reciever_role);
                    table_obj.Add(reciever_login_id, info);
                    Send_message sm = new Send_message(sender_ip_address, reciever_ip_address, sender_port, reciever_port, sender_user_name, reciever_user_name, sender_login_id, reciever_login_id, sender_role, reciever_role);
                    sm.ShowDialog();
                    
                }
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    //MessageBox.Show(frm.ToString());
                    if(frm.ToString().Contains(reciever_user_name))
                    {
                        //
                    }
                    else
                    {
                        table_obj.Remove(reciever_login_id);
                    }
                }
                

                

                return "succesful";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return "Connecting......";
            }
        }
        private async void ping_users()
        {
            string result = await connection_checking();
            label5.Text = result.ToString();
        }
        public  void OnTimerEvent(object source, EventArgs e)
        {
            if(Form1.login_id!=null || Form1.user_name!=null)
             ping_users();
                     
                
        }
        
        private  Task<string> connection_checking()
        {
            return Task.Factory.StartNew(() => Ping_user());
        }
        private  string Ping_user()
        {
            try
            {
                Thread.Sleep(1000);
                string query = "select user_name ,roll_no , ip_address from Student_status where roll_no <> '" + tem.login_id.ToString().Trim() + "' and status="+1+"";
                SqlConnection con = new SqlConnection(connectionstring);
                //con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var ping = new Ping();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse(ds.Tables[0].Rows[0][2].ToString());
                    var reply = ping.Send(ipaddress, 300);
                    int res;
                    if (reply.Status == IPStatus.Success)
                    { res = 1; }
                    else
                    { res = 0; }
                    string update_status = "update Student_status  set status= " + res + " where roll_no ='" + ds.Tables[0].Rows[0][1].ToString() + "'";
                    SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);

                }
                string query_instructor = "select user_id ,ip_address from Instructor_status where user_id<> '" + tem.login_id.ToString().Trim() + "'";
                SqlDataAdapter da_instructor = new SqlDataAdapter(query_instructor, con);
                DataSet ds_instructor = new DataSet();
                da_instructor.Fill(ds_instructor);
                for (int i = 0; i < ds_instructor.Tables[0].Rows.Count; i++)
                {
                    System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse(ds_instructor.Tables[0].Rows[0][1].ToString());
                    var reply = ping.Send(ipaddress, 300);
                    int res;
                    if (reply.Status == IPStatus.Success)
                    { res = 1; }
                    else { res = 0; }
                    string update_status = "update Instructor_status set status =" + res + " where user_id ='" + ds_instructor.Tables[0].Rows[0][0] + "'";
                    SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                }

                AddElements1();
                AddElements2();
                return "Connected";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return "Connecting.....";
                     
            }
        }
       
      
        private void AddElements1()
        {
            try
            {
                listBox1.Items.Clear();
                string query = "select user_name ,roll_no  from  Student_status where status  =" + 1 + " and roll_no<>'"+Form1.login_id.ToString().Trim()+"'";
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                listBox1.Items.Clear();
                if (ds.Tables.Count == 0)
                {
                    //listBox1.Items.Add("Users are Offline");
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listBox1.Items.Add(ds.Tables[0].Rows[0][0] + "\r\n \r\n ID -> " + ds.Tables[0].Rows[0][1]);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void AddElements2()
        {
            try
            {
                listBox2.Items.Clear();
                string query = "select user_id   from  Instructor_status where status  =" + 1 + " and user_id<>'"+Form1.login_id.ToString().Trim()+"'";
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                listBox2.Items.Clear();
                if(ds.Tables.Count==0)
                {
                    //listBox2.Items.Add("Users are Offline");
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    listBox2.Items.Add(ds.Tables[0].Rows[0][0] + "\r\n \r\n  ");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void InitializeOwnerDrawnListBox1()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();

            // Set the location and size.
            listBox1.Location = new Point(50, 100);
            listBox1.Size = new Size(200, 200);

            // Populate the ListBox.ObjectCollection property  
            // with several strings, using the AddRange method. 
           // this.listBox1.Items.AddRange(new object[]{"System.Windows.Forms", 
			//"System.Drawing", "System.Xml", "System.Net", "System.Runtime.Remoting", 
			//"System.Web","avneet"});
            AddElements1();
            // Turn off the scrollbar.
            listBox1.ScrollAlwaysVisible = true; ;

            // Set the border style to a single, flat border.
            listBox1.BorderStyle = BorderStyle.FixedSingle;

            // Set the DrawMode property to the OwnerDrawVariable value.  
            // This means the MeasureItem and DrawItem events must be  
            // handled.
            listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            listBox1.MeasureItem +=
                new MeasureItemEventHandler(ListBox1_MeasureItem);
            listBox1.DrawItem += new DrawItemEventHandler(ListBox1_DrawItem);
            this.Controls.Add(this.listBox1);

        }
       
        // Handle the DrawItem event for an owner-drawn ListBox. 
        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            // If the item is the selected item, then draw the rectangle 
            // filled in blue. The item is selected when a bitwise And   
            // of the State property and the DrawItemState.Selected  
            // property is true. 
            try
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(Brushes.LimeGreen, e.Bounds);
                }
                else
                {
                    // Otherwise, draw the rectangle filled in beige.
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                }

                // Draw a rectangle in blue around each item.
                e.Graphics.DrawRectangle(Pens.White, e.Bounds);

                // Draw the text in the item.
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),
                    this.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);

                // Draw the focus rectangle around the selected item.
                e.DrawFocusRectangle();
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {

                Console.WriteLine("Error: {0}", outOfRange.Message);
            }
        }

        // Handle the MeasureItem event for an owner-drawn ListBox. 
        private void ListBox1_MeasureItem(object sender,
            MeasureItemEventArgs e)
        {
            try
            {
                // Cast the sender object back to ListBox type.
                ListBox theListBox = (ListBox)sender;

                // Get the string contained in each item. 
                string itemString = (string)theListBox.Items[e.Index];

                // Split the string at the " . "  character.
                string[] resultStrings = itemString.Split('.');

                // If the string contains more than one period, increase the  
                // height by ten pixels; otherwise, increase the height by  
                // five pixels. 
                if (resultStrings.Length > 2)
                {
                    e.ItemHeight += 50;
                }
                else
                {
                    e.ItemHeight += 40;
                }
            }
            catch (Exception outOfRange)
            {

                Console.WriteLine("Error: {0}", outOfRange.Message);
            }
        }
        //to handle Instructor
        private void InitializeOwnerDrawnListBox2()
        {
            this.listBox2 = new System.Windows.Forms.ListBox();

            // Set the location and size.
            listBox2.Location = new Point(50, 350);
            listBox2.Size = new Size(200, 200);

            // Populate the ListBox.ObjectCollection property  
            // with several strings, using the AddRange method. 
            // this.listBox1.Items.AddRange(new object[]{"System.Windows.Forms", 
            //"System.Drawing", "System.Xml", "System.Net", "System.Runtime.Remoting", 
            //"System.Web","avneet"});
            AddElements2();
            // Turn off the scrollbar.
            listBox2.ScrollAlwaysVisible = true; ;

            // Set the border style to a single, flat border.
            listBox2.BorderStyle = BorderStyle.FixedSingle;

            // Set the DrawMode property to the OwnerDrawVariable value.  
            // This means the MeasureItem and DrawItem events must be  
            // handled.
            listBox2.DrawMode = DrawMode.OwnerDrawVariable;
            listBox2.MeasureItem +=
                new MeasureItemEventHandler(ListBox2_MeasureItem);
            listBox2.DrawItem += new DrawItemEventHandler(ListBox2_DrawItem);
            this.Controls.Add(this.listBox2);

        }
        // Handle the DrawItem event for an owner-drawn ListBox. 
        private void ListBox2_DrawItem(object sender, DrawItemEventArgs e)
        {

            // If the item is the selected item, then draw the rectangle 
            // filled in blue. The item is selected when a bitwise And   
            // of the State property and the DrawItemState.Selected  
            // property is true. 
            try
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(Brushes.LimeGreen, e.Bounds);
                }
                else
                {
                    // Otherwise, draw the rectangle filled in beige.
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                }
                if(DrawItemState.Focus==e.State)
                {
                    e.Graphics.FillRectangle(Brushes.Blue, e.Bounds);
                }

                // Draw a rectangle in blue around each item.
                e.Graphics.DrawRectangle(Pens.White, e.Bounds);

               // Point point = listBox2.PointToClient(Cursor.Position);
                //int index = listBox2.IndexFromPoint(point);
                //if (index < 0) return;

                //e.Graphics.DrawString(listBox2.Items[index].ToString(),
                   // this.Font, Brushes.Pink, e.Bounds.X, e.Bounds.Y);
                // Draw the text in the item.
                e.Graphics.DrawString(listBox2.Items[e.Index].ToString(),
                    this.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);

                // Draw the focus rectangle around the selected item.
                e.DrawFocusRectangle();
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {

                Console.WriteLine("Error: {0}", outOfRange.Message);
            }
        }

        // Handle the MeasureItem event for an owner-drawn ListBox. 
        private void ListBox2_MeasureItem(object sender,
            MeasureItemEventArgs e)
        {
            try
            {
                // Cast the sender object back to ListBox type.
                ListBox theListBox = (ListBox)sender;

                // Get the string contained in each item. 
                string itemString = (string)theListBox.Items[e.Index];

                // Split the string at the " . "  character.
                string[] resultStrings = itemString.Split('.');

                // If the string contains more than one period, increase the  
                // height by ten pixels; otherwise, increase the height by  
                // five pixels. 
                if (resultStrings.Length > 2)
                {
                    e.ItemHeight += 50;
                }
                else
                {
                    e.ItemHeight += 40;
                }
            }
            catch (Exception outOfRange)
            {

                Console.WriteLine("Error: {0}", outOfRange.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection(connectionstring);
                if (Form1.role_id.Equals("Student"))
                {
                    string update_status = "update Student_status set status ="+0+" where roll_no= '"+Form1.login_id.ToString().Trim()+"' and status="+1+"";
                    //MessageBox.Show(Form1.login_id.ToString().Trim());
                    SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                    Form1.role_id = null;
                    Form1.login_id = null;
                    Form1.user_name = null;
                    Form1 f1 = new Form1();
                    timer.Stop();
                    this.Hide();
                    this.Close();
                    f1.ShowDialog();
                }
                else if (Form1.role_id.Equals("Instructor"))
                {
                    string update_status = "update Instructor_status set status ="+0+" where user_id= '"+Form1.login_id.ToString().Trim()+"'";
                    SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                    Form1.role_id = null;
                    Form1.login_id = null;
                    Form1.user_name = null;
                    timer.Stop();
                    Form1 f1 = new Form1();
                    this.Hide();
                    this.Close();
                    f1.ShowDialog();
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            //MessageBox.Show(Offline_Chat.user_info.Current.name);
        }

        private void Home_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimumSize = new Size(300, 600);
            this.MaximumSize = new Size(300, 600);
        }        
    }
}
