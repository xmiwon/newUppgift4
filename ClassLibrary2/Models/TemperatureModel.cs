﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2.Models
{

    public class TemperatureModel
    {
        public Current Current { get; set; }
    }

    public class Current
    {
        public double Temp { get; set; }

        public double Humidity { get; set; }
    }
}
