using AutoMapper;
using UNote.Domain.Notes;
using UNote.Domain.Users;
using UNote.Domain.Teams;
using UNote.Services.Notes.Dto;
using UNote.Services.Users.Dto;
using UNote.Services.Teams.Dto;

namespace UNote.Services.Mapping
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings()
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal();

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal()
        {
            Mapper.CreateMap<Node, NodeDto>().ReverseMap();
            Mapper.CreateMap<Content, ContentDto>().ReverseMap();
            Mapper.CreateMap<Content, BoardTaskDto>().ReverseMap();
            Mapper.CreateMap<Content, BoardTaskBriefDto>().ReverseMap();
            Mapper.CreateMap<ContentColumn, BoardColumnDto>().ReverseMap();
            Mapper.CreateMap<ContentColumn, BoardColumnBriefDto>().ReverseMap();
            Mapper.CreateMap<Tag, TagDto>().ReverseMap();

            Mapper.CreateMap<User, UserDto>().ReverseMap();
            Mapper.CreateMap<Team, TeamDto>().ReverseMap();

        }
    }
}
