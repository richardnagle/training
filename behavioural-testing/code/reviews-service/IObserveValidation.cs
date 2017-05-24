namespace reviews_service
{
    public interface IObserveValidation
    {
        void ReviewFailedValidation(int httpStatusCode, string errorMessage);
    }
}