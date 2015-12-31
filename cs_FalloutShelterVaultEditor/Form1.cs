using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Codeplex.Data;


namespace cs_FalloutShelterVaultEditor
{
    public partial class Form1 : Form
    {
        dynamic _vault = null;
        int _dwellercount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVaultFromResource();

            //CountDwellers();
            //MaxDwellerHPandEND();
            //DisplayEnduraceAndMaxHealth();

            SortVaultRooms();


            CreateNewVaultJSON();

            this.Close();
        }

        private void SortVaultRooms()
        {
            dynamic rooms = _vault.vault.rooms;
            int roomcount = 0;

            foreach (var room in rooms)
            {
                ++roomcount;
            }

            for (int i = 0; i < roomcount - 1; ++i)
            {
                int swap = i;
                for (int j = i + 1; j < roomcount; ++j)
                {
                    if (rooms[j].col < rooms[swap].col)
                    {
                        swap = j;
                    }
                    else if (rooms[j].col == rooms[swap].col && rooms[j].row < rooms[swap].row)
                    {
                        swap = j;
                    }
                }

                if (swap != i)
                {
                    dynamic temp = rooms[swap];
                    rooms[swap] = rooms[i];
                    rooms[i] = temp;
                }
            }
        }

        private void CreateNewVaultJSON()
        {
            using (System.IO.StreamWriter w = new System.IO.StreamWriter("Vault1.json"))
            {
                w.Write(_vault.ToString());
                w.Close();
            }
        }

        private void MaxDwellerHPandEND()
        {
            foreach(var d in _vault.dwellers.dwellers)
            {
                d.health.maxHealth = 105 + (2.5 + 0.5 * 13) * (d.experience.currentLevel - 1);
                d.health.healthValue = d.health.maxHealth;
                d.stats.stats[3].value = 10;
            }
        }

        private void DisplayEnduraceAndMaxHealth()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID,LASTNAME,FIRSTNAME,GENDER,LEVEL,UNKNOWNSTAT,STRENGTH,PERCEPTION,ENDURANCE,CHARISMA,INTELLIGENCE,AGILITY,LUCK,MAXHP\n");

            foreach(var d in _vault.dwellers.dwellers)
            {
                double id = d.serializeId;
                string gender = d.gender == 1 ? "F" : "M";
                double unk = d.stats.stats[0].value + d.stats.stats[0].mod;
                double str = d.stats.stats[1].value + d.stats.stats[1].mod;
                double per = d.stats.stats[2].value + d.stats.stats[2].mod;
                double end = d.stats.stats[3].value + d.stats.stats[3].mod;
                double cha = d.stats.stats[4].value + d.stats.stats[4].mod;
                double inte = d.stats.stats[5].value + d.stats.stats[5].mod;
                double agi = d.stats.stats[6].value + d.stats.stats[6].mod;
                double luck = d.stats.stats[7].value + d.stats.stats[7].mod;
                double maxhealth = d.health.maxHealth;
                double level = d.experience.currentLevel;
                string lastname = d.lastName;
                string firstname = d.name;
                sb.Append(id).Append(',')
                    .Append(lastname).Append(',')
                    .Append(firstname).Append(',')
                    .Append(gender).Append(',')
                    .Append(level).Append(',')
                    .Append(unk).Append(',')
                    .Append(str).Append(',')
                    .Append(per).Append(',')
                    .Append(end).Append(',')
                    .Append(cha).Append(',')
                    .Append(inte).Append(',')
                    .Append(agi).Append(',')
                    .Append(luck).Append(',')
                    .Append(maxhealth).Append('\n');
            }

            using (System.IO.StreamWriter w = new System.IO.StreamWriter("dwellers_hp.csv"))
            {
                w.Write(sb.ToString());
                w.Close();
            }
        }

        private void CountDwellers()
        {
            foreach (var d in _vault.dwellers.dwellers)
            {
                ++_dwellercount;
            }
        }

        private void LoadVaultFromResource()
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in Properties.Resources.Vault1)
            {
                sb.Append(Convert.ToChar(b));
            }

            _vault = DynamicJson.Parse(sb.ToString());
        }
    }
}
