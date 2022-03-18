use university

create table Student(
	Id int identity(1,1) constraint PK_Student primary key,
	Name nvarchar(100),
	Age int
)

select * from Student

create table Groups(
	Id int identity(1,1) constraint PK_Groups primary key,
	Name nvarchar(100)
)

select * from Student
select * from Groups

create table StudentInGroups(
	StudentId int constraint FK_Post_Student references Student(Id),
	GroupsId int constraint FK_Post_Groups references Groups(Id)
)

select * from StudentInGroups