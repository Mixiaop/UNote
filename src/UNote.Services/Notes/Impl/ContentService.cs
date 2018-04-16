using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using LinqKit;
using U;
using U.UI;
using U.Application.Services.Dto;
using UNote.Domain.Teams;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Services.Teams;
using UNote.Services.Notification;

namespace UNote.Services.Notes
{
    public class ContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ITagService _tagService;
        private readonly INodeService _nodeService;
        private readonly ITeamService _teamService;
        private readonly IContentFollowerService _followerService;
        private readonly INotificationService _notificationService;
        public ContentService(IContentRepository contentRepository, ITagService tagService, INodeService nodeService, ITeamService teamService, IContentFollowerService followerService, INotificationService notificationService)
        {
            _contentRepository = contentRepository;
            _tagService = tagService;
            _nodeService = nodeService;
            _teamService = teamService;
            _followerService = followerService;
            _notificationService = notificationService;
        }

        #region Contents
        #region Query
        /// <summary>
        /// 查询所有的笔记内容
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="keywords"></param>
        /// <param name="nodeIds"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public PagedResultDto<Content> Query(int pageIndex, int pageSize = 20, int userId = -1, string keywords = "", IList<int> nodeIds = null, IList<int> teamIds = null, string tag = "")
        {
            var query = _contentRepository.GetAll()
                                          .Where(x => x.ParentId == 0);
            if (userId > 0)
                query = query.Where(x => x.UserId == userId);

            if (keywords.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Title.Contains(keywords) || x.Tag.Contains(keywords));
            }

            if (nodeIds != null && nodeIds.Count > 0)
            {
                query = query.Where(x => nodeIds.Contains(x.NodeId));
            }

