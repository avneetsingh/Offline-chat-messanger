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
using System.Data;
using System.Windows.Forms.Design;
namespace Offline_Chat
{
    
    public partial class Form1 : Form
    {
        public static string user_name=null;
        public static string login_id=null;
        public static string role_id = null;

        //string connectionstring = "Data Source=HULK\\SQLEXPRESS;database=Offline_Chat;uid=avneet;pwd=root";
        string connectionstring = "Data Source=192.168.65.44,1433;database=Offline_Chat;uid=avneet;pwd=root";
        int selected_index = 0;
        
        public Form1()
        {
            InitializeComponent();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //listrole.Items.Add("--select role--");
            listrole.Items.Add("Student");
            listrole.Items.Add("Instructor");
            listrole.Items.Add("Admin");
            selected_index = listrole.SelectedIndex;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                this.Hide();
                Form2 f2 = new Form2();
                f2.ShowDialog();
            }
            catch (Exception eg)
            {
                MessageBox.Show(eg.Message);
            }
        }
        private string get_ip_address()
        {
            string ipAddress = "";

            if (System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).Length > 0)
            {
                ipAddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[1].ToString();
            }
            return ipAddress;

        }
        private void listrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("you are changed");
            if (selected_index != listrole.SelectedIndex)
            {
                textid.Text = "";
                textpassword.Text = "";
            }
            if(listrole.SelectedIndex==0)
            {
                label2.Text = "Roll Number";
            }
            else if(listrole.SelectedIndex==1 || listrole.SelectedIndex==2)
            {
                label2.Text = "Login Id";
            }
            else
            {
                MessageBox.Show("You have not selected any role yet !!");
            }
            selected_index = listrole.SelectedIndex;
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            string loginid= textid.Text;
            loginid = loginid.Trim();
            string password = textpassword.Text;
            password = password.Trim();
            listrole_Click(sender, e);
            string ip = get_ip_address();
            if (listrole.SelectedIndex == 0)
            {
                string query = "select roll_no ,password ,name,email,mobile from Student where roll_no ='"+loginid+"' and password='"+password+"'";
                
                SqlConnection con = new SqlConnection(connectionstring);
               // con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                try
                {
                    string check_prev_login = "select  user_name , roll_no, ip_address from Student_status where roll_no ='"+loginid+"' and status = "+1+" and ip_address<>'"+ip.Trim()+"'";
                    SqlDataAdapter da_check = new SqlDataAdapter(check_prev_login, con);
                    DataSet ds_check = new DataSet();
                    da_check.Fill(ds_check);
                    if (ds_check.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("You Previously Logged in With this IP " + ds_check.Tables[0].Rows[0][2].ToString().Trim());
                    }
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                if (ds.Tables[0].Rows.Count == 1)
                {
                    //MessageBox.Show("You have succesfully Logged In");
                    user_name = ds.Tables[0].Rows[0][2].ToString();
                    login_id = loginid;
                    role_id = "Student";
                    
                    //MessageBox.Show("welcome " + ds.Tables[0].Rows[0][2].ToString() + "  " + ds.Tables[0].Rows[0][1].ToString() + "  " + ds.Tables[0].Rows[0][4].ToString() + "  " + ds.Tables[0].Rows[0][3].ToString() + "  " + ds.Tables[0].Rows[0][0].ToString() + "  " + "Student");
                    
                    try
                    {
                        Offline_Chat.user_info tem = new user_info();
                        //tem.Info(ds.Tables[0].Rows[0][2].ToString(), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][4].ToString(), ds.Tables[0].Rows[0][3].ToString(), ds.Tables[0].Rows[0][0].ToString(), Form1.role_id.ToString().Trim());
                        //MessageBox.Show(tem.name);
                        string update_status = "update Student_status set status = "+1+" , ip_address='"+ip+"'  where roll_no= '"+ds.Tables[0].Rows[0][0].ToString()+"'";
                        //con.Open();
                        SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                        DataSet ds_update = new DataSet();
                        da_update.Fill(ds_update);
                    
                    
                    this.Hide();
                    Home f3 = new Home();
                    f3.ShowDialog();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }
                   
                }
                else
                {
                    MessageBox.Show("Provided Information is wrong, Try Again!!");
                }

            }
            else if (listrole.SelectedIndex == 1)
            {
                string query = "select user_id ,password , name,email_id, mobile_no  from Instructor where user_id ='"+loginid+"' and password='"+password+"'";
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    try
                    {
                        MessageBox.Show("You have succesfully Logged In");
                        MessageBox.Show("welcome " + ds.Tables[0].Rows[0][1].ToString());
                        user_name = ds.Tables[0].Rows[0][2].ToString();
                        login_id = ds.Tables[0].Rows[0][0].ToString();
                        role_id = "Instructor";
                        this.Hide();
                        con.Close();
                       // Offline_Chat.user_info.Current.Info(ds.Tables[0].Rows[0][2].ToString(), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][4].ToString(), ds.Tables[0].Rows[0][3].ToString(), ds.Tables[0].Rows[0][0].ToString(), Form1.role_id.ToString().Trim());
                        string update_status = "update Instructor_status set status = "+1+" , ip_address=  '"+ip+"'  where user_id= '"+ds.Tables[0].Rows[0][0].ToString()+"'";
                        //con.Open();
                        SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                        DataSet ds_update = new DataSet();
                        da_update.Fill(ds_update);
                        Home f3 = new Home();
                        f3.ShowDialog();
                    }
                    catch (Exception ee)
                    {
                       // MessageBox.Show(ee.Message);
                    }


                }
                else
                {
                    MessageBox.Show("Provided Information is wrong, Try Again!!");
                }

            }
            else if (listrole.SelectedIndex == 2)
            {
                string query = "select login_id ,password,name  from admin_status where Username ='" + loginid + "' and password='" + password + "'";
                SqlConnection con = new SqlConnection(connectionstring);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("You have succesfully Logged In");
                    MessageBox.Show("welcome " + ds.Tables[0].Rows[0][2].ToString());
                    user_name = ds.Tables[0].Rows[0][2].ToString();
                    login_id = ds.Tables[0].Rows[0][0].ToString();
                    this.Hide();
                    Home f3 = new Home();
                    f3.ShowDialog();
                    string update_status = "update admin_status set status ="+1+" where login_id= '" + ds.Tables[0].Rows[0][0].ToString() + "',";
                    con.Open();
                    SqlDataAdapter da_update = new SqlDataAdapter(update_status, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                }
                else
                {
                    MessageBox.Show("Provided Information is wrong, Try Again!!");
                }
            }
            else
            {
                MessageBox.Show("You have not selected any role , Please select a role");
            }

        }

        private int Int16Converter(int p)
        {
            throw new NotImplementedException();
        }

        private void listrole_Click(object sender, EventArgs e)
        {
            listrole.Select();
        }

        private void listrole_MouseClick(object sender, MouseEventArgs e)
        {
            listrole.Select();
        }

        private void listrole_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimumSize = new Size(300, 600);
            this.MaximumSize = new Size(300, 600);
        }

        private void listrole_SizeChanged(object sender, EventArgs e)
        {
            //this.Size = new Size(50, 40);
        }
    }
}
