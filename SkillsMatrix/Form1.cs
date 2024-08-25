using System.Diagnostics;
using System.Text;

namespace SkillsMatrix
{
    public partial class Form1 : Form
    {
        private string fileName = string.Empty;
        SkillDoc document = new SkillDoc();
        FileManager fileManager = new FileManager();
        BrowserHelper browserHelper = new BrowserHelper();
        List<string> levelDescriptions = new List<string>();

        public Form1()
        {
            InitializeComponent();
            fileManager.Filter = "Skills Files (*.skills)|*.skills|All files (*.*)|*.*";

            // https://icombine.net/knowledge-base/skill-levels
            levelDescriptions.Add(@"");
            levelDescriptions.Add(@"- Has minimal or textbook knowledge without connecting it to the practice
- Needs close supervision or guidance
- Has little or no idea of ​​how to deal with complexity
- Tends to look at actions in isolation");
            levelDescriptions.Add(@"- Has basic knowledge of key aspects of the practice
- Straightforward tasks are likely to be done to an acceptable standard
- Is able to achieve some steps using his own judgment, but needs supervision for the overall task
- Appreciates complex situations, but is only able to achieve partial resolution
- Sees actions as a series of steps");
            levelDescriptions.Add(@"- Has good working and background knowledge of the area of practice
- Results can be achieved for open tasks, though may lack refinement
- Is able to achieve most tasks using own judgment
- Copes with complex situations through deliberate analysis and planning
- Sees actions at least partly in terms of longer-term goals");
            levelDescriptions.Add(@"- Depth of understanding of discipline and area of practice
- Fully acceptable standards are achieved routinely, and results are also achieved for open tasks
- Able to take full responsibility for own work (and that of others where applicable)
- Deals with complex situations holistically, confident decision-making
- Sees the overall picture and how individual actions fit within it");
            levelDescriptions.Add(@"- Authoritative knowledge of the discipline and deep tacit understanding across areas of practice
- Excellence achieved with relative ease
- Able to take responsibility for going beyond existing standards and creating own interpretations
- Holistic grasp of complex situations. Moves between intuitive and analytical approaches with ease
- Sees overall picture and alternative approaches, has a vision of what may be possible");
        }

        private void ShowDocument()
        {
            listBox1.Items.Clear();
            foreach (string str in document.Skills)
            {
                listBox1.Items.Add(str);
            }

            listBox2.Items.Clear();
            foreach (string str in document.PersonList)
            {
                listBox2.Items.Add(str);
            }

            listBox3.SelectedIndex = -1;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await browserHelper.Attach(webView21);

            browserHelper.ShowHtml($"<html><body><h1>Hello {Environment.UserName}</h1></body></html>");

            string[] args = Environment.GetCommandLineArgs();

            if ((args.Length > 1) && (!string.IsNullOrEmpty(args[1])))
            {
                fileName = args[1];
                document = fileManager.Open<SkillDoc>(fileName) ?? new();
                ShowDocument();
            }
            else
            {
                document = new();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.interfacing.com/what-is-rasci-raci";
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://expertprogrammanagement.com/2018/04/rapid-decision-making-model/";
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.linkedin.com/pulse/rapid-raci-matrices-same-heres-how-use-them-francisco-sagastume-he3if/";
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Insert:
                    string newTask = InputBox.ShowDialog("Name", "Add Task");
                    if (!string.IsNullOrEmpty(newTask))
                    {
                        int pos = listBox1.Items.Add(newTask);
                        document.Skills.Add(newTask);
                        listBox1.SelectedIndex = pos;
                    }
                    break;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSkillLevel();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSkillLevel();
        }


        bool programaticSetting = false;

        private void UpdateItem()
        {
            string task = GetSkill();
            string party = GetPerson();

            if ((string.IsNullOrEmpty(task)) || (string.IsNullOrEmpty(party)))
            {
                return;
            }
            SkillItem item = document.Items.Where(s => ((s.Skill == task) && (s.Person == party)))
                .FirstOrDefault();
            if (item != null)
            {
                item.SkillLevel = GetSkillLevel();
                item.InterestedInDeveloping = checkBox1.Checked;
            }
            else
            {
                document.Items.Add(new SkillItem()
                {
                    Skill = task,
                    Person = party,
                    SkillLevel = GetSkillLevel(),
                    InterestedInDeveloping = checkBox1.Checked
                });
            }
        }

        private string GetSkill()
        {
            string rtnVal = string.Empty;

            if (listBox1.SelectedIndex != -1)
            {
                rtnVal = listBox1.Items[listBox1.SelectedIndex].ToString();
            }

            return rtnVal;
        }

        private string GetPerson()
        {
            string rtnVal = string.Empty;

            if (listBox2.SelectedIndex != -1)
            {
                rtnVal = listBox2.Items[listBox2.SelectedIndex].ToString();
            }

            return rtnVal;
        }

        private string GetSkillLevel()
        {
            string rtnVal = string.Empty;

            if (listBox3.SelectedIndex != -1)
            {
                rtnVal = listBox3.Items[listBox3.SelectedIndex].ToString();
            }

            return rtnVal;
        }

        private void ShowSkillLevel()
        {
            string task = GetSkill();
            string party = GetPerson();

            bool enabled = ( //(comboBox2.SelectedIndex != -1) &&
                (!string.IsNullOrEmpty(task)) &&
                (!string.IsNullOrEmpty(party)));

            programaticSetting = true;

            listBox3.Enabled = enabled;

            listBox3.SelectedIndex = -1;
            checkBox1.Checked = false;

            SkillItem item = document.Items.Where(s => ((s.Skill == task) && (s.Person == party)))
                .FirstOrDefault();
            if (item != null)
            {
                int buttonNumber = 0;

                int founcAt = listBox3.FindStringExact(item.SkillLevel);
                listBox3.SelectedIndex = founcAt;

                checkBox1.Checked = item.InterestedInDeveloping;
            }

            ShowLevelDescription();

            programaticSetting = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newTask = InputBox.ShowDialog("Name   :", "Add Skill");
            if (!string.IsNullOrEmpty(newTask))
            {
                int pos = listBox1.Items.Add(newTask);
                document.Skills.Add(newTask);
                listBox1.SelectedIndex = pos;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string newRolePerson = InputBox.ShowDialog("Name    :", "Add Person");
            if (!string.IsNullOrEmpty(newRolePerson))
            {
                int pos = listBox2.Items.Add(newRolePerson);
                document.PersonList.Add(newRolePerson);
                listBox2.SelectedIndex = pos;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            string sel = listBox1.Items[listBox1.SelectedIndex].ToString();
            document.Skills.Remove(sel);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;

            string sel = listBox2.Items[listBox2.SelectedIndex].ToString();
            document.PersonList.Remove(sel);
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            document = new SkillDoc();
            ShowDocument();
            ShowSkillLevel();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkillDoc opened = fileManager.Open<SkillDoc>();
            if (document != null)
            {
                document = opened;
                ShowDocument();
                ShowSkillLevel();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileManager.Save(document);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileManager.SaveAs(document);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AddHtmlTop();
            sb.StartTable();
            sb.AddTableHeader(BumpColumns(document.PersonList));
            foreach (string task in document.Skills)
            {
                sb.AddHeaderedTableRow(BuildSkillRow(task));
            }
            sb.EndTable();
            sb.AddHtmlBottom();

            browserHelper.ShowHtml(sb.ToString());
        }

        // this will leave the first column empty (useful for the header row)
        private List<string> BumpColumns(List<string> columns)
        {
            List<string> result = new List<string>();
            result.Add(string.Empty);
            result.InsertRange(1, columns);
            return result;
        }

        private List<string> BuildSkillRow(string skillName)
        {
            List<string> result = new List<string>();
            result.Add(skillName);

            foreach (string person in document.PersonList)
            {
                StringBuilder skillLevelString = new();
                SkillItem item = document.Items.Where(s => ((s.Skill == skillName) && (s.Person == person)))
                    .FirstOrDefault();
                if (item != null)
                {
                    skillLevelString.Append(item.SkillLevel);
                    if (item.InterestedInDeveloping)
                    {
                        skillLevelString.Append(" +");
                    }
                }
                result.Add(skillLevelString.ToString());
            }

            return result;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (programaticSetting)
                return;

            ShowLevelDescription();
            UpdateItem();
        }

        private void ShowLevelDescription()
        {
            if (listBox3.SelectedIndex == -1)
            {
                textBox1.Text = "";
                return;
            }
            textBox1.Text = levelDescriptions[listBox3.SelectedIndex];
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (programaticSetting)
                return;
            UpdateItem();
        }
    }
}
