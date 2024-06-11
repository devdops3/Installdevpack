namespace MA2AAPI.Models
{
    public class S3PhotoResponse
    {
        public string PhotoUrl { get; set; }
        public string ResCode { get; set; }
        public string ResDescription { get; set; }
    }

    public class S3PhotoPresignedUrlResponse
    {
        public string PresignedUrl { get; set; }
        public string ResCode { get; set; }
        public string ResDescription { get; set; }
    }
}