using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Service;

public class SubjectService
{
    private readonly SubjectDao _subjectDao;

    public SubjectService(SubjectDao subjectDao)
    {
        _subjectDao = subjectDao;
    }

    public List<Subject> GetAllSubjects()
    {
        return _subjectDao.Select();
    }

    public Subject? GetSubjectById(long id)
    {
        return _subjectDao.SelectById(id);
    }

    public (bool b, string msg) CreateSubject(Subject subject)
    {
        if (_subjectDao.SelectByName(subject.Name) != null)
        {
            return (false, "Creation fails. Subject with the same name already exists");
        }

        if (subject.Password.Length == 0)
        {
            return (false, "Creation fails. Subject password cannot be empty");
        }

        var b = _subjectDao.Insert(subject) > 0;
        return (b, b ? "" : "Insertion into database failed");
    }
}