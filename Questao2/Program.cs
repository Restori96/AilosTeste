using Newtonsoft.Json;


public class Program
{
    static readonly HttpClient client = new HttpClient();
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {

        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = int.MaxValue;

        while (currentPage <= totalPages)
        {
            string response = await client.GetStringAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}");

            var result = JsonConvert.DeserializeObject<Response>(response);
            totalPages = result.Total_pages;

            foreach (var match in result.Data)
            {
                if (match.Team1.ToString() == team)
                {
                    totalGoals += int.Parse(match.Team1goals.ToString());
                }
              
            }
            currentPage++;
        }
        currentPage = 1;
        totalPages = int.MaxValue;
        while (currentPage <= totalPages)
        {
            string response = await client.GetStringAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}");

            var result = JsonConvert.DeserializeObject<Response>(response);
            totalPages = result.Total_pages;

            foreach (var match in result.Data)
            {
                if (match.Team2.ToString() == team)
                {
                    totalGoals += int.Parse(match.Team2goals.ToString());
                }
             
            }
            currentPage++;
        }
        return totalGoals;
    }
    public class Response
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<Games> Data { get; set; }
    }
    public class Games
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public string Round { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public string Team1goals { get; set; }
        public string Team2goals { get; set; }
    }

  
  

}