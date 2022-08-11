namespace TestContext.ServiceClient.APISandbox.Model
{
    public class StudentDummyContext : IStudentContext
    {
        private readonly List<Student> _students;

        public StudentDummyContext()
        {
            _students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    BirthDate = DateTime.Today.AddMonths(-88),
                    BirthPlace = "NearSomewhere",
                    Name = "John Everick"
                },
                new Student
                {
                    Id = 2,
                    BirthDate = DateTime.Today.AddMonths(-87).AddDays(5),
                    BirthPlace = "NearSomewhere",
                    Name = "Dennis Barb"
                },
                new Student
                {
                    Id = 3,
                    BirthDate = DateTime.Today.AddMonths(-85).AddDays(15),
                    BirthPlace = "NearSomewhere",
                    Name = "Nicole Carr"
                },
                new Student
                {
                    Id = 4,
                    BirthDate = DateTime.Today.AddMonths(-89).AddDays(-21),
                    BirthPlace = "NearSomewhere",
                    Name = "Caroline Smith"
                }
            };
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            var student =  _students.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(student);
        }

        public Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return Task.FromResult(_students.AsEnumerable());
        }

        public Task AddStudentAsync(Student student)
        {
            student.Id = _students.Max(s => s.Id) + 1;
            _students.Add(student);
            return Task.CompletedTask;
        }

        public Task<bool> UpdateStudentAsync(Student student)
        {
            var index = _students.FindIndex(s => s.Id == student.Id);
            if (index == -1)
            {
                return Task.FromResult(false);
            }

            _students[index] = student;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            var index = _students.FindIndex(s => s.Id == id);
            if (index == -1)
            {
                return Task.FromResult(false);
            }

            _students.RemoveAt(index);
            return Task.FromResult(true);
        }
    }
}
