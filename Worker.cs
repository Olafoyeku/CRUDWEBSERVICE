using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConsumeAPI
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
     
        private HttpClient client;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
          
        }
        public class StudentViewModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Nullable<int> Age { get; set; }
        }



       
        public HttpClient client1 = new HttpClient();

        public void ShowStudent(StudentViewModel student)
        {
            Console.WriteLine($"First Name: {student.FirstName}\tLast Name: " +
                $"{student.LastName}\tAge: {student.Age}");
        }

        public async Task<Uri> CreateStudentAsync(StudentViewModel st)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Student/CreateStudent", st);
            response.EnsureSuccessStatusCode();

            StudentViewModel stu = new StudentViewModel();
            if (response.IsSuccessStatusCode)
            {
               // stu = await response1.Content.ReadAsAsync<StudentViewModel>();

                
             
                stu = await response.Content.ReadAsAsync<StudentViewModel>();
                //response.Headers.Location = new Uri(client.BaseAddress, String.Format("https://localhost:44339/api/Student/GetStudentByID/{0}", stu.Id));
                // return URI of the created resource.
                return response.Headers.Location;
            }
            return response.Headers.Location;

    
           
        }

        public async Task<StudentViewModel> GetStudentAsync(int id)
        {
            StudentViewModel stu = null;
            HttpResponseMessage response = await client.GetAsync($"api/Student/GetStudentById/{id}", (HttpCompletionOption)id);
            if (response.IsSuccessStatusCode)
            {
                stu = await response.Content.ReadAsAsync<StudentViewModel>();
            }
            return stu;
        }

        public async Task<StudentViewModel> UpdateStudentAsync(int id, StudentViewModel st)
        {
            HttpResponseMessage response4 = await client.PostAsJsonAsync(
                $"api/Student/UpdateStudent/{id}", st);
            response4.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            st = await response4.Content.ReadAsAsync<StudentViewModel>();
            return st;
        }

        public async Task<HttpStatusCode> DeleteStudentAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/Student/DeleteStudent/{id}");
            return response.StatusCode;
        }




        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var handler = new HttpClientHandler
            {
                //CookieContainer = cookieJar,
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44339/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));



            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                //var responseTask1 = client.GetAsync("https://localhost:44339/api/Student");
                //responseTask1.Wait();

                //var result = responseTask1.Result;
                //if (result.IsSuccessStatusCode)
                //{
                //    _logger.LogInformation("Student list retrieved successfully, Status Code {StatusCode}", result.StatusCode);


                //    var responseTask2 = client.GetAsync("https://localhost:44339/api/Student/1");
                //    responseTask2.Wait();
                //    var result2 = responseTask2.Result;

                //    if (result2.IsSuccessStatusCode)
                //    {
                //        _logger.LogInformation("Student retrieved successfully, Status Code {StatusCode}", result2.StatusCode);

                //        StudentViewModel student = new StudentViewModel
                //        {
                //            FirstName = "Matthew",
                //            LastName = "John",
                //            Age = 3
                //        };

                //        var responseTask3 = await client.PostAsJsonAsync("Student", student);
                //        postTask.Wait();

                //        var result = postTask.Result;
                //        if (result.IsSuccessStatusCode)
                //        {
                //            return RedirectToAction("Index");
                //        }

                //    }


                //}

                //else
                //{
                //    _logger.LogInformation("List was not retrieved successfully, Status Code {StatusCode}", result.StatusCode);
                //}


                

                try
                {
                    int i = 2;
                    // Create a new product
                    StudentViewModel stv = new StudentViewModel
                    {
                        
                        FirstName = "hannah",
                        Age = 100,
                        LastName = "montanna"
                    };



                     await CreateStudentAsync(stv);

                    _logger.LogInformation($"Student Created Successfully");

                    // Get the product
                    stv = await GetStudentAsync(3);
                    ShowStudent(stv);

                    // Update the product
                    _logger.LogInformation("Updating price...");
                    stv.FirstName = "Mr";
                    stv.LastName = "Bean";
                    stv.Age = 12;

                    await UpdateStudentAsync(i++, stv);
                    _logger.LogInformation("Updated successfully");

                    // Delete the product
                    var statusCode = await DeleteStudentAsync(i++);
                    _logger.LogInformation($"Deleted (HTTP Status = {(int)statusCode})");
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                await Task.Delay(18000000, stoppingToken);
            }
        }
    }
}
