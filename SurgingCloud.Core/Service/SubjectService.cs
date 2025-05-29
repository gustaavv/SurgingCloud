using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Service;

public class SubjectService
{
    private readonly SubjectDao _subjectDao;

    private readonly ItemDao _itemDao;

    public SubjectService(SubjectDao subjectDao, ItemDao itemDao)
    {
        _subjectDao = subjectDao;
        _itemDao = itemDao;
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

    public (bool b, string msg) DeleteSubject(long id)
    {
        using (var conn = _subjectDao.CreateConnection())
        {
            conn.Open();
            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    var subject = _subjectDao.SelectById(id);

                    if (subject == null)
                    {
                        tx.Commit();
                        return (false, $"Subject {id} does not exist");
                    }

                    _subjectDao.Delete(id, tx);
                    _itemDao.DeleteBySubjectId(id, tx);

                    tx.Commit();
                    return (true, $"Delete subject {id} succeeds");
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return (false, $"Delete subject {id} fails, {ex.Message}");
                }
            }
        }
    }
}