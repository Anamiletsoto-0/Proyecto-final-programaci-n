using System;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmRegistro : Form
    {
        public frmRegistro()
        {
            InitializeComponent();
            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Registro de Usuario";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(236, 240, 241);

            Panel panel = new Panel();
            panel.BackColor = Color.White;
            panel.Size = new Size(450, 450);
            panel.Location = new Point((this.ClientSize.Width - panel.Width) / 2, 20);
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "📝 Registro de Usuario";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point((panel.Width - lblTitulo.Width) / 2, 20);
            panel.Controls.Add(lblTitulo);

            // Campos de registro
            string[] labels = { "Usuario:", "Contraseña:", "Confirmar Contraseña:", "Tipo de Usuario:" };
            int topPosition = 70;

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 10);
                lbl.ForeColor = Color.FromArgb(44, 62, 80);
                lbl.AutoSize = true;
                lbl.Location = new Point(25, topPosition);
                panel.Controls.Add(lbl);

                Control inputControl;
                if (i == 3) // Tipo de usuario (ComboBox)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.Font = new Font("Segoe UI", 10);
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmb.Size = new Size(300, 30);
                    cmb.Location = new Point(25, topPosition + 25);
                    cmb.Name = "cmbTipoUsuario";
                    cmb.Items.AddRange(new object[] { "Usuario", "Administrador" });
                    cmb.SelectedIndex = 0;
                    inputControl = cmb;
                }
                else if (i == 1 || i == 2) // Campos de contraseña
                {
                    TextBox txt = new TextBox();
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Size = new Size(300, 30);
                    txt.Location = new Point(25, topPosition + 25);
                    txt.UseSystemPasswordChar = true;

                    if (i == 1) txt.Name = "txtPassword";
                    else txt.Name = "txtConfirmPassword";

                    inputControl = txt;
                }
                else // Usuario
                {
                    TextBox txt = new TextBox();
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Size = new Size(300, 30);
                    txt.Location = new Point(25, topPosition + 25);
                    txt.Name = "txtUsuario";
                    inputControl = txt;
                }

                panel.Controls.Add(inputControl);
                topPosition += 70;
            }

            // Botones
            Button btnRegistrar = new Button();
            btnRegistrar.Text = "Registrar";
            btnRegistrar.BackColor = Color.FromArgb(39, 174, 96);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.Size = new Size(300, 35);
            btnRegistrar.Location = new Point(25, topPosition + 10);
            btnRegistrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegistrar.Click += BtnRegistrar_Click;
            panel.Controls.Add(btnRegistrar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.BackColor = Color.FromArgb(231, 76, 60);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Size = new Size(300, 35);
            btnCancelar.Location = new Point(25, topPosition + 55);
            btnCancelar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCancelar.Click += BtnCancelar_Click;
            panel.Controls.Add(btnCancelar);
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = this.Controls.Find("txtUsuario", true)[0].Text;
                string password = this.Controls.Find("txtPassword", true)[0].Text;
                string confirmPassword = this.Controls.Find("txtConfirmPassword", true)[0].Text;
                string tipoUsuario = ((ComboBox)this.Controls.Find("cmbTipoUsuario", true)[0]).SelectedItem.ToString();

                // Validaciones
                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Todos los campos son obligatorios", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Las contraseñas no coinciden", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password.Length < 4)
                {
                    MessageBox.Show("La contraseña debe tener al menos 4 caracteres", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear usuario
                UsuarioBL usuarioBL = new UsuarioBL();
                bool resultado = usuarioBL.CrearUsuario(new Usuario
                {
                    NombreUsuario = usuario,
                    Contrasena = password,
                    TipoUsuario = tipoUsuario
                });

                if (resultado)
                {
                    MessageBox.Show("Usuario registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmRegistro
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmRegistro";
            this.Load += new System.EventHandler(this.frmRegistro_Load);
            this.ResumeLayout(false);

        }

        private void frmRegistro_Load(object sender, EventArgs e)
        {

        }
    }
}