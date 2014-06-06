using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;


namespace CompassPlus.Services
{
    public interface ISettings
    {
        string Get(string key);
        void Add(string key, string value);
        void AddOrUpdate(string key, string value);

        void Get(string key, Action<string> completed);
        void AddOrUpdate(string key, string value, Action completed);
        void Add(string key, string value, Action completed);
        void Clear();
    }

    class MockSettings : ISettings
    {
        private static readonly Dictionary<string, string> Settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public void AddOrUpdate(string key, string value, Action completedCallback)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            Task.Factory.StartNew(() =>
            {

                if (Settings.ContainsKey(key))
                {
                    Settings[key] = value;
                }
                else
                {
                    Settings.Add(key, value);
                }
                completedCallback();
            });
        }


        public void Get(string key, Action<string> completed)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            Task.Factory.StartNew(() =>
            {
                string value = null;
                if (Settings.ContainsKey(key))
                {
                    value = Settings[key];
                }
                completed(value);
            });
        }

        public void Add(string key, string value, Action completed)
        {
            throw new NotImplementedException();
        }

        public string Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(string key, string value)
        {
            throw new NotImplementedException();
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }
    }

    class IsolatedSettings : ISettings
    {
        public void Get(string key, Action<string> completed)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (completed == null)
            {
                throw new ArgumentNullException("completed");
            }
            var settings = GetSettings();
            string value = string.Empty;
            if (settings.Contains(key))
            {
                value = settings[key].ToString();
            }
            completed(value);
        }

        public void AddOrUpdate(string key, string value, Action completed)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (completed == null)
            {
                throw new ArgumentNullException("completed");
            }
            AddOrUpdate(key, value);
            completed();
        }

        public void Add(string key, string value, Action completed)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (completed == null)
            {
                throw new ArgumentNullException("completed");
            }
            var settings = GetSettings();
            if (settings.Contains(key) == false)
            {
                settings.Add(key, value);
                settings.Save();
            }
            completed(); 
        }

        public string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            var settings = GetSettings();
            string value = null;
            if (settings.Contains(key))
            {
                value = (string)settings[key];
            }
            return value;
        }

        public void Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            var settings = GetSettings();
            if (settings.Contains(key) == false)
            {
                settings.Add(key, value);
                settings.Save();
            }
        }

        public void AddOrUpdate(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            } 
            var settings = GetSettings();
            if (settings.Contains(key) == false)
            {
                settings.Add(key, value);
            }
            else
            {
                // Update only if changed
                var original = (string)settings[key];
                if (string.Equals(original, value, StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    settings[key] = value;
                }
            }
            settings.Save();
        }

        public void Clear()
        {
            var settings = GetSettings();
            settings.Clear();
            settings.Save();
        }

        private IsolatedStorageSettings GetSettings()
        {
            return IsolatedStorageSettings.ApplicationSettings;
        }
    }
}
