using System;

namespace Business
{
    public sealed class DuplicateCustomerNameException : Exception
    {
        public DuplicateCustomerNameException()
            : base("Ya existe un cliente con el mismo nombre.")
        {
        }
    }
}
