using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using CompassPlus.Framework.ViewModel;
using CompassPlus.Models;
using CompassPlus.Services;
using CompassPlus.Framework.Commands;
using CompassPlus.Globalization;


namespace CompassPlus.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettings _settings;
        private readonly IMessageService _messageService;

        public SettingsViewModel(ISettings settings, IMessageService messageService)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (messageService == null)
            {
                throw new ArgumentNullException("messageService");
            }
            _settings = settings;
            _messageService = messageService;

            this.RestoreCommand = new DelegateCommand(OnRestoreCommand);

            this.PropertyChanged += SettingsViewModel_PropertyChanged;
        }
        
        #region Public methods

        public override void Initialize()
        {
            // Set to null to ensure that control rebind the values when locale changes.
            this.CompassColors = null;
            this.CompassTypes = null;
            this.CompassRoses = null;
            this.Languages = null;

            this.CharFilter = ".-,";
            this.CompassColors = Constants.Colors;
            this.CompassTypes = Constants.CompassTypes;
            this.CompassRoses = Constants.CompasRoses.Select(s => s.Name).ToArray();
            this.Languages = Constants.Languages;

            this.LoadSettings();

            // TODO: if async this will not work.

            this.IsInitialized = true;
        }

        #endregion

        #region Private methods

        private void LoadSettings()
        {
            GetCompassColor();
            GetCompassType();
            GetCompassRose();
            GetTimeBetweenUpdates();
            GetCollectErrors();
            GetLanguage();
        }

        private void GetCompassColor()
        {
            var value = _settings.Get(StoreKeyConstants.CompassColor);
            if (string.IsNullOrWhiteSpace(value))
            {
                // In case that settings was not found use default values, fail safe.
                value = Constants.DefaultCompassColor;
            }
            this.CompassColor = this.CompassColors.SingleOrDefault(x => string.Equals(x.Name, value, StringComparison.OrdinalIgnoreCase));
        }

        private void GetCompassType()
        {
            var value = _settings.Get(StoreKeyConstants.CompassType);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultCompassType;
            }
            this.CompassType = this.CompassTypes.SingleOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
        }

        private void GetCompassRose()
        {
            var value = _settings.Get(StoreKeyConstants.CompassRose);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultCompassRose;
            }
            this.CompassRose = this.CompassRoses.SingleOrDefault(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
        }

        private void GetTimeBetweenUpdates()
        {
            var value = _settings.Get(StoreKeyConstants.TimeBetweenUpdates);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultTimeBetweenUpdates.ToString(CultureInfo.InvariantCulture);
            }
            this.TimeBetweenUpdates = Utils.TryParse<int>(value, Int32.TryParse);
        }

        private void GetCollectErrors()
        {
            var value = _settings.Get(StoreKeyConstants.CollectErrors);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultCollectErrors.ToString();
            }
            this.CollectErrors = Utils.TryParse<bool>(value, Boolean.TryParse);
        }

        private void GetLanguage()
        {
            var value = _settings.Get(StoreKeyConstants.Language);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Constants.DefaultLanguage;
            }
            this.Language = Languages.SingleOrDefault(x => string.Equals(x.Name, value, StringComparison.OrdinalIgnoreCase));
        } 

        #endregion

        #region Event handlers

        private void SettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Here we want to store settings after initialize is completed.

            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => CompassType))
            {
                _settings.AddOrUpdate(StoreKeyConstants.CompassType, this.CompassType);
            }
            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => CompassColor))
            {
                _settings.AddOrUpdate(StoreKeyConstants.CompassColor, this.CompassColor.ToString());
            }
            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => CompassRose))
            {
                _settings.AddOrUpdate(StoreKeyConstants.CompassRose, this.CompassRose);
            }
            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => TimeBetweenUpdates))
            {
                _settings.AddOrUpdate(StoreKeyConstants.TimeBetweenUpdates, this.TimeBetweenUpdates.ToString(CultureInfo.InvariantCulture));
            }
            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => CollectErrors))
            {
                _settings.AddOrUpdate(StoreKeyConstants.CollectErrors, this.CollectErrors.ToString(CultureInfo.InvariantCulture));
            }
            if (IsInitialized && e.PropertyName == Utils.GetPropertyName(() => Language))
            {
                _settings.AddOrUpdate(StoreKeyConstants.Language, this.Language.Name);
            }

            if (e.PropertyName == Utils.GetPropertyName(() => Language))
            {
                new LocalizedStrings().WithLanguate(Language.Value).UpdateLanguageResources();
                this.Initialize();
            }
        }

        #endregion

        #region CommandMethods

        private void OnRestoreCommand()
        {
            var result = _messageService.ShowMessageOkCancel(new LocalizedStrings()["RestoreMessage"], null);
            if (result == false)
            {
                return;
            }

            new Setup(new IsolatedSettings()).Clear().LoadDefaultSettings();

            // Just reload new settings

            this.LoadSettings();
        }

        #endregion

        #region Commands

        public ICommand RestoreCommand { get; private set; } 

        #endregion

        #region Properties

        public string CharFilter { get; private set; }

        private IEnumerable<string> _compassRoses;

        public IEnumerable<string> CompassRoses
        {
            get { return _compassRoses; }
            private set
            {
                _compassRoses = value;
                NotifyOfPropertyChanged(() => CompassRoses);
            }
        }

        private string _compassRose;

        public string CompassRose
        {
            get { return _compassRose; }
            set
            {
                if (_compassRose != value)
                {
                    _compassRose = value;
                    NotifyOfPropertyChanged(() => CompassRose);
                }
            }
        }

        private IEnumerable<string> _compassTypes;

        public IEnumerable<string> CompassTypes
        {
            get { return _compassTypes; }
            private set
            {
                _compassTypes = value;
                NotifyOfPropertyChanged(() => CompassTypes);
            }
        }

        private string _compassType;

        public string CompassType
        {
            get { return _compassType; }
            set
            {
                if (_compassType != value)
                {
                    _compassType = value;
                    NotifyOfPropertyChanged(() => CompassType);
                }
            }
        }

        private IEnumerable<NameValue> _compassColors;

        public IEnumerable<NameValue> CompassColors
        {
            get { return _compassColors; }
            private set
            {
                _compassColors = value;
                NotifyOfPropertyChanged(() => CompassColors);
            }
        }

        private NameValue _compassColor;

        public NameValue CompassColor
        {
            get { return _compassColor; }
            set
            {
                if (_compassColor != value)
                {
                    _compassColor = value;
                    NotifyOfPropertyChanged(() => CompassColor);
                }
            }
        }

        private int _timeBetweenUpdates;

        public int TimeBetweenUpdates
        {
            get { return _timeBetweenUpdates; }
            set
            {
                if (_timeBetweenUpdates != value)
                {
                    if (value < 0)
                    {
                        value = Constants.DefaultTimeBetweenUpdates;
                    }
                    _timeBetweenUpdates = value;
                    NotifyOfPropertyChanged(() => TimeBetweenUpdates);
                }
            }
        }

        private bool _collectErrors;

        public bool CollectErrors
        {
            get { return _collectErrors; }
            set
            {
                if (_collectErrors != value)
                {
                    _collectErrors = value;
                    NotifyOfPropertyChanged(() => CollectErrors);
                }
            }
        }

        private IEnumerable<NameValue> _languages;

        public IEnumerable<NameValue> Languages
        {
            get { return _languages; }
            private set
            {
                _languages = value;
                NotifyOfPropertyChanged(() => Languages);
            }
        }

        private NameValue _language;

        public NameValue Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    NotifyOfPropertyChanged(() => Language);
                }
            }
        }

        #endregion
    }
}
