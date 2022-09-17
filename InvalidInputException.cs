namespace MusicUploader
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string input)
            : base($"\"{input}\" is not a valid option.")
        { }
    }
}