            if (tag.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Tag.Contains(tag));
            }

            //if (teamIds != null && teamIds.Count > 0)
            //{
            //      var teamPredicate = PredicateBuilder.True<Content>();
            //      teamPredicate.Or(x => teamIds.Contains(x.TeamId));
            //      query = query.AsExpandable().Where(teamPredicate);
            //}

            var list = query.OrderByDescending(x => x.IsTop)
                            .ThenByDescending(x => x.CreationTime)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize).ToList();
            var count = query.Count();
            if (list != null)
            {
                LoadFollowers(list); //载入关注的人
                LoadItems(list); //载入内容项
                foreach (var content in list)
                    LoadPreviewFromBody(content);
            }
            return new PagedResultDto<Content>(count, (List<Content>)list);
        }

        public PagedResultDto<Content> QueryPublicContents(int pageIndex = 1, int pageSize = 20, string keywords = "")
        {
            var query = _contentRepository.GetAll().Where(x => x.ParentId == 0);

            if (keywords.IsNotNullOrEmpty())
            {
                query = query.Where(x => x.Title.Contains(keywords) || x.Tag.Contains(keywords));
            }

            query = query.Where(x => x.Public == true);

            var list = query.OrderByDescending(x => x.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var count = query.Count();
            return new PagedResultDto<Content>(count, (List<Content>)list);
        }

        public IList<Content> QueryRecentNotes(User user, int count = 10)
        {
            var teams = _teamService.GetAllTeams(user.Id, false);
            var query = _contentRepository.GetAll().Where(x => x.ParentId == 0);

            if (teams != null && teams.Count > 0)
            {
                List<int> teamIds = new List<int>();
                foreach (var team in teams)
                    teamIds.Add(team.Id);

                query = query.Where(x => x.UserId == user.Id || teamIds.Contains(x.TeamId));
            }
            else
            {
                query = query.Where(x => x.UserId == user.Id);
            }

            var list = query.OrderByDescending(x => x.CreationTime).Take(count);
            return list.ToList();
        }

        public PagedResultDto<Content> QueryRecentNotes(User user, int pageIndex = 1, int pageSize = 20)
        {
            var teams = _teamService.GetAllTeams(user.Id, false);
            var query = _contentRepository.GetAll().Where(x => x.ParentId == 0);

            if (teams != null && teams.Count > 0)
            {
                List<int> teamIds = new List<int>();
                foreach (var team in teams)
                    teamIds.Add(team.Id);

                query = query.Where(x => x.UserId == user.Id || teamIds.Contains(x.TeamId));
            }
            else
            {
                query = query.Where(x => x.UserId == user.Id);
            }
            var count = query.Count();
            var list = query.OrderByDescending(x => x.CreationTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResultDto<Content>(count, list);
        }

        #endregion
        public int Count(int userId)
        {
            var count = _contentRepository.Count(x => x.UserId == userId);
            return count;
        }

        /// <summary>
        /// 获取一条笔记内容
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public Content GetById(int contentId)
        {
            var content = _contentRepository.Get(contentId);
            if (content == null)
                throw new UserFriendlyException("Content is null");
            return content;
        }

        public int Insert(Content content)
        {
            if (content.Title.IsNullOrEmpty())
            {
                throw new UserFriendlyException("内容标题不能为空");
            }

            if (content.FileId > 0 && content.FileSize <= 0)
            {
                throw new UserFriendlyException("上传文件的情况下，文件大小不能为空");
            }

            if (content.Tag.IsNotNullOrEmpty())
            {
                content.Tag = content.Tag.Replace("，", ",");
                //更新标签
                _tagService.CreateOrUpdateTags(content.NodeId, content.Tag);
            }

            if (!content.Body.IsNullOrEmpty())
            {
                var body = content.Body.RemoveHtml();

                if (body.Length > 255)
                {
                    content.Summary = body.SubString2(255);
                }
                else
                    content.Summary = body;
            }

            content.Id = _contentRepository.InsertAndGetId(content);
            if (content.ParentId == 0)
            {
                if (content.Id > 0)
                {
                    //更新数量
                    var node = _nodeService.GetById(content.NodeId);
                    if (node != null)
                    {
                        node.ContentTotal++;
                        _nodeService.Update(node);
                    }
                }

                //关注的人
                List<User> followers = new List<User>();
                if (content.Followers != null && content.Followers.Count > 0)
                {
                    foreach (var user in content.Followers)
                    {
                        _followerService.AddFollower(content, user.User);
                        followers.Add(user.User);
                    }
                }

                if (followers.Count > 0)
                {
                    //_notificationService.AddContentAfterSending(content, followers);
                }
            }
            return content.Id;
        }

        public void Update(Content content, int oldNodeId = 0)
        {
            if (oldNodeId > 0 && oldNodeId != content.NodeId)
            {
                var oldNode = _nodeService.GetById(oldNodeId);
                var newNode = _nodeService.GetById(content.NodeId);
                oldNode.ContentTotal--;
                newNode.ContentTotal++;
                _nodeService.Update(oldNode);
                _nodeService.Update(newNode);
            }

            if (!content.Body.IsNullOrEmpty())
            {
                var body = content.Body.RemoveHtml();

                if (body.Length > 255)
                {
                    content.Summary = body.SubString2(255);
                }
                else
                    content.Summary = body;
            }

            if (!content.BodyPersons.IsNullOrEmpty())
            {
                var bodyPersons = content.BodyPersons.RemoveHtml();

                if (bodyPersons.Length > 255)
                {
                    content.Summary = bodyPersons.SubString2(255);
                }
                else
                    content.Summary = bodyPersons;
            }

            //if (content.Tag.IsNotNullOrEmpty())
            //{
            //    content.Tag = content.Tag.Replace("，", ",");
            //    //更新标签
            //    _tagService.UpdateTags(content.NodeId, content.Tag);
            //}

            _contentRepository.Update(content);

            //关注的人
            List<User> followers = new List<User>();
            if (content.Followers != null && content.Followers.Count > 0)
            {
                foreach (var user in content.Followers)
                {
                    _followerService.AddFollower(content, user.User);
                    followers.Add(user.User);
                }
            }

            if (followers.Count > 0)
            {
                //_notificationService.UpdateContentAfterSending(content, followers);
            }
        }

        public void Delete(int contentId)
        {
            var content = _contentRepository.Get(contentId);
            if (content == null)
                throw new UserFriendlyException("content is null");

            Delete(content);
        }

        /// <summary>
        /// 删除一条笔记
        /// </summary>
        /// <param name="content"></param>
        public void Delete(Content content)
        {
            if (content.Node != null)
            {
                content.Node.ContentTotal--;
                _nodeService.Update(content.Node);
            }
            _contentRepository.Delete(content);
            var contentId = content.Id;
            _followerService.DeleteFollowers(contentId);
        }

        public void AddVisitCount(int contentId, Content info = null, int userId = 0)
        {
            if (info != null)
            {
                info.VisitCount++;
                _contentRepository.Update(info);
            }
            else
            {
                info = GetById(contentId);
                info.VisitCount++;
                _contentRepository.Update(info);
            }
        }

        #region Utilities

        /// <summary>
        /// 从内容里装载预览图
        /// </summary>
        /// <param name="content"></param>
        private void LoadPreviewFromBody(Content content)
        {
            if (content.PreviewUrl.IsNullOrEmpty())
            {
                Regex regPreview = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
                //设置预览图
                Match match = regPreview.Match(content.Body);
                if (match != null)
                    content.FormatPreviewUrl = match.Groups["imgUrl"].ToString();
            }
            else
            {
                content.FormatPreviewUrl = content.PreviewUrl;
            }
        }

        /// <summary>
        /// 为笔记批量载入关注的人
        /// </summary>
        /// <param name="contents"></param>
        private void LoadFollowers(IList<Content> contents)
        {
            if (contents != null)
            {
                List<int> ids = new List<int>();
                contents.ForEach((x) =>
                {
                    ids.Add(x.Id);
                });
                var followers = _followerService.GetAllFollowers(ids);

                if (followers != null && followers.Count > 0)
                {
                    foreach (var content in contents)
                    {
                        content.Followers = followers.Where(x => content.Id == x.ContentId).ToList();
                    }
                }
            }
        }
        #endregion
        #endregion

        #region ContentItems
        public void InsertItem(Content content, Content item)
        {

            if (content != null)
            {
                item.ParentId = content.Id;
                Insert(item);

                content.ContentItemCount += 1;
                Update(content);
            }
        }

        public IList<Content> GetAllItems(int contentId)
        {
            var query = _contentRepository.GetAll()
                                          .Where(x => x.ParentId == contentId)
                                          .OrderByDescending(x => x.CreationTime);
            return query.ToList();
        }

        private void LoadItems(List<Content> contents)
        {
            if (contents != null) {
                List<int> ids = new List<int>();
                foreach (var content in contents) {
                    ids.Add(content.Id);

                }

                var query = _contentRepository.GetAll()
                                          .Where(x => ids.Contains(x.ParentId))
                                          .OrderByDescending(x => x.CreationTime);
                var items = query.ToList();
                foreach (var content in contents) {
                    content.ContentItems = items.Where(x => x.ParentId == content.Id).ToList();
                }
            }
        }
        #endregion
    }
}
