using System;

namespace backend.Exceptions
{
    public abstract class NoSuchEntityException : Exception
    {
        protected NoSuchEntityException(string model, int id) : base("No such " + model + " with id: " + id) { }
    }
}