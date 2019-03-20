using System;

namespace Interfaces.Logic
{
    public interface IExceptionHelper
    {
        string GetExceptionInfo(Exception exception);
    }
}