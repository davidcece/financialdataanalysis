namespace FinancialDataAnalysis.Dto
{
    public class GenericResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public GenericResponse(T data) {
            Data = data;
            Success = true;
            Message = "Success";
        }

        public GenericResponse(Exception ex)
        {
            Data = default;
            Success = false;
            Message = "Internal error occured "+ex.Message;
        }

    }
}
