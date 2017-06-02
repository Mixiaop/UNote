using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNote.Domain.Notes;
using UNote.Domain.Users;

namespace UNote.Services.Notes
{
    public class ContentFollowerService : IContentFollowerService
    {
        private readonly IContentFollowerRepository _followRepository;

        public ContentFollowerService(IContentFollowerRepository followRepository) {
            _followRepository = followRepository;
        }

        public void AddFollower(Content content, User user, string remark = "") {
            if (content != null && user != null) {
                if (!ExistsFollower(content, user))
                {
                    ContentFollower entity = new ContentFollower();
                    entity.ContentId = content.Id;
                    entity.UserId = user.Id;
                    entity.Remark = remark;
                    _followRepository.InsertAndGetId(entity);
                }
            }
        }

        public IList<ContentFollower> GetAllFollowers(int contentId) {
            if (contentId > 0)
            {
                var query = _followRepository.GetAll()
                                             .Where(x => x.ContentId == contentId)
                                             .OrderBy(x => x.CreationTime);

                return query.ToList();
                                             
            }
            else
                return null;
        }

        public IList<ContentFollower> GetAllFollowers(List<int> contentIds)
        {
            if (contentIds != null && contentIds.Count() > 0)
            {
                var query = _followRepository.GetAll()
                                             .Where(x => contentIds.Contains(x.ContentId))
                                             .OrderBy(x => x.CreationTime);

                return query.ToList();
            }
            else {
                return null;
            }
        }

        public IList<ContentFollower> GetLastFollowers(int userId, int count = 0)
        {
            var query = _followRepository.GetAll()
                                            .Where(x => x.UserId == userId)
                                            .OrderByDescending(x => x.Content.CreationTime);

            IList<ContentFollower> list;
            if (count > 0)
            {
                list = query.Take(count).ToList();
            }
            else
                list = query.ToList();

            return query.ToList();
        }

        public int Count(int userId) {
            return _followRepository.Count(x => x.UserId == userId);
        }

        public void RemoveFollower(int followerId) {
            var entity = _followRepository.Get(followerId);

            _followRepository.Delete(entity);
            
        }

        public void RemoveFollower(Content content, User user) {
            if (content != null && user != null) {
                var entity = _followRepository.GetAll().Where(x => x.ContentId == content.Id && x.UserId == user.Id).FirstOrDefault();
                _followRepository.Delete(entity);
            }
        }

        public void DeleteFollowers(int contentId) {
            _followRepository.Delete(x => x.ContentId == contentId);
        }

        public bool ExistsFollower(Content content, User user) {
            if (content != null && user != null)
            {
                return _followRepository.Count(x => x.ContentId == content.Id && x.UserId == user.Id) > 0;
            }
            else
                return false;
        }
    }
}
