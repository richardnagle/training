namespace reviews_service
{
    public interface IObserveSaving
    {
        void ReviewSaved();
        void ReviewNotSaved(int httpStatusCode, string errorMessage);
    }
}