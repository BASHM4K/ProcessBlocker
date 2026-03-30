using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TapokBlock
{
    public partial class MainForm : Form
    {
        private TextBox txtProcessName;
        private Button btnBlock;
        private Button btnUnblock;
        private Button btnBio;
        private Label lblStatus;

        public MainForm()
        {
            InitializeComponent();
            LoadBlockedProcesses();
        }

        private void InitializeComponent()
        {
            this.Text = "TapokBlock";
            this.Size = new Size(680, 370);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 10);

            // --- Title ---
            Label lblTitle = new Label
            {
                Text = "TapokBlock",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            // --- Description ---
            Label lblDesc = new Label
            {
                Text = "Яндекс ведёт крайне агрессивную и навязчивую рекламную политику своего браузера, " +
                       "поэтому в своих сборках я изначально боролся с этим, предварительно удаляя браузер " +
                       "во время первоначальной настройки...\r\nТеперь пришло время создать программу, которая " +
                       "запретит использование Яндекс Браузера для всех людей в этом мире!",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                AutoSize = false,
                Size = new Size(630, 90),
                Location = new Point(20, 50),
                TextAlign = ContentAlignment.TopLeft
            };

            // --- Process input label ---
            Label lblInputHint = new Label
            {
                Text = "Имя процесса (.exe):",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                AutoSize = true,
                Location = new Point(20, 148)
            };

            // --- Process input ---
            txtProcessName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(390, 26),
                Location = new Point(20, 168),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // --- Block button (big, left) ---
            btnBlock = new Button
            {
                Text = "Заблокировать",
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 50),
                Location = new Point(20, 210),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBlock.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            btnBlock.FlatAppearance.BorderSize = 2;
            btnBlock.Click += BtnBlock_Click;

            // --- Unblock button ---
            btnUnblock = new Button
            {
                Text = "Разблокировать",
                Font = new Font("Segoe UI", 12),
                Size = new Size(200, 50),
                Location = new Point(230, 210),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUnblock.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            btnUnblock.FlatAppearance.BorderSize = 2;
            btnUnblock.Click += BtnUnblock_Click;

            // --- Bio button ---
            btnBio = new Button
            {
                Text = "Мое Bio",
                Font = new Font("Segoe UI", 10),
                Size = new Size(200, 50),
                Location = new Point(440, 210),
                BackColor = Color.FromArgb(230, 230, 230),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBio.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnBio.Click += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://fakecrime.bio/Bashmachok") { UseShellExecute = true });

            // --- Status label ---
            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(0, 120, 0),
                AutoSize = true,
                Location = new Point(20, 275)
            };

            // --- Footer separator ---
            Panel separator = new Panel
            {
                Size = new Size(660, 1),
                Location = new Point(0, 300),
                BackColor = Color.FromArgb(200, 200, 200)
            };

            // --- Footer left ---
            Label lblFooterLeft = new Label
            {
                Text = "~/home/BASHM4K@ - 2026",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = true,
                Location = new Point(10, 310)
            };

            LinkLabel lnkSupport = new LinkLabel
            {
                Text = "Поддержать",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(145, 310)
            };
            lnkSupport.LinkClicked += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://adderly.top/donate") { UseShellExecute = true });

            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblDesc,
                lblInputHint, txtProcessName,
                btnBlock, btnUnblock, btnBio,
                lblStatus,
                separator,
                lblFooterLeft, lnkSupport
            });
        }

        private void BtnBlock_Click(object sender, EventArgs e)
        {
            string processName = txtProcessName.Text.Trim();
            if (string.IsNullOrWhiteSpace(processName))
            {
                MessageBox.Show("Введите имя процесса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (ProcessBlocker.BlockProcess(processName))
                {
                    lblStatus.Text = $"Процесс '{processName}' заблокирован.";
                    lblStatus.ForeColor = Color.FromArgb(0, 120, 0);
                    txtProcessName.Clear();
                }
                else
                {
                    lblStatus.Text = $"Не удалось заблокировать '{processName}'.";
                    lblStatus.ForeColor = Color.FromArgb(180, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUnblock_Click(object sender, EventArgs e)
        {
            string processName = txtProcessName.Text.Trim();
            if (string.IsNullOrWhiteSpace(processName))
            {
                MessageBox.Show("Введите имя процесса для разблокировки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (ProcessBlocker.UnblockProcess(processName))
                {
                    lblStatus.Text = $"Процесс '{processName}' разблокирован.";
                    lblStatus.ForeColor = Color.FromArgb(0, 120, 0);
                    txtProcessName.Clear();
                }
                else
                {
                    lblStatus.Text = $"Не удалось разблокировать '{processName}'.";
                    lblStatus.ForeColor = Color.FromArgb(180, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBlockedProcesses()
        {
            // Reserved for future list display
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ProcessBlocker.IsRunningAsAdministrator())
            {
                MessageBox.Show("Для работы программы требуются права администратора.\n\nПожалуйста, перезапустите от имени Администратора.",
                    "Требуются права администратора", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
    }
}