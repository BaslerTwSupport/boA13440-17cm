﻿using System.Linq.Expressions;
using System.Reflection;

namespace FlatFieldCalibration.Models;

public class BaslerCommand : DelegateCommandBase
{
    private readonly Action _executeMethod;
    private Func<bool> _canExecuteMethod;
    //private readonly MessageBoxViewModel _messageBox;

    /// <summary>
    /// Creates a new instance of <see cref="DelegateCommand"/> with the <see cref="Action"/> to invoke on execution.
    /// </summary>
    /// <param name="executeMethod">The <see cref="Action"/> to invoke when <see cref="ICommand.Execute(object)"/> is called.</param>
    public BaslerCommand(Action executeMethod)
        : this(executeMethod, () => true)
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="DelegateCommand"/> with the <see cref="Action"/> to invoke on execution
    /// and a <see langword="Func" /> to query for determining if the command can execute.
    /// </summary>
    /// <param name="executeMethod">The <see cref="Action"/> to invoke when <see cref="ICommand.Execute"/> is called.</param>
    /// <param name="canExecuteMethod">The <see cref="Func{TResult}"/> to invoke when <see cref="ICommand.CanExecute"/> is called</param>
    public BaslerCommand(Action executeMethod, Func<bool> canExecuteMethod)
    {
        //_messageBox = ContainerLocator.Container.Resolve<MessageBoxViewModel>();
        if (executeMethod == null || canExecuteMethod == null)
            throw new ArgumentNullException(nameof(executeMethod));

        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    ///<summary>
    /// Executes the command.
    ///</summary>
    public void Execute()
    {
        try
        {
            _executeMethod();
        }
        catch (Exception ex)
        {
            //LogHelper.Error($"{_executeMethod.Method.Name}, {ex}", ex);
            //_messageBox.ShowAsync("Error", $"{ex}");
        }
    }

    /// <summary>
    /// Determines if the command can be executed.
    /// </summary>
    /// <returns>Returns <see langword="true"/> if the command can execute,otherwise returns <see langword="false"/>.</returns>
    public bool CanExecute()
    {
        try
        {
            return _canExecuteMethod();
        }
        catch (Exception ex)
        {
            //LogHelper.Error($"{_executeMethod.Method.Name}, {ex}", ex);
            //_messageBox.ShowAsync("Error", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Handle the internal invocation of <see cref="ICommand.Execute(object)"/>
    /// </summary>
    /// <param name="parameter">Command Parameter</param>
    protected override void Execute(object parameter)
    {
        Execute();
    }

    /// <summary>
    /// Handle the internal invocation of <see cref="ICommand.CanExecute(object)"/>
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns><see langword="true"/> if the Command Can Execute, otherwise <see langword="false" /></returns>
    protected override bool CanExecute(object parameter)
    {
        return CanExecute();
    }

    /// <summary>
    /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
    /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public BaslerCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
    {
        ObservesPropertyInternal(propertyExpression);
        return this;
    }

    /// <summary>
    /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public BaslerCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
    {
        _canExecuteMethod = canExecuteExpression.Compile();
        ObservesPropertyInternal(canExecuteExpression);
        return this;
    }
}

/// <summary>
/// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/>.
/// </summary>
/// <typeparam name="T">Parameter type.</typeparam>
/// <remarks>
/// The constructor deliberately prevents the use of value types.
/// Because ICommand takes an object, having a value type for T would cause unexpected behavior when CanExecute(null) is called during XAML initialization for command bindings.
/// Using default(T) was considered and rejected as a solution because the implementor would not be able to distinguish between a valid and defaulted values.
/// <para/>
/// Instead, callers should support a value type by using a nullable value type and checking the HasValue property before using the Value property.
/// <example>
///     <code>
/// public MyClass()
/// {
///     this.submitCommand = new DelegateCommand&lt;int?&gt;(this.Submit, this.CanSubmit);
/// }
/// 
/// private bool CanSubmit(int? customerId)
/// {
///     return (customerId.HasValue &amp;&amp; customers.Contains(customerId.Value));
/// }
///     </code>
/// </example>
/// </remarks>
public class BaslerCommand<T> : DelegateCommandBase
{
    readonly Action<T> _executeMethod;
    Func<T, bool> _canExecuteMethod;
    //private readonly MessageBoxViewModel? _messageBox;

    /// <summary>
    /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
    /// </summary>
    /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
    /// <remarks><see cref="CanExecute(T)"/> will always return true.</remarks>
    public BaslerCommand(Action<T> executeMethod)
        : this(executeMethod, (_) => true)
    {
        //_messageBox = ContainerLocator.Container.Resolve<MessageBoxViewModel>();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
    /// </summary>
    /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
    /// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command. This can be null.</param>
    /// <exception cref="ArgumentNullException">When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> are <see langword="null" />.</exception>
    public BaslerCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    {
        if (executeMethod == null || canExecuteMethod == null)
            throw new ArgumentNullException(nameof(executeMethod));

        TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

        // DelegateCommand allows object or Nullable<>.  
        // note: Nullable<> is a struct so we cannot use a class constraint.
        if (genericTypeInfo.IsValueType)
        {
            if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
            {
                throw new InvalidCastException("Delegate command invalid generic payloadType");
            }
        }

        _executeMethod = executeMethod;
        _canExecuteMethod = canExecuteMethod;
    }

    ///<summary>
    ///Executes the command and invokes the <see cref="Action{T}"/> provided during construction.
    ///</summary>
    ///<param name="parameter">Data used by the command.</param>
    public void Execute(T parameter)
    {
        try
        {
            _executeMethod(parameter);
        }
        catch (Exception ex)
        {
            //LogHelper.Error($"{_executeMethod.Method.Name}, {ex}", ex);
            //_messageBox?.ShowAsync("Error", ex.Message);
        }
    }

    ///<summary>
    ///Determines if the command can execute by invoked the <see cref="Func{T,Bool}"/> provided during construction.
    ///</summary>
    ///<param name="parameter">Data used by the command to determine if it can execute.</param>
    ///<returns>
    ///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    ///</returns>
    public bool CanExecute(T parameter)
    {
        return _canExecuteMethod(parameter);
    }

    /// <summary>
    /// Handle the internal invocation of <see cref="ICommand.Execute(object)"/>
    /// </summary>
    /// <param name="parameter">Command Parameter</param>
    protected override void Execute(object parameter)
    {
        Execute((T)parameter);
    }

    /// <summary>
    /// Handle the internal invocation of <see cref="ICommand.CanExecute(object)"/>
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns><see langword="true"/> if the Command Can Execute, otherwise <see langword="false" /></returns>
    protected override bool CanExecute(object parameter)
    {
        return CanExecute((T)parameter);
    }

    /// <summary>
    /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <typeparam name="TType">The type of the return value of the method that this delegate encapsulates</typeparam>
    /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public BaslerCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
    {
        ObservesPropertyInternal(propertyExpression);
        return this;
    }

    /// <summary>
    /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public BaslerCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
    {
        Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
        _canExecuteMethod = expression.Compile();
        ObservesPropertyInternal(canExecuteExpression);
        return this;
    }
}