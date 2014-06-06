using System;
using Microsoft.Devices.Sensors;


namespace CompassPlus.Sensors
{
    public class DeviceCompass : ICompass
    {
        private Compass _compass;

        #region Public methods

        public void Start()
        {
            if (_compass != null)
            {
                return;
            }

            if (Compass.IsSupported == false)
            {
                return;
            }

            _compass = new Compass
            {
                TimeBetweenUpdates = TimeBetweenUpdates
            };

            // The sensor may not support the requested time between updates.
            // The TimeBetweenUpdates property reflects the actual rate.

            TimeBetweenUpdates = _compass.TimeBetweenUpdates;

            _compass.CurrentValueChanged += CompassCurrentValueChanged;
            _compass.Calibrate += CompassCalibrate;
            _compass.Start();
        }

        public void Stop()
        {
            this.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            if (this._compass != null)
            {
                _compass.Calibrate -= CompassCalibrate;
                _compass.CurrentValueChanged -= CompassCurrentValueChanged;

                _compass.Stop();
                _compass.Dispose();
                _compass = null;
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Event handlers
        
        private void CompassCalibrate(object sender, CalibrationEventArgs e)
        {
            var handler = Calibrate;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private void CompassCurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            var handler = CurrentValueChanged;
            if (handler != null)
            {
                handler(sender, new CompassDataChangedEventArgs(new CompassData(e.SensorReading)));
            }
        }

        #endregion

        #region Properties
        
        public bool IsSupported
        {
            get { return Compass.IsSupported; }
        }

        public bool IsDataValid
        {
            get { return _compass != null && _compass.IsDataValid; }
        }

        public TimeSpan TimeBetweenUpdates { get; set; }
        
        public CompassData CurrentValue
        {
            get { return _compass != null ? new CompassData(_compass.CurrentValue) : default(CompassData); }
        }

        public event EventHandler<EventArgs> Calibrate;

        public event EventHandler<CompassDataChangedEventArgs> CurrentValueChanged;

        #endregion
    }
}
