using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smithWaterman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "smithWaterman";
            //Default olarak alınan gap, match, mismatch değerleri
            this.textBox1.Text = "1";
            this.textBox2.Text = "-1";
            this.textBox3.Text = "-2";
            
        }
        string seq1, seq2;
        string seq1_length, seq2_length;

        StringBuilder Seq1Alignment = new StringBuilder();
        StringBuilder Seq2Alignment = new StringBuilder();

        List<string> SeqList1 = new List<string>();
        List<string> SeqList2 = new List<string>();
        List<int> scoreList = new List<int>();

        //hesaplama butonu
        private void button1_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start(); ;

            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                dataGridView.Refresh();

                textBox5.Text = String.Empty;
                textBox6.Text = String.Empty;

                StreamReader sr = new StreamReader(@"C:\Users\serka\OneDrive\Masaüstü\seq1.txt");
                seq1_length = sr.ReadLine();
                seq1 = sr.ReadToEnd();

                StreamReader sr2 = new StreamReader(@"C:\Users\serka\OneDrive\Masaüstü\seq2.txt");
                seq2_length = sr2.ReadLine();
                seq2 = sr2.ReadToEnd();

                string s1 = seq1.ToUpper();
                string s2 = seq2.ToUpper();

                int match = Convert.ToInt32(textBox1.Text); //match değeri
                int missmatch = Convert.ToInt32(textBox2.Text); //mismatch değeri
                int gap = Convert.ToInt32(textBox3.Text); //gap değeri

                char[] gecici = new char[s1.Length];
                char[] gecici2 = new char[s2.Length];

                gecici = s1.ToCharArray();
                gecici2 = s2.ToCharArray();

                dataGridView.ColumnCount = gecici.Length + 1;
                dataGridView.RowCount = gecici2.Length + 1;
                dataGridView.Columns[0].Name = "*";
                dataGridView.Rows[0].HeaderCell.Value = "*";

                for (int i = 0; i < gecici.Length; i++)
                {
                    dataGridView.Columns[i + 1].Name = gecici[i].ToString();
                }
                for (int j = 0; j < gecici2.Length; j++)
                {
                    dataGridView.Rows[j + 1].HeaderCell.Value = gecici2[j].ToString();
                }

                int a;
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView[i, 0].Value = 0;
                }
                for (int j = 0; j < dataGridView.Rows.Count; j++)
                {
                    dataGridView[0, j].Value = 0;
                }

                for (int i = 1; i < dataGridView.Columns.Count; i++)
                {
                    for (int j = 1; j < dataGridView.Rows.Count; j++)
                    {
                        if (dataGridView.Columns[i].Name.ToString() == dataGridView.Rows[j].HeaderCell.Value.ToString())
                        {
                            a = Math.Max(int.Parse(dataGridView[i - 1, j].Value.ToString()) + gap,
                                Math.Max(int.Parse(dataGridView[i, j - 1].Value.ToString()) + gap,
                                int.Parse(dataGridView[i - 1, j - 1].Value.ToString()) + match)); //match

                            if (a > 0)
                            {
                                dataGridView[i, j].Value = a;
                            }
                            else
                            {
                                dataGridView[i, j].Value = 0;
                            }
                        }
                        else
                        {
                            a = Math.Max(int.Parse(dataGridView[i - 1, j].Value.ToString()) + gap,
                                Math.Max(int.Parse(dataGridView[i, j - 1].Value.ToString()) + gap,
                                int.Parse(dataGridView[i - 1, j - 1].Value.ToString()) + missmatch)); //mismatch

                            if (a > 0)
                            {
                                dataGridView[i, j].Value = a;
                            }
                            else
                            {
                                dataGridView[i, j].Value = 0;
                            }

                        }
                    }
                }
                int max = int.Parse(dataGridView[0, 0].Value.ToString());

                s1 = "-" + s1;
                s2 = "-" + s2;
                int score = 0;

                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Rows.Count; j++)
                    {
                        if (int.Parse(dataGridView[i, j].Value.ToString()) > max)
                        {
                            max = int.Parse(dataGridView[i, j].Value.ToString());
                        }
                    }
                }

                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Rows.Count; j++)
                    {
                        if (int.Parse(dataGridView[i, j].Value.ToString()) == max)
                        {
                            int x = i;
                            int y = j;
                            while (true)
                            {
                                int temp_score = 0;

                                while (int.Parse(dataGridView[x, y].Value.ToString()) != 0)
                                {
                                    if (dataGridView.Columns[x].Name.ToString() == dataGridView.Rows[y].HeaderCell.Value.ToString())
                                    {
                                        Seq1Alignment.Insert(0, s1[x]);
                                        Seq2Alignment.Insert(0, s2[y]);
                                        dataGridView[x, y].Style.BackColor = Color.Aqua;
                                        temp_score = temp_score + match;
                                        x--;
                                        y--;
                                    }
                                    else
                                    {
                                        if (int.Parse(dataGridView[x, y].Value.ToString()) == (int.Parse(dataGridView[x, y - 1].Value.ToString()) + gap))
                                        {
                                            Seq1Alignment.Insert(0, "-");
                                            Seq2Alignment.Insert(0, s2[y]);
                                            dataGridView[x, y].Style.BackColor = Color.Aqua;
                                            temp_score = temp_score + gap;
                                            y--;
                                        }
                                        else if (int.Parse(dataGridView[x, y].Value.ToString()) == (int.Parse(dataGridView[x - 1, y - 1].Value.ToString()) + missmatch))
                                        {
                                            Seq1Alignment.Insert(0, s1[x]);
                                            Seq2Alignment.Insert(0, s2[y]);
                                            dataGridView[x, y].Style.BackColor = Color.Aqua;
                                            temp_score = temp_score + missmatch;
                                            x--;
                                            y--;
                                        }
                                        else if (int.Parse(dataGridView[x, y].Value.ToString()) == (int.Parse(dataGridView[x - 1, y].Value.ToString()) + gap))
                                        {
                                            Seq1Alignment.Insert(0, s1[x]);
                                            Seq2Alignment.Insert(0, "-");
                                            dataGridView[x, y].Style.BackColor = Color.Aqua;
                                            temp_score = temp_score + gap;
                                            x--;
                                        }
                                    }
                                }
                                scoreList.Add(temp_score);
                                SeqList1.Add(Seq1Alignment.ToString());
                                SeqList2.Add(Seq2Alignment.ToString());

                                Seq1Alignment.Length = 0;
                                Seq2Alignment.Length = 0;

                                if (temp_score >= score)
                                {
                                    score = temp_score;
                                }
                                else
                                {
                                    continue;
                                }
                                if (int.Parse(dataGridView[x, y].Value.ToString()) == 0)
                                {
                                    dataGridView[x, y].Style.BackColor = Color.Aqua;
                                    break;
                                }
                            }
                        }
                        else continue;
                    }
                }

                int count = 0;
                for (int i = 0; i < scoreList.Count; i++)
                {
                    if (scoreList[i] == score)
                    {
                        count++;
                        if (count > 1)
                        {
                            textBox5.Text += SeqList1[i].ToString() + "      ";
                            textBox6.Text += SeqList2[i].ToString() + "      ";
                        }
                        else
                        {
                            textBox5.Text += SeqList1[i].ToString() + "      ";
                            textBox6.Text += SeqList2[i].ToString() + "      ";
                        }
                    }
                }
                textBox7.Text = score.ToString();
            }
            watch.Stop();
            textBox4.Text = ($"{watch.ElapsedMilliseconds} ms");
        }

        
      


    }
}
