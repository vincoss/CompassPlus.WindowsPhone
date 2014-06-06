using System;


namespace CompassPlus.Sensors
{
    public class CompassDataChangedEventArgs : EventArgs
    {
        public CompassDataChangedEventArgs(CompassData compassData)
        {
            if (compassData == null)
            {
                throw new ArgumentNullException("compassData");
            }
            CompassData = compassData;
        }

        public CompassData CompassData { get; private set; }
    }
}