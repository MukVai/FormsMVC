using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FormsApp.Controllers;

public class PersonController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = "https://localhost:7055/api/PersonApi";

    public PersonController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync(_apiUrl);
        if (!response.IsSuccessStatusCode)
            return View(new List<Person>());

        var json = await response.Content.ReadAsStringAsync();
        var persons = JsonSerializer.Deserialize<List<Person>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(persons);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Person person)
    {
        var json = JsonSerializer.Serialize(person);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View(person);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<Person>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(person);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Person person)
    {
        var json = JsonSerializer.Serialize(person);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View(person);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<Person>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(person);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
        return RedirectToAction("Index");
    }
}
