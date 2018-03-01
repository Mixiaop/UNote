using System;
using System.Linq;
using System.Collections.Generic;
using UNote.Domain.Users;
using UNote.Domain.Notes;

namespace UNote.Services.Users
{
    public class UserVisitLogService : IUserVisitLogService
    {
        private readonly IUserVisitLogRepository _userVisitLogRepository;
        public UserVisitLogService(IUserVisitLogRepository userVisitLogRepository)
        {
            _userVisitLogRepository = userVisitLogRepository;
        }

        public void Visit(User user, int typeId, Node node = null, Content content = null)
        {
            UserVisitLog log = new UserVisitLog();
            if (user != null) {
                log.UserId = user.Id;
                if (typeId == 1)
                {
                    #region Node
                    log.TypeId = 1;
                    if (node != null) {
                        if (_userVisitLogRepository.Count(x => x.UserId == user.Id && x.NodeId == node.Id) > 0)
                        {
                            log = _userVisitLogRepository.GetAll().Where(x => x.UserId == user.Id && x.NodeId == node.Id).FirstOrDefault();
                        }
                        else {
                            log.NodeId = node.Id;
                        }

                    }
                    #endregion
                }
                else
                {
                    #region Content
                    log.TypeId = 2;
                    if (content != null) {
                        if (_userVisitLogRepository.Count(x => x.UserId == user.Id && x.ContentId == content.Id) > 0)
                        {
                            log = _userVisitLogRepository.GetAll().Where(x => x.UserId == user.Id && x.ContentId == content.Id).FirstOrDefault();
                        }
                        else
                        {
                            log.ContentId = content.Id;
                        }
                    }
                    #endregion
                }
            }

            log.LastVisitTime = DateTime.Now;
            if (log.Id > 0)
            {
                _userVisitLogRepository.Update(log);
            }
            else
                _userVisitLogRepository.Insert(log);
        }

        public IList<UserVisitLog> QueryLastVisitLogs(User user, int count = 0) {
            var query = _userVisitLogRepository.GetAll()
                                               .Where(x => x.UserId == user.Id)
                                               .OrderByDescending(x => x.LastVisitTime);
            IList<UserVisitLog> list;
            if (count > 0)
            {
                list = query.Take(count).ToList();
            }
            else
                list = query.ToList();

            return list.ToList();
        }
    }
}
