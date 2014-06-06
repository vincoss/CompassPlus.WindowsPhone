using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;


namespace CompassPlus.Framework.ViewModel
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region Public methods

        /// <summary>
        /// Notifies all subscribers of the property change.
        /// </summary>
        public void NotifyOfPropertyChanged()
        {
            NotifyOfPropertyChanged(string.Empty);
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyOfPropertyChanged(string propertyName)
        {
            OnUiThread(() => RaisePropertyChangedEventCore(propertyName));
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        public void NotifyOfPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            NotifyOfPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyOfPropertyChangedImmediately(string propertyName)
        {
            RaisePropertyChangedEventCore(propertyName);
        }

        #endregion

        #region Private methods

        private void RaisePropertyChangedEventCore(string propertyName)
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return property.Name;
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            var type = this.GetType();
            if (string.IsNullOrEmpty(propertyName) == false && type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        private void OnUiThread(Action action)
        {
            action();

            // TODO: remove this we dont want this functionality.

            //Exception exception = null;
            //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
            //    try
            //    {
            //        action();
            //    }
            //    catch (Exception ex)
            //    {
            //        exception = ex;
            //    }
            //});
            //if (exception != null)
            //{
            //    throw exception;
            //}
        }

        #endregion

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}
