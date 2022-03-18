namespace Sql
{
    internal interface IStudentInGroupsRepository
    {
        List<StudentInGroups> GetByStudentIdAndGroupsId();
        void Add(StudentInGroups studentInGroups);
    }
}