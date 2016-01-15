using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using WinPhoneClient.Model;

namespace WinPhoneClient.Common
{
    public interface IDroneChangedMessage
    {
        Drone ChangedDrone { get; set; }
    }

    public class DronePossitionChangedMessage : IDroneChangedMessage
    {
        public Drone ChangedDrone { get; set; }
        public Geopoint NewDronePosition { get; set; }
        public List<BasicGeoposition> NewRoutePoints { get; set; }

        public DronePossitionChangedMessage(Drone drone, Geopoint newPosition)
        {
            if(drone == null || newPosition == null)
                throw new ArgumentException();
            ChangedDrone = drone;
            NewDronePosition = newPosition;
        }

        public DronePossitionChangedMessage(Drone drone, Geopoint newPosition, List<BasicGeoposition> points ): this(drone, newPosition)
        {
            if(points == null || !points.Any())
                throw new AggregateException();

            NewRoutePoints = points;
        }
    }
}
