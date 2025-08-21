
using System;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmLogin : Form
    {
        private Panel panel; // Declarar panel como variable de clase

        public frmLogin()
        {
            InitializeComponent();
            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Sistema de Control de Autobuses - Login";
            this.Size = new Size(500, 500); // Aumentado para el botón de registro
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Panel contenedor
            panel = new Panel();
            panel.BackColor = Color.White;
            panel.Size = new Size(450, 400);
            panel.Location = new Point((this.ClientSize.Width - panel.Width) / 2, 50);
            this.Controls.Add(panel);

            // Logo/Title
            Label lblTitulo = new Label();
            lblTitulo.Text = "🚌 Control de Autobuses";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point((panel.Width - lblTitulo.Width) / 5, 20);
            panel.Controls.Add(lblTitulo);

            // Usuario
            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario:";
            lblUsuario.Font = new Font("Segoe UI", 10);
            lblUsuario.ForeColor = Color.FromArgb(44, 62, 80);
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(25, 70);
            panel.Controls.Add(lblUsuario);

            TextBox txtUsuario = new TextBox();
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Font = new Font("Segoe UI", 10);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Size = new Size(300, 30);
            txtUsuario.Location = new Point(25, 95);
            panel.Controls.Add(txtUsuario);

            // Password
            Label lblPassword = new Label();
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Segoe UI", 10);
            lblPassword.ForeColor = Color.FromArgb(44, 62, 80);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(25, 130);
            panel.Controls.Add(lblPassword);

            TextBox txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(25, 155);
            txtPassword.UseSystemPasswordChar = true;
            panel.Controls.Add(txtPassword);

            // Botón Login
            Button btnLogin = new Button();
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.BackColor = Color.FromArgb(44, 62, 80);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(300, 35);
            btnLogin.Location = new Point(25, 200);
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.Click += BtnLogin_Click;
            panel.Controls.Add(btnLogin);

            // Botón Registro (NUEVO)
            Button btnRegistrar = new Button();
            btnRegistrar.Text = "Registrarse";
            btnRegistrar.BackColor = Color.FromArgb(52, 152, 219);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Size = new Size(300, 35);
            btnRegistrar.Location = new Point(25, 245);
            btnRegistrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegistrar.Click += BtnRegistrar_Click;
            panel.Controls.Add(btnRegistrar);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = this.Controls.Find("txtUsuario", true)[0].Text;
                string contrasena = this.Controls.Find("txtPassword", true)[0].Text;

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UsuarioBL usuarioBL = new UsuarioBL();
                Usuario usuarioAutenticado = usuarioBL.Autenticar(usuario, contrasena);

                if (usuarioAutenticado != null)
                {
                    this.Hide();
                    frmPrincipal principal = new frmPrincipal(usuarioAutenticado);
                    principal.Show();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al autenticar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRegistro registro = new frmRegistro();
            registro.ShowDialog();
            this.Show();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmLogin
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.ResumeLayout(false);

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}