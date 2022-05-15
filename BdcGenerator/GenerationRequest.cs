namespace BdcGenerator
{
    public class GenerationRequest
    {
        public string ModelPath { get; set; }
        public string PhotoFolder { get; set; }
        public string OutputFolder { get; set; }
    }
    public class GenerationResponse
    {
        public int FileCount { get; set; }
        public string OutputFolder { get; set; }
    }
}
