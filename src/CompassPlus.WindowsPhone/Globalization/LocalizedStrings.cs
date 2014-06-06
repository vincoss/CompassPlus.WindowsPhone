using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Threading;

using CompassPlus.Resources;


namespace CompassPlus.Globalization
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings : IEnumerable<KeyValuePair<string, string>>, INotifyPropertyChanged
    {
        private AppResources _resources;

        public LocalizedStrings()
        {
        }

        #region Public methods

        public LocalizedStrings WithLanguate(string locale)
        {
            if (string.IsNullOrWhiteSpace(locale))
            {
                throw new ArgumentNullException("locale");
            }
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            return this;
        }

        public void UpdateLanguageResources()
        {
            var strings = (LocalizedStrings)System.Windows.Application.Current.Resources["LocalizedStrings"];

            _resources = null;
            _resources = new AppResources();

            strings.OnPropertyChanged("Item[]"); // For accessor
            strings.OnPropertyChanged("LocalizedResources");
            
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            var manager = new ResourceManager(typeof(AppResources));
            ResourceSet resources = manager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator enumerator = resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return new KeyValuePair<string, string>((string)enumerator.Key, (string)enumerator.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Event handlers

        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        
        #region Properties

        public AppResources LocalizedResources
        {
            get
            {
                if (_resources == null)
                {
                    this.UpdateLanguageResources();
                }
                return _resources;
            }
        }

        public string this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException("key");
                }
                var pair = this.SingleOrDefault(x => x.Key == key);
                if (pair.Equals(default(KeyValuePair<string, string>)))
                {
                    return key;
                }
                return pair.Value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}