namespace reviews_service.infrastructure
{
    public interface IHandle<T>
    {
        Response Handle(Request<T> request);
    }
}
