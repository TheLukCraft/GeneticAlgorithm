using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Algorytm_Evolucyjny
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Okienko
        public MainWindow()
        {
            InitializeComponent();
        }

        private Population population;
        private Environment environment;
        private List<SubjectInfo> SubjectInfoList = new List<SubjectInfo>();
        private UInt32 populacja = 0;
        //Stwórz populację (ilość)
        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                UInt32 size = Convert.ToUInt32(populationSize.Text);

                population = new Population(size);
                if (environment != null) population.Environment = environment;
                populacja = 1;
                ShowPopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
        //wypisuje populacje w okienku i podsumowanie
        private void ShowPopulation()
        {
            if (environment != null) topLbl.Content = environment;

            if (population != null)
            {
                SubjectInfoList.Clear();

                double sum = population.Sum();

                for (UInt32 i = 0; i < population.Subjects.Length; i++)
                {
                    if (environment != null)
                    {
                        double val = population.ValOfSubject(i);
                        SubjectInfoList.Add(new SubjectInfo() { Id = i, Dna = population.Subjects[i].ToString(), Val = population.Subjects[i].Value, YVal = val, Part = val / sum });

                    }
                    else
                        SubjectInfoList.Add(new SubjectInfo() { Id = i, Dna = population.Subjects[i].ToString(), Val = population.Subjects[i].Value, YVal = 0, Part = 0 });
                }
                //prawe okienko
                listView.ItemsSource = SubjectInfoList;
                listView.Items.Refresh();

                if(environment != null)
                {
                    var best = SubjectInfoList.First(x => x.YVal == SubjectInfoList.Max(y => y.YVal));
                    var worst = SubjectInfoList.First(x => x.YVal == SubjectInfoList.Min(y => y.YVal));

                    topLbl.Content += "\t max: " + best.Id + ", min: " + worst.Id;
                }

                topLbl.Content += "\tPopulacja: " + populacja;

            }
            else
                topLbl.Content += "\t Stwórz populację!";
        }
        //ustawia dane środowiska 
        private void Set_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                environment = new Environment(Convert.ToDouble(a.Text), Convert.ToDouble(b.Text), Convert.ToDouble(c.Text));

                if (population != null) population.Environment = environment;

                ShowPopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //mutacja
        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (population != null)
                {
                    population = population.Mutate(Convert.ToDouble(subjectChance.Text), Convert.ToDouble(genChance.Text));

                    ShowPopulation();
                }
                else
                {
                    throw new Exception("Najpierw stwórz lub wczytaj populację");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //krzyżowanie
        private void Cross_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (population != null)
                {

                    population = population.Cross(Convert.ToDouble(crossChance.Text));
                    ShowPopulation();
                }
                else
                {
                    throw new Exception("Najpierw stwórz lub wczytaj populację");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //ruletka
        private Subject Ruletka()
        {
            if (population != null && environment != null)
            {
                Subject best = population.Roulette();

                return best;
            }
            else if (population == null)
            {
                throw new Exception("Najpierw stwórz lub wczytaj populację!");
            }
            else
            {
                throw new Exception("Najpier dodaj warunki!");
            }
        }
        //przycisk ruletki
        private void Roulette_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Subject best = Ruletka();
                MessageBox.Show("Wybrano" + best.Value, "Ruletka", MessageBoxButton.OK, MessageBoxImage.Information);
                population = new Population(best, (UInt32)population.Subjects.Length);
                population.Environment = environment;
                populacja++;
                ShowPopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //to nie działa
        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                MessageBox.Show(openFileDialog.FileName, "Plik", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //to nie działa
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                MessageBox.Show(openFileDialog.FileName, "Plik", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //ilość powtórzeń (uruchomienia programu)
        private void Evolute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UInt32 ilosc = Convert.ToUInt32(quantity.Text);

                double cross_chanse = Convert.ToDouble(crossChance.Text);
                double subject_chance = Convert.ToDouble(subjectChance.Text);
                double gen_chance = Convert.ToDouble(genChance.Text);

                if (ilosc > 0)
                {
                    string[] file_text = new string[ilosc];

                    for(UInt32 i = 0; i < ilosc; i++)
                    {

                        population = population.Cross(cross_chanse);
                        population = population.Mutate(subject_chance, gen_chance);

                        Subject best = Ruletka();
                        //MessageBox.Show("Wybrano" + best.Value, "Ruletka", MessageBoxButton.OK, MessageBoxImage.Information);


                        file_text[i] = $"{population.Environment.Check(best)}\t{best.Value}";

                        population = new Population(best, (UInt32)population.Subjects.Length);
                        population.Environment = environment;
                        populacja++;

                    }
                    //zapis do pliku
                    File.AppendAllLines(System.IO.Path.Combine(@"C:\Users\karol\Desktop", "text.txt"), file_text);

                    //MessageBox.Show(file_text.Last(), "Info", MessageBoxButton.OK, MessageBoxImage.None);
                }
            
                else throw new Exception("Zła ilość!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
