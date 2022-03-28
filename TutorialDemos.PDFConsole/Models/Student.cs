namespace TutorialDemos.PDFConsole.Models;

public class Student
{
    public string? FirstName { get; }
    public string? OtherNames { get; }
    public int Age { get; }
    public DateTime DoB { get; }

    public Student(string? firstName, string? otherNames, int age, DateTime doB)
    {
        FirstName = firstName;
        OtherNames = otherNames;
        Age = age;
        DoB = doB;
    }
    public Student()
    {

    }

    public static List<Student> GetStudents()
    {
        return new List<Student>()
        {
            new Student(firstName: "John", otherNames: "Tuza", age: 10, doB: DateTime.UtcNow.AddYears(-10).Date),
            new Student(firstName: "Mbabazi", otherNames: "List", age: 20, doB: DateTime.UtcNow.AddYears(-15).Date),
            new Student(firstName: "Joel", otherNames: "March", age: 10, doB: DateTime.UtcNow.AddYears(-20).Date)
        };
    }
}
