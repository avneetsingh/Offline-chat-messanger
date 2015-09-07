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

namespace Offline_Chat
{
    public partial class Form2 : Form
    {
        user_info current;
        //string connectionstring = "Data Source=HULK\\SQLEXPRESS;database=Offline_Chat;uid=avneet;pwd=root";
        string connectionstring = "Data Source=192.168.65.44,1433;database=Offline_Chat;uid=avneet;pwd=root";
        int selected_index = 0;
        public Form2()
        {
            InitializeComponent();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //role.Items.Add("--select role--");
            role.Items.Add("Student");
            role.Items.Add("Instructor");
            //role.Items.Add("Admin");
            
            selected_index = role.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string db_name = name.Text.Trim();
            string db_roll_no = roll_number.Text.Trim();
            string db_password = password.Text.Trim();
            string db_comfirm_password = comfirm_password.Text.Trim();
            Int64 db_mobile = Int64.Parse(mobile_no.Text.Trim());
            string db_email = email_id.Text.Trim();
            role_Click(sender, e);
            string ip = get_ip_address();
            MessageBox.Show("Your Ip address is" + ip);
            if (db_name.Length == 0 || db_roll_no.Length == 0 || db_password.Length == 0 || db_comfirm_password.Length == 0 || db_mobile.Equals(0) || db_email.Length == 0)
            { MessageBox.Show("empty field"); }
            if(role.SelectedIndex==0)
            {
                //MessageBox.Show("hello");
                try
                {
                    label3.Text = "Roll Number";
                    string query = "insert into Student values('"+db_name+"','"+db_roll_no+"','"+db_password+"','"+db_comfirm_password+"','"+ db_email+"',"+db_mobile+")";
                    string update_query = "insert into Student_status values('"+db_name+"','"+db_roll_no+"',"+1+",'"+ip+"')";
                    SqlConnection con = new SqlConnection(connectionstring);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //Offline_Chat.user_info.Current.Info(db_name, db_password, db_mobile.ToString(), db_email, db_roll_no, Form1.role_id.ToString().Trim());
                    //con.Open();
                    SqlDataAdapter da_update = new SqlDataAdapter(update_query, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                    con.Close();
                    roll_number.Visible = true;
                    label3.Visible = true;
                    Form1.user_name = db_name;
                    Form1.login_id = db_roll_no;
                    MessageBox.Show("Succesfully Register");
                    Form1.role_id = "Student";
                    Home f3 = new Home();
                    f3.ShowDialog();
                    this.Hide();
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show("Processor Usage" + ex.Message);
                    //MessageBox.Show(ex.Message);
                }
                
            }
            else if(role.SelectedIndex==1)
            {
                try
                {
                    //MessageBox.Show(db_roll_no);
                    string query = "insert into Instructor values('"+db_name+"','"+db_password+"','"+db_email+"','"+db_mobile+ "','"+db_roll_no+"')";
                    string update_query = "insert into Instructor_status values('"+db_roll_no+"',"+1+",'"+ip+"')";
                    SqlConnection con = new SqlConnection(connectionstring);                    
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //Offline_Chat.user_info.Current.Info(db_name, db_password, db_mobile.ToString(), db_email, db_roll_no, Form1.role_id.ToString().Trim());
                    //con.Open();
                    SqlDataAdapter da_update = new SqlDataAdapter(update_query, con);
                    DataSet ds_update = new DataSet();
                    da_update.Fill(ds_update);
                    con.Close();
                    Form1.user_name = db_name;
                    Form1.login_id = db_roll_no;
                    MessageBox.Show("Succesfully Register");
                    Form1.role_id = "Instructor";
                    Home f3 = new Home();
                    f3.ShowDialog();
                    this.Hide();
                }
                catch (NullReferenceException ex)
                {
                   //MessageBox.Show("Processor Usage" + ex.Message);
                    //MessageBox.Show(ex.Message);
                }
            }
            else 
            {
                MessageBox.Show("You have not selected any role , Please select a role");
            }


            

        }

        private void role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected_index != role.SelectedIndex)
            {
                name.Text = "";
                password.Text = "";
                comfirm_password.Text = "";
                roll_number.Text = "";
                email_id.Text = "";
                mobile_no.Text = "";
            
            }
            selected_index = role.SelectedIndex;
            if (role.SelectedIndex == 0)
            {
                
                label3.Text = "Roll Number";
            }
            else
            {
                label3.Text = "Login Id";
            }
        }

        private void role_MouseClick(object sender, MouseEventArgs e)
        {
            role.Select();
        }

        private void role_Click(object sender, EventArgs e)
        {
            role.Select();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimumSize = new Size(300, 600);
            this.MaximumSize = new Size(300, 600);
        }
    }
}
