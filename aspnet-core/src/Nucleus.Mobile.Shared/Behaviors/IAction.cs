using Xamarin.Forms.Internals;

namespace Nucleus.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}