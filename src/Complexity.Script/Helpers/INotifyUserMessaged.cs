using System;

namespace Complexity.Helpers
{
    public interface INotifyUserMessaged
    {
        event EventHandler<UserMessagedEventArgs> UserMessaged;
    }
}
