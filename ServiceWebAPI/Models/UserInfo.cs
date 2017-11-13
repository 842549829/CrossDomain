using System;

namespace ServiceWebAPI
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserPass { get; set; }

        public string Email { get; set; }

        public DateTime RegTime { get; set; }
    }
}