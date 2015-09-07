using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;


namespace Offline_Chat 
{
    
    class user_info
    {
        public static user_info current;
        
        public string name;
        public string password;
        public string mobile_no;
        public string email_id;
        public string login_id;
        public string role;
        public int status;
        public static user_info Current
        {
            get { return current; }
            set { current = value; }
        }
        public void Info(string name, string password, string mobile_no, string email_id,string login_id,string role)
        {
            this.name = name;
            this.password = password;
            this.mobile_no = mobile_no;
            this.email_id = email_id;
            this.login_id = login_id;
            this.role = role;
            this.status = 1;
          
        }
        public void logout()
        {
            this.name = null;
            this.password = null;
            this.mobile_no = null;
            this.email_id = null;
            this.login_id = null;
            this.role = null;
            this.status = 0;
        }



        
    }
}
