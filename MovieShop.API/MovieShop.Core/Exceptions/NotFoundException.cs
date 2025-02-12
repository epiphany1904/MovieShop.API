using System;

namespace MovieShop.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Resource \"{name}\" ({key}) was not found.")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}