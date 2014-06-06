using System;


namespace CompassPlus.Framework.ViewModel
{
    public class ViewModelBase : PropertyChangedBase
    {
        public virtual void Initialize()
        {
        }

        private bool _isInitialized;

        public bool IsInitialized
        {
            get { return _isInitialized; }
            set
            {
                if (_isInitialized != value)
                {
                    _isInitialized = value;
                    NotifyOfPropertyChanged(() => this.IsInitialized);
                }
            }
        }

        public Action<Action> UiAction;
    }
}
