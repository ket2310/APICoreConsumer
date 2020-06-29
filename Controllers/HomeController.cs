using APICoreConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace APICoreConsumer.Controllers
{
    public class HomeController : Controller
    {
        const string myUrl = "https://localhost:44348/api/TodoItems/";

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
                using (var response = await httpClient.GetAsync(myUrl))
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
            var response = await httpClient.GetAsync(myUrl + id).ConfigureAwait(false);

            if (response.IsSuccessStatusCode) 
            { 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todoItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);
                }
            }
            return View(todoItem);
        }

        public ViewResult AddTodoItem() => View();

        [HttpPost]
        public async Task<IActionResult> AddTodoItem(TodoItem tdi)
        {
            TodoItem todoItem = new TodoItem();
            using(var cli = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(tdi), System.Text.Encoding.UTF8, "application/json");

                using (var response = await cli.PostAsync(myUrl + "CreateTodo/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todoItem = JsonConvert.DeserializeObject<TodoItem>(apiResponse);
                }
            }
            return View(todoItem);
        }

    }
}
