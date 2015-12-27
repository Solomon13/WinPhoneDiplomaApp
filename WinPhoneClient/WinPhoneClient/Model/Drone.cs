using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using WinPhoneClient.Enums;

namespace WinPhoneClient.Model
{
    public class Drone
    {
        public Point Possition { get; set; }
        public DroneType DroneType { get; set; }
        public string CurrecntTask { get; set; }
    }
}
