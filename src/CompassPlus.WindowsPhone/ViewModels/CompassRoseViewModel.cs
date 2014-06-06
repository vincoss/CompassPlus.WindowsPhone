using System;
using System.Collections.Generic;
using System.Linq;
using CompassPlus.Filter;
using CompassPlus.Sensors;
using CompassPlus.Services;
using System.Globalization;

using CompassPlus.Framework.ViewModel;
using CompassPlus.Models;


namespace CompassPlus.ViewModels
{
    public class CompassRoseViewModel : ViewModelBase
    {
        private readonly ICompass _compass;
        private readonly ISettings _settings;
        private readonly IMessageService _messageService;

        private readonly ReadingSmoother _magneticHeadingSmoother;
        private readonly ReadingSmoother _trueHeadingSmoother;
        private readonly ReadingSmoother _headingAccuracySmoother;

        private readonly IFilterStrategy _filterStrategy = new LowPassFilterStrategy
            {
                NoiseThreshold = 0.05D
            };

        public CompassRoseViewModel(ICompass compass, ISettings settings, IMessageService messageService)
        {
            if (compass == null)
            {
                throw new ArgumentNullException("compass");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (messageService == null)
            {
                throw new ArgumentNullException("messageService");
            }

            _compass = compass;
            _settings = settings;
            _messageService = messageService;

            _magneticHeadingSmoother = new ReadingSmoother(_filterStrategy);
            _trueHeadingSmoother = new ReadingSmoother(_filterStrategy);
            _headingAccuracySmoother = new ReadingSmoother(_filterStrategy);

            _compass.CurrentValueChanged += Compass_CurrentValueChanged;

            this.PropertyChanged += CompassRoseViewModel_PropertyChanged;
        }

        private void CompassRoseViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Utils.GetPropertyName(() => SelectedCompassType))
            {
                this.IsPlane = this.SelectedCompassType == "Plane";
                this.IsShip = this.SelectedCompassType == "Ship";
                this.IsTrekking = this.SelectedCompassType == "Trekking";
            }
        }

        #region Public methods

        public override void Initialize()
        {
            this.Scale = 0;
            this.ColorList = Constants.Colors;
            this.CompassRoseList = Constants.CompasRoses;
            this.CompassTypeList = Constants.CompassTypes;

            OnCompassTypeCompleted();
            OnCompassColorCompleted();
            OnCompassRoseCompleted();
            OnTimeBetweenUpdates();

            this.IsInitialized = true;
        }

        public void Start()
        {
            if (_compass.IsSupported == false)
            {
                _messageService.ShowMessage("Compass is not supported on this device.", "Compass Plus");
                return;
            }

            _compass.Start();
        }

        public void Stop()
        {
            _compass.Stop();
        } 

        #endregion

        #region Event handlers

        private void Compass_CurrentValueChanged(object sender, CompassDataChangedEventArgs e)
        {
            UiAction(() =>
                {
                    // Compas not ready if view model not initialized.

                    if (this.IsInitialized == false)
                    {
                        return;
                    }

                    if (_compass.IsDataValid == false)
                    {
                        return;
                    }

                    var trueheading = _trueHeadingSmoother.ProcessReading(e.CompassData.TrueHeading);
                    var magneticHeading = _magneticHeadingSmoother.ProcessReading(e.CompassData.MagneticHeading);

                    // To negative so it rotates correctly

                    this.Angle = magneticHeading*-1;

                    var ratio = 1.0D;

                    if (SelectedCompassRose.Name == CompassRose.Rose6000Name)
                    {
                        ratio = Constants.Points6000/Constants.Points360;
                    }

                    if (SelectedCompassRose.Name == CompassRose.Rose6400Name)
                    {
                        ratio = Constants.Points6400/Constants.Points360;
                    }

                    trueheading = trueheading*ratio;
                    magneticHeading = magneticHeading*ratio;

                    TrueHeading = trueheading;
                    MagneticHeading = magneticHeading;
                    HeadingAccuracy = _headingAccuracySmoother.ProcessReading(Math.Abs(e.CompassData.HeadingAccuracy));
                });
        }

        #endregion

        #region Private methods

        private void OnCompassTypeCompleted()
        {
            var value = _settings.Get(StoreKeyConstants.CompassType);
            if (string.IsNullOrWhiteSpace(value))
            {
                // In case that settings was not found use default values
                value = Constants.DefaultCompassType;
            }
            this.SelectedCompassType = this.CompassTypeList.SingleOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
        }

