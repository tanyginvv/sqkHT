using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Sql
{
    class Program
    {
        private static string _connectionString = @"Data Source=LAPTOP-LJAM8EEK\SQLEXPRESS;Initial Catalog=university;Pooling=true;Integrated Security=SSPI";

        static void Main(string[] args)
        {
            IStudentRepository studentRepository = new StudentRawSqlRepository(_connectionString);
            IGroupsRepository groupsRepository = new GroupsRawSqlRepository(_connectionString);
            IStudentInGroupsRepository studentInGroupsRepository = new StudentInGroupsRawSqlRepository(_connectionString);

            Console.WriteLine("Доступные команды:");
            Console.WriteLine("add-student - Добавить студента");
            Console.WriteLine("add-group - Добавить группу");
            Console.WriteLine("add-student-in-group - Добавить студента в группу");
            Console.WriteLine("get-students - Вывести список студентов");
            Console.WriteLine("get-groups - Вывести список групп");
            Console.WriteLine("get-students-in-group - Вывести студентов по Id группы");
            Console.WriteLine("report - отчёт");
            Console.WriteLine("exit - выйти из приложения");

            while (true)
            {
                string command = Console.ReadLine();

                if (command == "add-student")
                {
                    Console.WriteLine("Введите имя студента");
                    string name = Console.ReadLine();
                    if (String.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Имя не может быть пустым");
                        continue;
                    }
                    Console.WriteLine("Введите возраст студента");
                    if (!Int32.TryParse(Console.ReadLine(), out int age))
                    {
                        Console.WriteLine("Вводите возраст цифрами");
                        continue;
                    }

                    studentRepository.Add(new Student
                    {
                        Name = name,
                        Age = age
                    });
                    Console.WriteLine("Success");
                }
                else if (command == "add-group")
                {
                    Console.WriteLine("Введите имя группы:");
                    string name = Console.ReadLine();
                    if (String.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("Имя не может быть пустым!");
                        continue;
                    }

                    groupsRepository.Add(new Groups
                    {
                        Name = name
                    });
                    Console.WriteLine("Success");
                }

                else if (command == "add-student-in-group")
                {
                    Console.WriteLine("Введите id студента которого хотите добавить в группу:");
                    if (!Int32.TryParse(Console.ReadLine(), out int studentId))
                    {
                        Console.WriteLine("Вводите id цифрами!");
                        continue;
                    }
                    Console.WriteLine("Введите id группы в которую хотите добавить студента:");
                    if (!Int32.TryParse(Console.ReadLine(), out int groupsId))
                    {
                        Console.WriteLine("Вводите id цифрами!");
                        continue;
                    }

                    List<StudentInGroups> studentInGroups = studentInGroupsRepository.GetByStudentIdAndGroupsId();
                    if (!((studentInGroups.Exists((StudentInGroups x) => (x.StudentId == studentId))) & (studentInGroups.Exists((StudentInGroups x) => (x.GroupsId == groupsId)))))
                    {
                        studentInGroupsRepository.Add(new StudentInGroups
                        {
                            StudentId = studentId,
                            GroupsId = groupsId
                        });
                        Console.WriteLine("Success");
                    }
                    else
                    {
                        Console.WriteLine("Студент уже есть в группе!");
                    }
                }
                else if (command == "get-students")
                {
                    var students = studentRepository.GetAll();
                    if (students.Count < 1)
                    {
                        Console.WriteLine("Ни одного студента не обнаружено");
                        continue;
                    }
                    foreach (Student student in students)
                    {
                        Console.WriteLine($"Id: {student.Id}, Name: {student.Name}");
                    }
                }

                else if (command == "get-groups")
                {
                    var groupss = groupsRepository.GetAll();
                    if (groupss.Count < 1)
                    {
                        Console.WriteLine("Ни одного студента не обнаружено");
                        continue;
                    }
                    foreach (Groups groups in groupss)
                    {
                        Console.WriteLine($"Id: {groups.Id}, Name: {groups.Name}");
                    }
                }
                else if (command == "get-students-in-group")
                {
                    Console.WriteLine("Введите id группы в которой хотите узнать студентов");
                    if (!Int32.TryParse(Console.ReadLine(), out int groupsId))
                    {
                        Console.WriteLine("Вводите id цифрами");
                        continue;
                    }
                    if (groupsRepository.GetById(groupsId) == null)
                    {
                        Console.WriteLine("Группа не найден");
                        continue;
                    }

                     List<StudentInGroups> studentsInGroups = studentInGroupsRepository.GetByStudentIdAndGroupsId();
                    var student = new Student();
                    foreach (var studentInGroups in studentsInGroups)
                    {
                        if (studentInGroups.GroupsId == groupsId)
                        {
                            student = studentRepository.GetById(studentInGroups.StudentId);
                            Console.WriteLine($"Id: {student.Id}, Name: {student.Name}");
                        }
                    }
                }
                else if (command == "report")
                {
                    List<Groups> groups = groupsRepository.GetAll();
                    if (groups.Count < 1)
                    {
                        Console.WriteLine("Нет ни одной группы!");
                        continue;
                    }
                    foreach (Groups group in groups)
                    {
                        Console.Write($"Группа: {group.Name} ");
                        List<Groups> Groups = groupsRepository.GetAll();
                        if (groups.Count < 1)
                        {
                            Console.WriteLine("Нет ни одного студента!");
                        }
                        else
                        {
                            Console.WriteLine($"Студентов в группе: {groups.Count}");
                        }

                    }
                }
                else if (command == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Команда не найдена");
                }
            }
        }
    }
    }

    