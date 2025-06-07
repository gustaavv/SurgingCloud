using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Vo;

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

    public List<Item> GetAllItems(long subjectId)
    {
        return _itemDao.SelectBySubjectId(subjectId);
    }

    public Subject? GetSubjectById(long id)
    {
        return _subjectDao.SelectById(id);
    }

    public OperationResult<long> CreateSubject(Subject subject)
    {
        if (_subjectDao.SelectByName(subject.Name) != null)
        {
            return OperationResult<long>.Fail("Creation fails. Subject with the same name already exists");
        }

        if (subject.Password.Length == 0)
        {
            return OperationResult<long>.Fail("Creation fails. Subject password cannot be empty");
        }

        var b = _subjectDao.Insert(subject) > 0;
        if (!b)
        {
            return OperationResult<long>.Fail("Insertion into database failed");
        }

        subject = _subjectDao.SelectByName(subject.Name)!;
        return OperationResult<long>.Ok($"Creation succeeds, new subject id = {subject.Id}", subject.Id);
    }

    public OperationResult<object> DeleteSubject(long id)
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
                        return OperationResult<object>.Fail($"Subject {id} does not exist");
                    }

                    _subjectDao.Delete(id, tx);
                    _itemDao.DeleteBySubjectId(id, tx);

                    tx.Commit();
                    return OperationResult<object>.Ok($"Delete subject {id} succeeds");
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return OperationResult<object>.Fail($"Delete subject {id} fails, {ex.Message}");
                }
            }
        }
    }
}