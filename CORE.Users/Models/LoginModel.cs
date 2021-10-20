﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.Users.Models
{
    public class LoginModel: BaseModel
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public class LoginMinModel:BaseModel
    {
        
    }
}
