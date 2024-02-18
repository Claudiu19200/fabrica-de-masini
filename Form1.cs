using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TemaBD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Client values (@Nume, @Prenume , @CNP)", con);
            cmd.Parameters.AddWithValue("@Nume", textBox1.Text);
            cmd.Parameters.AddWithValue("@Prenume", textBox3.Text);
            cmd.Parameters.AddWithValue("@CNP", textBox2.Text);
            cmd.ExecuteNonQuery();

            con.Close();
            MessageBox.Show("Succesfully Saved!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("update Client set Nume = @Nume, Prenume = @Prenume where CNP = @CNP ", con);
            cmd.Parameters.AddWithValue("@Nume", textBox1.Text);
            cmd.Parameters.AddWithValue("@Prenume", textBox3.Text);
            cmd.Parameters.AddWithValue("@CNP", textBox2.Text);
            cmd.ExecuteNonQuery();

            con.Close();
            MessageBox.Show("Succesfully updated!");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete Client where CNP = @CNP", con);
            cmd.Parameters.AddWithValue("@CNP", textBox2.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Succesfully deleted!");


        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Client", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;


        }

        // Metode pentru tabelul Fabrica
        private void button5_Click(object sender, EventArgs e) // buton pentru inserarea în Fabrica
        {
            ExecuteSqlCommand("insert into Fabrica (NumeFabrica, Adresa, CodPostal) values (@NumeFabrica, @Adresa, @CodPostal)",
                              new SqlParameter("@NumeFabrica", textBox4.Text),
                              new SqlParameter("@Adresa", textBox5.Text),
                              new SqlParameter("@CodPostal", textBox6.Text));
            MessageBox.Show("Succesfully Saved in Fabrica!");
        }

        private void button6_Click(object sender, EventArgs e) // buton pentru actualizarea în Fabrica
        {
            ExecuteSqlCommand("update Fabrica set Adresa = @Adresa, CodPostal = @CodPostal where NumeFabrica = @NumeFabrica",
                              new SqlParameter("@NumeFabrica", textBox4.Text),
                              new SqlParameter("@Adresa", textBox5.Text),
                              new SqlParameter("@CodPostal", textBox6.Text));
            MessageBox.Show("Succesfully Updated in Fabrica!");
        }

        private void button7_Click(object sender, EventArgs e) // buton pentru ștergerea din Fabrica
        {
            ExecuteSqlCommand("delete from Fabrica where NumeFabrica = @NumeFabrica",
                              new SqlParameter("@NumeFabrica", textBox4.Text));
            MessageBox.Show("Succesfully Deleted from Fabrica!");
        }

        private void ExecuteSqlCommand(string query, params SqlParameter[] parameters)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Fabrica", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT F.NumeFabrica, M.Model, M.Caroserie, M.Serie, M.AnFabricatie
                FROM Masini M
                INNER JOIN Fabrica F ON M.FabricaID = F.FabricaID";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



        }

        private void button11_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    int nrMasini;
                    if (!int.TryParse(textBox7.Text, out nrMasini))
                    {
                        MessageBox.Show("Vă rugăm introduceți un număr valid.");
                        return;
                    }

                    string query = @"
                SELECT C.Nume, C.Prenume, C.CNP
                FROM Client C
                INNER JOIN MasiniClient MC ON C.ClientID = MC.ClientID
                GROUP BY C.Nume, C.Prenume, C.CNP
                HAVING COUNT(MC.MasiniID) >= @NrMasini";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@NrMasini", nrMasini);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT C.Nume, C.Prenume, Co.DataComanda, F.NumeFurnizor, F.Adresa
                FROM Comenzi Co
                INNER JOIN MasiniMaterial MM ON Co.MaterialID = MM.MaterialID
                INNER JOIN Client C ON Co.CNP = C.CNP
                INNER JOIN Furnizor F ON Co.FurnizorID = F.FurnizorID";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT A.Nume, A.Prenume, A.Functie, F.NumeFabrica, F.Adresa, F.CodPostal
                FROM Angajat A
                INNER JOIN Fabrica F ON A.FabricaID = F.FabricaID";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT DISTINCT F.NumeFurnizor, F.Adresa, F.Contact
                FROM Furnizor F
                WHERE F.FurnizorID IN (
                    SELECT C.FurnizorID
                    FROM Comenzi C
                    WHERE C.ValoareTotala > 10000
                )";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string codPostal = textBox8.Text;
                    string query = @"
                SELECT Model
                FROM Masini
                WHERE FabricaID IN (SELECT FabricaID FROM Fabrica WHERE CodPostal = @CodPostal)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CodPostal", codPostal);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT TipMaterial
                FROM Material
                WHERE Pret > (SELECT AVG(Pret) FROM Material)";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT C.Nume, C.Prenume
                  FROM Client C
                  WHERE C.CNP IN (
                  SELECT MC.ClientID
                  FROM MasiniClient MC
                  JOIN Masini M ON MC.MasiniID = M.MasiniID
                  GROUP BY MC.ClientID
                  HAVING COUNT(DISTINCT M.Model) >= 3)";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-GUTTERSB\\SQLSERVER;Initial Catalog=MasiniFabrica; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT A.Nume, A.Prenume
                FROM Angajat A
                WHERE A.FabricaID IN (
                    SELECT MM.FabricaID
                    FROM MasiniMaterial MM
                    JOIN Material M ON MM.MaterialID = M.MaterialID
                    GROUP BY MM.FabricaID
                    HAVING SUM(M.StocDisponibil) > 1000
                )";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
