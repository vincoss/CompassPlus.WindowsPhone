using System;
using Microsoft.Xna.Framework;
using Microsoft.Devices.Sensors;


namespace CompassPlus.Sensors
{
    public class CompassData
    {
        public CompassData(DateTimeOffset timestamp, double headingAccurancy, double trueHeading, double magneticHeading, Vector3 magnetometerReading)
        {
            if (headingAccurancy < 0)
            {
                throw new ArgumentOutOfRangeException("headingAccurancy");
            }
            if (trueHeading < 0)
            {
                throw new ArgumentOutOfRangeException("trueHeading");
            }
            if (magneticHeading < 0)
            {
                throw new ArgumentOutOfRangeException("magneticHeading");
            }

            HeadingAccuracy = headingAccurancy;
            MagneticHeading = magneticHeading;
            MagnetometerReading = magnetometerReading;
            Timestamp = timestamp;
            TrueHeading = trueHeading;
        }

        public CompassData(CompassReading compassReading)
            : this(compassReading.Timestamp, compassReading.HeadingAccuracy, compassReading.TrueHeading,
                   compassReading.MagneticHeading, compassReading.MagnetometerReading)
        {
        }

        #region Properties

        public double HeadingAccuracy { get; private set; }

        public double MagneticHeading { get; private set; }

        public Vector3 MagnetometerReading { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public double TrueHeading { get; private set; }

        #endregion
    }
}
