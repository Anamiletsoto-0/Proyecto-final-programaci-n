using System;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmPrincipal : Form
    {
        private Usuario usuarioActual;

        public frmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;
            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            // Configurar formulario
            this.Text = "Sistema de Control de Autobuses";
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header
            Panel header = new Panel();
            header.BackColor = Color.FromArgb(44, 62, 80);
            header.Dock = DockStyle.Top;
            header.Height = 60;
            this.Controls.Add(header);

            Label lblTitulo = new Label();
            lblTitulo.Text = "🚌 Sistema de Control de Autobuses";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 15);
            header.Controls.Add(lblTitulo);

            Label lblUsuario = new Label();
            lblUsuario.Text = $"{usuarioActual.NombreUsuario} ({usuarioActual.TipoUsuario}) | Cerrar Sesión";
            lblUsuario.Font = new Font("Segoe UI", 10);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(header.Width - lblUsuario.Width - 20, 20);
            lblUsuario.Click += LblCerrarSesion_Click;
            header.Controls.Add(lblUsuario);

            // Status bar
            Panel statusBar = new Panel();
            statusBar.BackColor = Color.White;
            statusBar.Dock = DockStyle.Top;
            statusBar.Height = 40;
            statusBar.Top = header.Height;
            this.Controls.Add(statusBar);

            Label lblBienvenido = new Label();
            lblBienvenido.Text = $"Bienvenido, {usuarioActual.NombreUsuario}";
            lblBienvenido.Font = new Font("Segoe UI", 10);
            lblBienvenido.AutoSize = true;
            lblBienvenido.Location = new Point(20, 10);
            statusBar.Controls.Add(lblBienvenido);

            // Menu panel
            Panel menuPanel = new Panel();
            menuPanel.BackColor = Color.FromArgb(52, 73, 94);
            menuPanel.Dock = DockStyle.Left;
            menuPanel.Width = 200;
            this.Controls.Add(menuPanel);

            // Menu items
            string[] menuItems = { "Choferes", "Autobuses", "Rutas", "Asignaciones", "Reportes" };
            int topPosition = 20;

            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button();
                btnMenuItem.Text = item;
                btnMenuItem.BackColor = Color.Transparent;
                btnMenuItem.ForeColor = Color.White;
                btnMenuItem.FlatStyle = FlatStyle.Flat;
                btnMenuItem.FlatAppearance.BorderSize = 0;
                btnMenuItem.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 62, 80);
                btnMenuItem.TextAlign = ContentAlignment.MiddleLeft;
                btnMenuItem.Size = new Size(200, 40);
                btnMenuItem.Location = new Point(0, topPosition);
                btnMenuItem.Font = new Font("Segoe UI", 10);
                btnMenuItem.Click += MenuItem_Click;

                // Configurar permisos según tipo de usuario
                if (usuarioActual.TipoUsuario == "Usuario" &&
                   (item == "Choferes" || item == "Autobuses" || item == "Rutas"))
                {
                    btnMenuItem.Enabled = false;
                }

                menuPanel.Controls.Add(btnMenuItem);
                topPosition += 45;
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string formName = clickedButton.Text;

            Form form = null;

            switch (formName)
            {
                case "Choferes":
                    form = new frmChoferes();
                    break;
                case "Autobuses":
                    form = new frmAutobuses();
                    break;
                case "Rutas":
                    form = new frmRutas();
                    break;
                case "Asignaciones":
                    form = new frmAsignaciones();
                    break;
                case "Reportes":
                    MessageBox.Show("Módulo de reportes en desarrollo");
                    return;
            }

            if (form != null)
            {
                form.MdiParent = this;
                form.Show();
            }
        }

        private void LblCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro de cerrar sesión?",
                "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.Show();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmPrincipal
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmPrincipal";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
