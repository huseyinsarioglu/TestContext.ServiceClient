namespace TestContext.ServiceClient.APISandbox.Model
{
    public interface IStudentContext
    {
        Task<IEnumerable<Student>> GetStudentsAsync();

        Task<Student?> GetStudentByIdAsync(int id);

        Task AddStudentAsync(Student student);

        Task<bool> UpdateStudentAsync(Student student);

        Task<bool> DeleteStudentAsync(int id);
    }
}
