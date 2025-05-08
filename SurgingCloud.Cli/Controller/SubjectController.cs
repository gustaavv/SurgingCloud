using ConsoleTables;
using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Service;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli.Controller;

public class SubjectController
{
    private readonly SubjectService _subjectService;

    public SubjectController(SubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public void ListAllSubjects()
    {
        var consoleTable = new ConsoleTable(new ConsoleTableOptions
        {
            Columns = new[] { "id", "name", "password", "hash algorithm" },
            EnableCount = true
        });

        var subjects = _subjectService.GetAllSubjects();
        subjects.ForEach(s => { consoleTable.AddRow(s.Id, s.Name, s.Password, s.HashAlg); });

        consoleTable.Write();
    }

    public void GetSubject(SubjectOptions opt)
    {
        var id = opt.SubjectId;
        var subject = _subjectService.GetSubjectById(id);
        if (subject != null)
        {
            var json = JsonUtils.ToStr(subject, pretty: true);
            Console.WriteLine(json);
        }
        else
        {
            Console.WriteLine("No subject found");
        }
    }

    public void CreateSubject(SubjectOptions opt)
    {
        opt.Name ??= "";
        opt.Name = opt.Name.Trim();
        if (!(0 < opt.Name.Length && opt.Name.Length < 64))
        {
            Console.WriteLine("Creation fails. Name must be between 1 and 64 characters");
            return;
        }

        opt.Password ??= "";
        if (!(0 < opt.Password.Length && opt.Password.Length < 128))
        {
            Console.WriteLine("Creation fails. Password must be between 1 and 128 characters");
            return;
        }

        var subject = new Subject { Name = opt.Name, Password = opt.Password, HashAlg = opt.HashAlg };
        var (b, msg) = _subjectService.CreateSubject(subject);
        Console.WriteLine(b ? "Creation succeeds" : msg);
    }
}