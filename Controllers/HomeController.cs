using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APICoreConsumer.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace APICoreConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<TodoItem> tdis = new List<TodoItem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44348/api/TodoItems"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    tdis = JsonConvert.DeserializeObject<List<TodoItem>>(apiResponse);
                }
            }

            return View(tdis);
        }


        public ViewResult GetTodoItem() => View();

        [HttpPost]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            TodoItem todoItem = new TodoItem();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:44348/api/TodoItems/" + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode) 
            { 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todoItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);
                }
            }
            return View(todoItem);
        }

    }
}
