using System.Dynamic;
using ConsoleTables;
using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Service;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli.Controller;

public class SubjectController
{
    private readonly SubjectService _subjectService;

    private readonly EncryptService _encryptService;

    public SubjectController(SubjectService subjectService, EncryptService encryptService)
    {
        _subjectService = subjectService;
        _encryptService = encryptService;
    }

    public void ListAllSubjects(SubjectOptions opt)
    {
        var consoleTable = new ConsoleTable(new ConsoleTableOptions
        {
            Columns = new[] { "id", "name", "password", "hash algorithm" },
            EnableCount = true
        });

        var subjects = _subjectService.GetAllSubjects();

        if (opt.JsonFormatOutput)
        {
            Console.WriteLine(JsonUtils.ToStr(subjects, pretty: true));
            return;
        }

        subjects.ForEach(s => { consoleTable.AddRow(s.Id, s.Name, s.Password, s.HashAlg); });

        consoleTable.Write();
    }

    public async Task GetSubject(SubjectOptions opt)
    {
        var id = opt.SubjectId;
        var subject = _subjectService.GetSubjectById(id);
        if (subject != null)
        {
            dynamic newObj = JsonUtils.ToObj<ExpandoObject>(JsonUtils.ToStr(subject))!;
            newObj.ActualPassword = await _encryptService.CreateArchivePassword(subject);
            var json = JsonUtils.ToStr(newObj, pretty: true);
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