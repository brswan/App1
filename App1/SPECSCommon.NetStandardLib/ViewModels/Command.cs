using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Reflection;

namespace SPECSCommon.NetStandardLib.ViewModels
{
    public sealed class Command<T> : Command
    {
        public Command(Action<T> execute)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            })
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            }, o => IsValidParameter(o) && canExecute((T)o))
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        static bool IsValidParameter(object o)
        {
            if (o != null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                return o is T;
            }

            var t = typeof(T);

            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }

            //TODO: Wait until netstandard 2.0 for this method to return
            // Not a Nullable, if it's a value type then null is not valid
            //return !t.GetType().IsValueType();

            return true;
        }
    }

    public class Command : ICommand
    {
        private readonly Func<object, bool> m_CanExecute;
        private readonly Action<object> m_Execute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            m_Execute = execute;
        }

        public Command(Action execute) : this(o => execute())
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        public Command(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            m_CanExecute = canExecute;
        }

        public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        public bool CanExecute(object parameter)
        {
            if (m_CanExecute != null)
            {
                return m_CanExecute(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            m_Execute(parameter);
        }

        public void ChangeCanExecute()
        {
            EventHandler changed = CanExecuteChanged;
            changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
