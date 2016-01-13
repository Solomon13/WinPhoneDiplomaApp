using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinPhoneClient.Enums;

namespace WinPhoneClient.Model
{
    public class Sensor
    {
        public Sensor(SensorType type)
        {
            Type = type;
        }

        public SensorType Type { get; set; }
        public bool IsEnable { get; set; }
    }
}
