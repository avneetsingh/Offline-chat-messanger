using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Offline_Chat
{
    public partial class Send_message : Form
    {
        private string sender_ip_address;
        private string reciever_ip_address;
        private string sender_port;
        private string reciever_port;
        private string sender_user_name;
        private string reciever_user_name;
        private string sender_login_id;
        private string reciever_login_id;
        private string sender_role;
        private string reciever_role;
        
        Socket sck;
        EndPoint epLocal, epRemote;
        byte[] buffer;
        public Send_message()
        {
            InitializeComponent();
        }

        public Send_message(string sender_ip_address, string reciever_ip_address, string sender_port, string reciever_port, string sender_user_name, string reciever_user_name, string sender_login_id, string reciever_login_id, string sender_role, string reciever_role)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.sender_ip_address = sender_ip_address;
            this.reciever_ip_address = reciever_ip_address;
            this.sender_port = sender_port;
            this.reciever_port = reciever_port;
            this.sender_user_name = sender_user_name;
            this.reciever_user_name = reciever_user_name;
            this.sender_login_id = sender_login_id;
            this.reciever_login_id = reciever_login_id;
            this.sender_role = sender_role;
            this.reciever_role = reciever_role;
            this.Text = reciever_user_name;
        }

        private void Send_message_Load(object sender, EventArgs e)
        {
            //set up socket
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress,true);

            //binding socket
            epLocal = new IPEndPoint(IPAddress.Parse(sender_ip_address), Convert.ToInt32(sender_port));
            sck.Bind(epLocal);
            //Connecting to remote IP
            epRemote = new IPEndPoint(IPAddress.Parse(reciever_ip_address), Convert.ToInt32(reciever_port));
            sck.Connect(epRemote);
             
            // Listening the Specific Port
            buffer = new byte[2000];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);

        }
        private void MessageCallBack(IAsyncResult aResult)
        {
           
            try
            {
                byte[] recieved_data = new byte[2000];
                recieved_data = (byte[])aResult.AsyncState;
                //Converting byte[] to string
                ASCIIEncoding eEncoding = new ASCIIEncoding();
                string recieved_message = eEncoding.GetString(recieved_data);

                //Adding this message to List Box
                listBox1.Items.Add(sender_user_name + " : " + recieved_message);

                buffer = new byte[2000];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack),buffer);
           }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //convert string message to byte[]
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sending_message = new byte[2000];
            sending_message = aEncoding.GetBytes(textmessage.Text);
            //sending the encoded message
            sck.Send(sending_message);
            //adding to the listbox
            listBox1.Items.Add("Me :" + textmessage.Text);
            textmessage.Text = "";
        }

        

    }
}
