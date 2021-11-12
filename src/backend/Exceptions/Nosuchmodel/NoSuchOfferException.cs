namespace backend.Exceptions
{
    public class NoSuchOfferException : NoSuchEntityException
    {
        public NoSuchOfferException(int id): base("offer", id) { }
    }
}