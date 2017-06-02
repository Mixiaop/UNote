
namespace UNote.Web.AjaxServices.Dto
{
    public class InsertOrUpdateNodeInput
    {
        public int Id { get; set; }

        public string TeamKey { get; set; }

        public int ParentId { get; set; }

        

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Des { get; set; }

        public int Order { get; set; }

        public bool Public { get; set; }

        public int NodeTypeId { get; set; }

        public string CustomUrl { get; set; }
    }
}