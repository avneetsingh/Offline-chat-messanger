using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offline_Chat
{
    class Chat_info
    {
        public  string sender_ip_address;
        public  string reciever_ip_address;
        public  string sender_port;
        public  string reciever_port;
        public  string sender_user_name;
        public  string reciever_user_name;
        public  string sender_login_id;
        public  string reciever_login_id;
        public  string sender_role;
        public string reciever_role;

        public Chat_info(string sender_ip_address, string reciever_ip_address, string sender_port, string reciever_port, string sender_user_name, string reciever_user_name, string sender_login_id, string reciever_login_id, string sender_role, string reciever_role)
        {
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
        }
        public static void connect_user()
        {
            //Send_message sm = new Send_message(sender_ip_address,reciever_ip_address,sender_port,reciever_port,sender_user_name,reciever_user_name,sender_login_id,reciever_login_id,sender_role,reciever_role);
            //sm.ShowDialog();
        }

      

    }
}
