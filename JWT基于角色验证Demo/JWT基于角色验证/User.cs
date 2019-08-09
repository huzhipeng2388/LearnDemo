using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT基于角色验证
{
    public class User
    {
        public int Id { get; set; }
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Headphoto { get; set; }
        public string Nickname { get; set; }
        public string Mobile { get; set; }
        public DateTime GenTime { get; set; }
        public int? UserType { get; set; }
        public int Status { get; set; }
    }
}
