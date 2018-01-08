using UNote.Domain.Notes;

namespace UNote.Services.Events
{
    public class TagUpdatedEvent
    {
        public TagUpdatedEvent(Node node, string originalTagName, string newTagName) {
            this.Node = node;
            this.OriginalTagName = originalTagName;
            this.NewTagName = newTagName;
        }

        /// <summary>
        /// 所有分类 
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// 原标签名称
        /// </summary>
        public string OriginalTagName { get; set; }

        /// <summary>
        /// 新标签名称
        /// </summary>
        public string NewTagName { get; set; }
    }
}
