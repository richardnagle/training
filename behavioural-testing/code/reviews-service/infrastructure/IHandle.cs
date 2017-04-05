namespace reviews_service.infrastructure
{
    public interface IHandle<T>
    {
        void Handle(Request<T> request);
    }
}
