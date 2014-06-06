using System;


namespace CompassPlus.Framework.Commands
{
    public class DelegateCommand : Command
    {
        private readonly Action _excecute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute) : this(execute, () => true)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExcute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this._excecute = execute;
            this._canExecute = canExcute;
        }

        public override void Execute(object parameter)
        {
            _excecute();
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return true;
        }
    }
}
