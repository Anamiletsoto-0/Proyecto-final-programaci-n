using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmChoferes : Form
    {
        private ChoferBL choferBL;
        private int? choferEditandoId = null;

        public frmChoferes()
        {
            InitializeComponent();
            choferBL = new ChoferBL();
            ConfigurarInterfaz();
            CargarChoferes();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Choferes";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241);

            Panel panel = new Panel();
            panel.BackColor = Color.White;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "👨‍💼 Gestión de Choferes";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panel.Controls.Add(lblTitulo);

            // Campos del formulario
            string[] labels = { "Nombre:", "Apellido:", "Cédula:", "Fecha de Nacimiento:" };
            int topPosition = 60;

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 10);
                lbl.ForeColor = Color.FromArgb(44, 62, 80);
                lbl.AutoSize = true;
                lbl.Location = new Point(20, topPosition);
                panel.Controls.Add(lbl);

                Control inputControl;
                if (i == 3) // Fecha de nacimiento
                {
                    DateTimePicker dtp = new DateTimePicker();
                    dtp.Font = new Font("Segoe UI", 10);
                    dtp.Format = DateTimePickerFormat.Short;
                    dtp.Size = new Size(300, 30);
                    dtp.Location = new Point(20, topPosition + 25);
                    dtp.Name = "dtpFechaNacimiento";
                    dtp.MaxDate = DateTime.Now.AddYears(-21); // Mínimo 21 años
                    inputControl = dtp;
                }
                else
                {
                    TextBox txt = new TextBox();
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Size = new Size(300, 30);
                    txt.Location = new Point(20, topPosition + 25);

                    switch (i)
                    {
                        case 0: txt.Name = "txtNombre"; break;
                        case 1: txt.Name = "txtApellido"; break;
                        case 2: txt.Name = "txtCedula"; break;
                    }

                    inputControl = txt;
                }

                panel.Controls.Add(inputControl);
                topPosition += 70;
            }

            // Botones
            int buttonTop = topPosition + 20;
            Button btnGuardar = CrearBoton("Guardar", Color.FromArgb(44, 62, 80), 20, buttonTop);
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.Name = "btnGuardar";
            panel.Controls.Add(btnGuardar);

            Button btnLimpiar = CrearBoton("Limpiar", Color.FromArgb(149, 165, 166), 120, buttonTop);
            btnLimpiar.Click += BtnLimpiar_Click;
            panel.Controls.Add(btnLimpiar);

            Button btnCancelar = CrearBoton("Cancelar", Color.FromArgb(231, 76, 60), 220, buttonTop);
            btnCancelar.Click += BtnCancelar_Click;
            btnCancelar.Visible = false;
            btnCancelar.Name = "btnCancelar";
            panel.Controls.Add(btnCancelar);

            // DataGridView
            buttonTop += 60;
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvChoferes";
            dgv.Size = new Size(panel.Width - 60, 250);
            dgv.Location = new Point(20, buttonTop);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += DgvChoferes_CellClick;

            // Columnas
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("Apellido", "Apellido");
            dgv.Columns.Add("Cedula", "Cédula");
            dgv.Columns.Add("FechaNacimiento", "Fecha Nac.");
            dgv.Columns.Add("Acciones", "Acciones");

            dgv.Columns["Id"].Visible = false;

            panel.Controls.Add(dgv);
        }

        private Button CrearBoton(string texto, Color color, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(90, 35);
            btn.Location = new Point(x, y);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            return btn;
        }

        private void CargarChoferes()
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls.Find("dgvChoferes", true)[0];
                dgv.Rows.Clear();

                List<Chofer> choferes = choferBL.ObtenerChoferes();

                foreach (var chofer in choferes)
                {
                    dgv.Rows.Add(
                        chofer.Id,
                        chofer.Nombre,
                        chofer.Apellido,
                        chofer.Cedula,
                        chofer.FechaNacimiento.ToString("dd/MM/yyyy"),
                        "Editar | Eliminar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar choferes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = this.Controls.Find("txtNombre", true)[0].Text;
                string apellido = this.Controls.Find("txtApellido", true)[0].Text;
                string cedula = this.Controls.Find("txtCedula", true)[0].Text;
                DateTime fechaNacimiento = ((DateTimePicker)this.Controls.Find("dtpFechaNacimiento", true)[0]).Value;

                Chofer chofer = new Chofer
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Cedula = cedula,
                    FechaNacimiento = fechaNacimiento
                };

                bool resultado;
                Button btnGuardar = (Button)sender;

                if (btnGuardar.Text == "Guardar")
                {
                    resultado = choferBL.CrearChofer(chofer);
                    if (resultado) MessageBox.Show("Chofer registrado correctamente");
                }
                else
                {
                    chofer.Id = choferEditandoId.Value;
                    resultado = choferBL.ActualizarChofer(chofer);
                    if (resultado) MessageBox.Show("Chofer actualizado correctamente");
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    CargarChoferes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            this.Controls.Find("txtNombre", true)[0].Text = "";
            this.Controls.Find("txtApellido", true)[0].Text = "";
            this.Controls.Find("txtCedula", true)[0].Text = "";
            ((DateTimePicker)this.Controls.Find("dtpFechaNacimiento", true)[0]).Value = DateTime.Now.AddYears(-21);

            Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
            btnGuardar.Text = "Guardar";

            Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
            btnCancelar.Visible = false;

            choferEditandoId = null;
        }

        private void DgvChoferes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string accion = dgv.Rows[e.RowIndex].Cells["Acciones"].Value.ToString();

                if (accion.Contains("Editar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EditarChofer(id);
                }
                else if (accion.Contains("Eliminar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EliminarChofer(id);
                }
            }
        }

        private void EditarChofer(int id)
        {
            try
            {
                Chofer chofer = choferBL.ObtenerChoferPorId(id);

                if (chofer != null)
                {
                    this.Controls.Find("txtNombre", true)[0].Text = chofer.Nombre;
                    this.Controls.Find("txtApellido", true)[0].Text = chofer.Apellido;
                    this.Controls.Find("txtCedula", true)[0].Text = chofer.Cedula;
                    ((DateTimePicker)this.Controls.Find("dtpFechaNacimiento", true)[0]).Value = chofer.FechaNacimiento;

                    Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
                    btnGuardar.Text = "Actualizar";

                    Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
                    btnCancelar.Visible = true;

                    choferEditandoId = id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar chofer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarChofer(int id)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de eliminar este chofer?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool eliminado = choferBL.EliminarChofer(id);

                    if (eliminado)
                    {
                        MessageBox.Show("Chofer eliminado correctamente");
                        CargarChoferes();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar chofer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmChoferes
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmChoferes";
            this.Load += new System.EventHandler(this.frmChoferes_Load);
            this.ResumeLayout(false);

        }

        private void frmChoferes_Load(object sender, EventArgs e)
        {

        }
    }
}
