using ConsoleApp3.Models;
using ConsoleApp3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3.Threading
{
    public class ParallelThreading
    {

        private List<PersonModel> RandomData(int range)
        {
            List<PersonModel> personModels = new List<PersonModel>();
            for (int i = 0; i < range; i++)
            {
                var person = new PersonModel()
                {
                    NumberIdentification = i,
                    Age = Convert.ToInt32(GenerateString.RandomNumbers(2)),
                    DateOfBirth = GenerateString.GenerateDateTime(),
                    LastName = GenerateString.RandomString(10),
                    Name = GenerateString.RandomString(10),
                    City = GenerateString.RandomString(10)
                };
                personModels.Add(person);
            }
            return personModels;
        }

        private List<PersonModel> RandomDataWithSpecificSameData(int range, string city)
        {
       
            List<PersonModel> personModels = new List<PersonModel>();
            for (int i = 0; i < range; i++)
            {
                var person = new PersonModel();
                if (GenerateString.random.Next(range - 1) % 2 == 0)
                {
                    person = new PersonModel()
                    {
                        NumberIdentification = i,
                        Age = Convert.ToInt32(GenerateString.RandomNumbers(2)),
                        DateOfBirth = GenerateString.GenerateDateTime(),
                        LastName = GenerateString.RandomString(10),
                        Name = GenerateString.RandomString(10),
                        City = city
                    };
                }
                else
                {
                    person = new PersonModel()
                    {
                        NumberIdentification = i,
                        Age = Convert.ToInt32(GenerateString.RandomNumbers(2)),
                        DateOfBirth = GenerateString.GenerateDateTime(),
                        LastName = GenerateString.RandomString(10),
                        Name = GenerateString.RandomString(10),
                        City = GenerateString.RandomString(10),                        
                    };
                }
                personModels.Add(person);
            }
            return personModels;
        }



        #region ParallelInvoke
        public void ParallelInvoke(int range)
        {
            Parallel.Invoke(Task1, Task2);
        }

        private void Task1()
        {
            Console.WriteLine("Task 1 starting");
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 ending");
        }

        private void Task2()
        {
            Console.WriteLine("Task 2 starting");
            Thread.Sleep(1000);
            Console.WriteLine("Task 2 ending");
        }

        #endregion

        #region ParallelForEach
        public void ParallelForEach(int range)
        {
            var xx = RandomData(range);
            int crash = 0;
            var xs = Parallel.ForEach(xx,(x, state) =>
            {
                WorkOnItem(x.LastName);
                crash++;
                if(crash == 22)
                {
                    state.Stop();
                }

                //if(crash == 23)
                //{
                //    throw new Exception("something");
                //}

                if(state.IsExceptional)
                {
                    Console.WriteLine("Exceasdadption");
                    state.Break();
                }                
                
            });
            
        }

        static void WorkOnItem(object item)
        {
            Console.WriteLine("Started working on: " + item);
            Thread.Sleep(100);
            Console.WriteLine("Finished working on: " + item);
        }
        #endregion

        #region AsParallelLINQ

        public List<PersonModel> InitializeDataForParallelData(int range)
        {
            return RandomDataWithSpecificSameData(range, "Oradea");
        }

        public void AsParallel(List<PersonModel> data)
        {
            var result = data
                .AsParallel()
                .Where(c => c.City == "Oradea")
                .Select(c => c);
            foreach (var person in result)
            {
                Console.WriteLine($"{person.NumberIdentification} + {person.Name} + {person.City}");
            }
        }

        public void AsParallelAddingParallelization(List<PersonModel> data)
        {
            var result = data
                .AsParallel()
                .WithDegreeOfParallelism(8)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Where(c => c.City == "Oradea")
                .Select(c => c);
            foreach (var person in result)
            {
                Console.WriteLine($"{person.NumberIdentification} + {person.Name} + {person.City}");
            }
        }

        public void AsParallelAsOrdered(List<PersonModel> data)
        {
            var result = data
                .AsParallel()
                .AsOrdered()
                .Where(c => c.City == "Oradea")
                .Select(c => c);
            foreach (var person in result)
            {
                Console.WriteLine($"{person.NumberIdentification} + {person.Name} + {person.City}");
            }
        }

        public void WithoutAsParallel(List<PersonModel> data)
        {
            var result = data.Where(c => c.City == "Oradea").Select(c => c);
            foreach (var person in result)
            {
                Console.WriteLine($"{person.NumberIdentification} + {person.Name} + {person.City}");
            }
        }

        #endregion

    }
}
