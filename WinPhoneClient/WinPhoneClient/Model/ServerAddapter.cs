using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using WinPhoneClient.Enums;

namespace WinPhoneClient.Model
{
    class ServerAddapter
    {
        private Settings _settings = new Settings();
        private List<Drone> _droneList = new List<Drone>
        {
            new Drone {CurrecntTask = "Collect info", DroneType = DroneType.Uterus, Possition = new Point(1,1)},
            new Drone {CurrecntTask = "Moving to target", DroneType = DroneType.Quadrocopter, Possition = new Point(2,2)},
            new Drone {CurrecntTask = "Searching", DroneType = DroneType.Tank, Possition = new Point(3,3)},
            new Drone {CurrecntTask = "Returnung to initial point", DroneType = DroneType.Quadrocopter, Possition = new Point(4,4)}
        };

        public Settings Settings
        {
            get { return _settings; }
        }

        public List<Drone> DroneList
        {
            get { return _droneList; }
        } 
    }
}