        private void OnCompassColorCompleted()
        {
            var value = _settings.Get(StoreKeyConstants.CompassColor);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultCompassColor;
            }
            this.SelectedColor = this.ColorList.SingleOrDefault(x => string.Equals(x.Name, value, StringComparison.OrdinalIgnoreCase)).Value;
        }

        private void OnCompassRoseCompleted()
        {
            var value = _settings.Get(StoreKeyConstants.CompassRose);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultCompassRose;
            }
            this.SelectedCompassRose = this.CompassRoseList.SingleOrDefault(x => string.Equals(x.Name, value, StringComparison.OrdinalIgnoreCase));
        }

        private void OnTimeBetweenUpdates()
        {
            var value = _settings.Get(StoreKeyConstants.TimeBetweenUpdates);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultTimeBetweenUpdates.ToString(CultureInfo.InvariantCulture);
            }
            _compass.TimeBetweenUpdates = new TimeSpan(Utils.TryParse<int>(value, Int32.TryParse));
        }

        #endregion

        #region Public properties

        private double _scale;

        public double Scale
        {
            get { return _scale; }
            set
            {
                if (Utils.DoubleEquals(_scale, value, Constants.NotifyPropertyChangeDoubleAccurary))
                {
                    return;
                }
                _scale = value;
                NotifyOfPropertyChanged(() => Scale);
            }
        }

        private string _selectedColor;

        public string SelectedColor
        {
            get { return _selectedColor; }
            private set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    NotifyOfPropertyChanged(() => SelectedColor);
                }
            }
        }

        private string _selectedCompassType;

        public string SelectedCompassType
        {
            get { return _selectedCompassType; }
            private set
            {
               if (_selectedCompassType != value)
               {
                   _selectedCompassType = value;
                   NotifyOfPropertyChanged(() => SelectedCompassType);
               }
            }
        }

        private CompassRose _selectedCompassRose;

        public CompassRose SelectedCompassRose
        {
            get { return _selectedCompassRose; }
            private set
            {
                if (_selectedCompassRose != value)
                {
                    _selectedCompassRose = value;
                    NotifyOfPropertyChanged(() => SelectedCompassRose);
                }
            }
        }

        private IEnumerable<NameValue> _colorList;

        public IEnumerable<NameValue> ColorList
        {
            get { return _colorList; }
            private set
            {
                _colorList = value;
                NotifyOfPropertyChanged(() => ColorList);
            }
        }

        private IEnumerable<string> _compassTypeList;

        public IEnumerable<string> CompassTypeList
        {
            get { return _compassTypeList; }
            private set
            {
                _compassTypeList = value;
                NotifyOfPropertyChanged(() => CompassTypeList);
            }
        }

        private IEnumerable<CompassRose> _compassRoseList;

        public IEnumerable<CompassRose> CompassRoseList
        {
            get { return _compassRoseList; }
            private set
            {
                _compassRoseList = value;
                NotifyOfPropertyChanged(() => CompassRoseList);
            }
        }

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            private set
            {
                if (Utils.DoubleEquals(_angle, value, Constants.NotifyPropertyChangeDoubleAccurary))
                {
                    return;
                }
                _angle = value;
                NotifyOfPropertyChanged(() => Angle);
            }
        }

        private double _headingAccuracy;

        public double HeadingAccuracy
        {
            get { return _headingAccuracy; }
            private set
            {
                if (Utils.DoubleEquals(_headingAccuracy, value, Constants.NotifyPropertyChangeDoubleAccurary))
                {
                    return;
                }
                _headingAccuracy = value;
                NotifyOfPropertyChanged(() => HeadingAccuracy);
            }
        }

        private double _trueHeading;

        public double TrueHeading
        {
            get { return _trueHeading; }
            private set
            {
                if (Utils.DoubleEquals(_trueHeading, value, Constants.NotifyPropertyChangeDoubleAccurary))
                {
                    return;
                }
                _trueHeading = value;
                NotifyOfPropertyChanged(() => TrueHeading);
            }
        }

        private double _magneticHeading;

        public double MagneticHeading
        {
            get { return _magneticHeading; }
            private set
            {
                if (Utils.DoubleEquals(_magneticHeading, value, Constants.NotifyPropertyChangeDoubleAccurary))
                {
                    return;
                }
                _magneticHeading = value;
                NotifyOfPropertyChanged(() => MagneticHeading);
            }
        }

        #endregion

        private bool _isPlane;

        public bool IsPlane
        {
            get { return _isPlane; }
            set
            {
                if (_isPlane != value)
                {
                    _isPlane = value;
                    NotifyOfPropertyChanged(() => IsPlane);
                }
            }
        }

        private bool _isShip;

        public bool IsShip
        {
            get { return _isShip; }
            set
            {
                if (_isShip != value)
                {
                    _isShip = value;
                    NotifyOfPropertyChanged(() => IsShip);
                }
            }
        }

        private bool _isTrekking;

        public bool IsTrekking
        {
            get { return _isTrekking; }
            set
            {
                if (_isTrekking != value)
                {
                    _isTrekking = value;
                    NotifyOfPropertyChanged(() => IsTrekking);
                }
            }
        }
    }
}
