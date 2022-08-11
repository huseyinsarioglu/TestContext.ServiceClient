namespace TestContext.ServiceClient.APISandbox.Model
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string BirthPlace { get; set; } = string.Empty;

        public byte[]? Image { get; set; }
    }
}
