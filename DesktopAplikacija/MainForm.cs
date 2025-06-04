using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DesktopAplikacija
{
    public partial class MainForm : Form
    {
        private List<Korisnik> lista = new List<Korisnik>();
        private readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                      AttachDbFilename=C:\Users\38597\source\repos\DesktopAplikacija\DesktopAplikacija\KorisniciBaza.mdf;
                                      Integrated Security=True;Connect Timeout=30";

        public MainForm()
        {
            InitializeComponent();
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            string ime = txtIme.Text.Trim();
            string prezime = txtPrezime.Text.Trim();
            string kontakt = txtKontakt.Text.Trim();
            if (string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(prezime) || string.IsNullOrEmpty(kontakt))
            {
                MessageBox.Show("Sva polja moraju biti ispunjena!", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "INSERT INTO Korisnici (Ime, Prezime, Kontakt) VALUES (@Ime, @Prezime, @Kontakt)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ime", ime);
                    cmd.Parameters.AddWithValue("@Prezime", prezime);
                    cmd.Parameters.AddWithValue("@Kontakt", kontakt);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Korisnik je spremljen u bazu.", "Uspjeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtKontakt.Text = "";
        }

        private void btnUcitaj_Click(object sender, EventArgs e)
        {
            lista.Clear();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                string sql = "SELECT ID, Ime, Prezime, Kontakt FROM Korisnici";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string ime = reader.GetString(1);
                        string prezime = reader.GetString(2);
                        string kontakt = reader.GetString(3);
                        Korisnik k = new Korisnik(id, ime, prezime, kontakt);
                        lista.Add(k);
                    }
                }
            }
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
            if (dataGridView1.Columns["Kontakt"] != null)
                dataGridView1.Columns["Kontakt"].Visible = false;
            if (dataGridView1.Columns["Prezime"] != null)
                dataGridView1.Columns["Prezime"].Visible = false;
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            Korisnik odabrani = dataGridView1.Rows[e.RowIndex].DataBoundItem as Korisnik;
            if (odabrani != null)
            {
                string naslov = $"Podatci za {odabrani.Ime}";
                string poruka = $"ID: {odabrani.ID}\r\nIme: {odabrani.Ime}\r\nPrezime: {odabrani.Prezime}\r\nKontakt: {odabrani.Kontakt}";
                MessageBox.Show(poruka, naslov, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
