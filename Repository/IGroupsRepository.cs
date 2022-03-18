namespace Sql
{
    internal interface IGroupsRepository
    {
        void Add(Groups groups);
        List<Groups> GetAll();
        Groups GetById(int groupsId);
    }
}