﻿using System;

namespace VnuaVaccine.Common
{
    [Serializable]
    public class UserLogin
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int IdUserName { get; set; }
        public int? RoleId { get; set; }     
    }
}