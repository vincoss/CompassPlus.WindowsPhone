using System;
using System.ComponentModel;
using System.Windows.Threading;
using Microsoft.Xna.Framework;


namespace CompassPlus.Sensors
{
    public class MockCompass : ICompass
    {
        private bool _isStarted;
        private bool _isDataValid;
        private CompassData _currentValue;
        private TimeSpan _timeBetweenUpdates;

        private readonly Random _random;
        private readonly BackgroundWorker _worker;
        private readonly DispatcherTimer _dispatcherTimer;

        public MockCompass()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += OnDoWork;

            _random = new Random();

            this._dispatcherTimer = new DispatcherTimer();
            this._dispatcherTimer.Tick += OnTick;

            _timeBetweenUpdates = new TimeSpan(0, 0, 0, 0, 500);
           
            _currentValue = new CompassData(new DateTimeOffset(), 0, 0, 0, new Vector3());
        }

        #region Public methods

        public void Start()
        {
            if (_isStarted)
            {
                return;
            }

            this._dispatcherTimer.Interval = TimeBetweenUpdates;
            this._dispatcherTimer.Start();
            _isStarted = true;
        }

        public void Stop()
        {
            _isStarted = false;
            this._dispatcherTimer.Stop();
            _isDataValid = false;

            if (_worker.WorkerSupportsCancellation)
            {
                _worker.CancelAsync();
            }
        }

        public void Dispose()
        {
        }

        #endregion

        #region Event handlers

        private void OnTick(object sender, EventArgs e)
        {
            if (_worker.IsBusy == false)
            {
                _worker.RunWorkerAsync();
            }
        }

        private void OnDoWork(object sender, DoWorkEventArgs e)
        {
            _currentValue = new CompassData(
                new DateTimeOffset(DateTime.UtcNow),
                _random.NextDouble() * 25,
                (_currentValue.TrueHeading + 0.1) % 360,
                (_currentValue.MagneticHeading + 0.1) % 360,
                _currentValue.MagnetometerReading
                );

            _isDataValid = true;

            var handler = CurrentValueChanged;
            if (handler != null)
            {
                handler(this, new CompassDataChangedEventArgs(_currentValue));
            }
        } 

        private void OnCalibrate()
        {
            var handler = this.Calibrate;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Properties

        public bool IsSupported
        {
            get { return true; }
        }

        public bool IsDataValid
        {
            get { return _isDataValid; }
        }

        public CompassData CurrentValue
        {
            get { return _currentValue; }
        }

        public TimeSpan TimeBetweenUpdates
        {
            get { return _timeBetweenUpdates; }
            set
            {
                _timeBetweenUpdates = value;
                if (_isStarted)
                {
                    _dispatcherTimer.Interval = TimeBetweenUpdates;
                }
            }
        }

        public event EventHandler<EventArgs> Calibrate;

        public event EventHandler<CompassDataChangedEventArgs> CurrentValueChanged; 

        #endregion
    }
}
