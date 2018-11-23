namespace Holistor.KeyReader.Dto
{
    public class ResponseLlaveEncrypDto : IResponseEncrypDto
    {
        public Encryptedpacket result { get; set; }
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public ErrorDto error { get; set; }
    }
}
