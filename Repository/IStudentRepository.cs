namespace Sql
{
    internal interface IStudentRepository
    {
        void Add(Student student);
        List<Student> GetAll();
        Student GetById(int studentId);
    }
}