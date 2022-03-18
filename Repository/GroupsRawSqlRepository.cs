using Sql;
using System.Data;
using System.Data.SqlClient;

public class GroupsRawSqlRepository : IGroupsRepository
{
    private string _connectionString;

    public GroupsRawSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Groups> GetAll()
    {
        var result = new List<Groups>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "select [Id], [Name] from [Groups]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Groups
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"])
                        });
                    }
                }
            }
        }

        return result;
    }

    public void Add(Groups groups)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"insert into [Groups]
                            ([Name])
                        values
                            (@name)
                        select SCOPE_IDENTITY()";

                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = groups.Name;

                groups.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    public Groups GetById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    @"select [Id], [Name]
                        from [Groups]
                        where [Id] = @id";

                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Groups
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"])
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}

    