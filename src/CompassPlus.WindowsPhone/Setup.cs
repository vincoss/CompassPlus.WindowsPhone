using System;
using System.Linq;
using System.Globalization;

using CompassPlus.Services;
using CompassPlus.Globalization;


namespace CompassPlus
{
    public class Setup
    {
        private readonly ISettings _settings;

        public Setup(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            _settings = settings;
        }

        public Setup Clear()
        {
            _settings.Clear();
            return this;
        }

        public Setup LoadDefaultSettings()
        {
            _settings.Add(StoreKeyConstants.CompassType, Constants.DefaultCompassType);
            _settings.Add(StoreKeyConstants.CompassColor, Constants.DefaultCompassColor);
            _settings.Add(StoreKeyConstants.CompassRose, Constants.DefaultCompassRose);
            _settings.Add(StoreKeyConstants.TimeBetweenUpdates, Constants.DefaultTimeBetweenUpdates.ToString(CultureInfo.InvariantCulture));
            _settings.Add(StoreKeyConstants.CollectErrors, Constants.DefaultCollectErrors.ToString());
            _settings.Add(StoreKeyConstants.Language, Constants.DefaultLanguage);
            return this;
        }

        public Setup WithCurrentCulture()
        {
            var value = _settings.Get(StoreKeyConstants.Language);
            var language = Constants.Languages.Single(x => string.Equals(x.Name, value, StringComparison.OrdinalIgnoreCase));
            new LocalizedStrings().WithLanguate(language.Value);
            return this;
        }
    }
}
