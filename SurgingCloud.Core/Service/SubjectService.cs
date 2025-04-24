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
    
    public (bool, string) CreateSubject(Subject subject)
    {
        if (_subjectDao.SelectByName(subject.Name) != null)
        {
            return (false, "Subject with the same name already exists");
        }

        if (subject.Password.Length == 0)
        {
            return (false, "Subject password cannot be empty");
        }

        return (_subjectDao.Insert(subject) > 0, "");
    }
}