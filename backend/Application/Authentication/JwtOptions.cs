﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;

    public int ExpiresHours { get; set; }
}