using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace DesktopAplikacija
{
    public partial class MainForm : Form
    {
        List<Korisnik> listaKorisnika = new List<Korisnik>();
        List<Korisnik> korisnici = new List<Korisnik>();

        public MainForm()
        {
            InitializeComponent();
            UcitajPodatke();
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.ColumnCount = 4; 
                dataGridView1.Columns[0].Name = "ID";
                dataGridView1.Columns[1].Name = "Ime";
                dataGridView1.Columns[2].Name = "Prezime";
                dataGridView1.Columns[3].Name = "Kontakt";
            }
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            int id = korisnici.Count + 1;
            string ime = txtIme.Text;
            string prezime = txtPrezime.Text;
            string kontakt = txtKontakt.Text;

            Korisnik noviKorisnik = new Korisnik(id, ime, prezime, kontakt);
            korisnici.Add(noviKorisnik);
            SpremiPodatke();
            PrikaziPodatke();
        }

        private void SpremiPodatke()
        {
            string filePath = "korisnici.txt"; 
            List<string> linije = new List<string>();

            foreach (Korisnik k in listaKorisnika)
            {
                linije.Add($"{k.ID},{k.Ime},{k.Prezime},{k.Kontakt}");
            }

            File.WriteAllLines(filePath, linije); 
        }

        private void UcitajPodatke()
        {
            if (File.Exists("korisnici.txt"))
            {
                var linije = File.ReadAllLines("korisnici.txt");
                List<Korisnik> korisnici = linije.Select(Korisnik.FromString).ToList();

                dataGridView1.Rows.Clear(); 
                foreach (var k in korisnici)
                {
                    dataGridView1.Rows.Add(k.ID, k.Ime, k.Prezime, k.Kontakt);
                }

            }
            else
            {
                MessageBox.Show("Datoteka ne postoji, učitavanje nije moguće!");
            }
        }


        private void PrikaziPodatke()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = korisnici;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            int id = listaKorisnika.Count + 1; 
            string ime = txtIme.Text;
            string prezime = txtPrezime.Text;
            string kontakt = txtKontakt.Text;

            Korisnik noviKorisnik = new Korisnik(id, ime, prezime, kontakt);
            listaKorisnika.Add(noviKorisnik);

            SpremiPodatke(); 

            MessageBox.Show("Podaci su uspješno spremljeni!", "Obavijest", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void btnUcitaj_Click(object sender, EventArgs e)
        {
            UcitajPodatke();
        }
    }
}
