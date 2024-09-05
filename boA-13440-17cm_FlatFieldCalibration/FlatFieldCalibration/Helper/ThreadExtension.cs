using System.Windows;
using System.Windows.Threading;

namespace FlatFieldCalibration.Helper;

public static class ThreadExtension
{
    /// <summary>
    /// Gets the UI dispatcher.
    /// </summary>
    /// <value>The UI dispatcher.</value>
    private static Dispatcher UiDispatcher => Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

    /// <summary>
    /// Dispatches to UI.
    /// </summary>
    /// <param name="caller">The caller.</param>
    /// <param name="method">The method.</param>
    /// <param name="priority">The priority.</param>
    public static void DispatchToUi(this object caller, Action method, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        try
        {
            if (UiDispatcher.CheckAccess())
            {
                method.Invoke();
            }
            else
            {
                UiDispatcher.Invoke(method, priority);
            }
        }
        catch
        {
            // Previously the exception was ignored in this section of the code. I reverted this. 
            // The following problems were caused by that decision:
            //
            // Not responsible for the issue but responsible for the remote command not signaling a proper error.
            //
            // I need this exception! Do not catch and eat! The logic of some
            // remote commands is dispatched to th UI thread! If you catch and eat those
            // Commands will never return a proper error.

            // Catch the exception outside!
            throw;
        }
    }

    public static void DispatchToUiAsync(this object caller, Action method, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        if (UiDispatcher.CheckAccess())
        {
            // Dear past me: You are an idiot! DO NOT USE BeginInvoke! It will fuck things up!
            // If I'm on the main thread Invoke should be used. I'm already on the main thread! No deadlock potential! 
            // BeginInvoke will dispatch to ANOTHER thread than the UI thread!
            //
            // I'll leave my original comment as warning for the next time I feel like changing this code:
            // Changed to BeginInvoke. (From Invoke; Invoke seems dubious here. I Suspect a copy & paste error)
            method.Invoke();
        }
        else
        {
            UiDispatcher.BeginInvoke(method, priority);
        }
    }
}