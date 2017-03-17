using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace OpenSpace4xLauncher
{
    public partial class Form1 : Form
    {
        ModLoadConfig modLoadConfig;
        List<ModInfo> modInfos = new List<ModInfo>();
        string modFolderPath = Directory.GetCurrentDirectory() + "/OpenSpace4x_Data/StreamingAssets/Mods/";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMods();
            ModList.ScrollAlwaysVisible = true;
        }

        private void LaunchButton(object sender, EventArgs e)
        {
            string executable = Directory.GetCurrentDirectory() + "/OpenSpace4x.exe";

            //Build new load config
            UpdateModLoadConfig();

            if (File.Exists(executable))
            {
                Process.Start(@executable);
            }
            Application.Exit();
        }

        private void ChangeSelectedMod(object sender, EventArgs e)
        {
            if (ModList.SelectedItem == null || ModList.SelectedIndex < 0)
                return; // No selected item - nothing to do
            string key = ModList.GetItemText(ModList.Items[ModList.SelectedIndex]);

            foreach(ModInfo modInfo in modInfos)
            {
                if(key == modInfo.Name)
                {
                    modVersionBox.Text = modInfo.Version;
                    ModDescriptionBox.Text = modInfo.Description;
                }
            }
        }

        private void MoveModUpButton(object sender, EventArgs e)
        {
            MoveItem(ModList, -1);
        }

        private void MoveModDownButton(object sender, EventArgs e)
        {
            MoveItem(ModList, 1);
        }

        void MoveItem(CheckedListBox list,int direction)
        {
            // Checking selected item
            if (list.SelectedItem == null || list.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = list.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= list.Items.Count)
                return; // Index out of range - nothing to do

            object selected = list.SelectedItem;
            bool checkedBox = list.GetItemChecked(list.SelectedIndex);

            // Removing removable element
            list.Items.Remove(selected);
            // Insert it in new position
            list.Items.Insert(newIndex, selected);
            // Restore selection
            list.SetSelected(newIndex, true);
            //Restored Checkbox
            list.SetItemChecked(newIndex, checkedBox);
        }

        void LoadMods()
        {
            if (!Directory.Exists(modFolderPath))
                return;

            FileStream stream = new FileInfo(modFolderPath + "ModLoadConfig.xml").OpenRead();
            modLoadConfig = (ModLoadConfig)new XmlSerializer(typeof(ModLoadConfig)).Deserialize(stream);
            stream.Close();

            FileInfo[] files = GetFilesByType(modFolderPath, "ModInfo.xml");
            foreach (FileInfo file in files)
            {
                ModInfo modInfo = new ModInfo();
                modInfo = (ModInfo)new XmlSerializer(typeof(ModInfo)).Deserialize((Stream)file.OpenRead());
                modInfos.Add(modInfo);
            }

            //Fill list with mods already in load config and in modinfos
            foreach (ModLoadConfig.ModEntry entry in modLoadConfig.ModLoadOrder)
            {
                foreach(ModInfo modInfo in modInfos)
                {
                    if(modInfo.Name == entry.Name)
                    {
                        ModList.Items.Add(entry.Name, entry.Load);
                    }
                }
            }

            //Fill list with mods not in load config
            foreach(ModInfo modInfo in modInfos)
            {
                if (!modLoadConfig.hasMod(modInfo.Name))
                {
                    ModList.Items.Add(modInfo.Name, false);
                }
            }

        }

        void UpdateModLoadConfig()
        {
            ModLoadConfig newModLoadConfig = new ModLoadConfig();
            XmlSerializer serializer = new XmlSerializer(typeof(ModLoadConfig));
            FileStream stream = new FileStream(modFolderPath + "ModLoadConfig.xml", FileMode.Create);

            for (int i = 0; i < ModList.Items.Count; i++)
            {
                newModLoadConfig.ModLoadOrder.Add(new ModLoadConfig.ModEntry(ModList.GetItemText(ModList.Items[i]), ModList.GetItemChecked(i)));
            }

            serializer.Serialize(stream, newModLoadConfig);
            stream.Close();
        }

        FileInfo[] GetFilesByType(string path, string extension)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files;
            try
            {
                files = dirInfo.GetFiles(extension, SearchOption.AllDirectories);
            }
            catch
            {
                files = new FileInfo[0];
            }
            return files;
        }
    }
}
