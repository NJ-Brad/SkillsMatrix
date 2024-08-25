namespace SkillsMatrix
{
    public class SkillDoc
    {
        public List<string> Skills { get; set; } = new List<string>();
        public List<string> PersonList { get; set; } = new List<string>();
        public List<SkillItem> Items { get; set; } = new List<SkillItem>();
    }

    public class SkillItem
    {
        public string Skill { get; set; }
        public string Person { get; set; }
        public string SkillLevel { get; set; }
        public bool InterestedInDeveloping { get; set; } = false;
    }
}
//         [YamlIgnore]
