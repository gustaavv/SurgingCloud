using System.Dynamic;
using ConsoleTables;
using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Vo;
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
        var subjects = _subjectService.GetAllSubjects();

        if (opt.JsonFormatOutput)
        {
            Console.WriteLine(JsonUtils.ToStr(subjects, pretty: true));
            return;
        }

        var consoleTable = new ConsoleTable(new ConsoleTableOptions
        {
            Columns = new[] { "id", "name", "password", "hash algorithm" },
            EnableCount = true
        });

        consoleTable.MaxWidth = int.MaxValue;

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
            opt.Cw(OperationResult<object>.Fail("No subject found"));
        }
    }

    public void CreateSubject(SubjectOptions opt)
    {
        opt.Name ??= "";
        opt.Name = opt.Name.Trim();
        if (!(0 < opt.Name.Length && opt.Name.Length < 64))
        {
            opt.Cw(OperationResult<object>.Fail("Creation fails. Name must be between 1 and 64 characters"));
            return;
        }

        opt.Password ??= "";
        if (!(0 < opt.Password.Length && opt.Password.Length < 128))
        {
            opt.Cw(OperationResult<object>.Fail("Creation fails. Password must be between 1 and 128 characters"));
            return;
        }

        var subject = new Subject
        {
            Name = opt.Name,
            Password = opt.Password,
            HashAlg = opt.HashAlg,
            EncMethod = opt.EncMethod
        };
        var result = _subjectService.CreateSubject(subject);
        opt.Cw(result);
    }

    public void DeleteSubject(SubjectOptions opt)
    {
        var result = _subjectService.DeleteSubject(opt.SubjectId);
        opt.Cw(result);
    }


    public void ListAllItems(SubjectOptions opt)
    {
        var items = _subjectService.GetAllItems(opt.SubjectId);

        if (opt.JsonFormatOutput)
        {
            Console.WriteLine(JsonUtils.ToStr(items, pretty: true));
            return;
        }

        var consoleTable = new ConsoleTable(new ConsoleTableOptions
        {
            Columns = new[] { "id", "name before", "name after", "item type", "hash before", "hash after" },
            EnableCount = true
        });

        consoleTable.MaxWidth = int.MaxValue;

        items.ForEach(e =>
        {
            consoleTable.AddRow(e.Id, e.NameBefore, e.NameAfter, e.ItemType, e.HashBefore, e.HashAfter);
        });

        consoleTable.Write();
    }
}