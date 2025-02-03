using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Localization;

namespace TesteApiItau.Models
{
    public class Estatistica
    {
        public int count { get; set; }
        private double sum { get; set; }
        private double avg { get; set; }
        private double min { get; set; }
        private double max { get; set; }
    }
}
